using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NLog;
using Pingu.Net.Handler;
using Pingu.Net.Message;
using Pingu.Pingu;
using Pingu.Util;

namespace Pingu.Net
{
    internal class ClientHandler : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ClientHandler(Server server, Socket clientSocket)
        {
            Server = server;
            Logger.Debug("Initializing ClientHandler..");

            ClientSocket = clientSocket;
            SocketBuffer = new byte[1024];
            Disposing = false;
        }

        public bool Disposing { get; private set; }

        public Server Server { get; }

        private Socket ClientSocket { get; }

        private byte[] SocketBuffer { get; }

        public string Username { get; set; }

        public void Listen()
        {
            ReceiveData();
        }

        private void ReceiveData()
        {
            if (ClientSocket == null || !ClientSocket.Connected || Disposing)
            {
                return;
            }

            ClientSocket.BeginReceive(SocketBuffer, 0, SocketBuffer.Length, SocketFlags.None, ReceiveDataCallback, null);
        }

        private void ReceiveDataCallback(IAsyncResult ar)
        {
            if (ClientSocket == null || !ClientSocket.Connected || Disposing)
            {
                return;
            }

            try
            {
                var bytesReceived = ClientSocket.EndReceive(ar);
                if (bytesReceived == 0)
                {
                    Logger.Info($"[{Username ?? "Anonymous"}] has disconnected from the server.");

                    if (Username != null)
                    {
                        PlayerRoom.RemovePlayer(Username);
                    }

                    Disconnect();
                    return;
                }

                var packetBytes = new byte[bytesReceived];
                Buffer.BlockCopy(SocketBuffer, 0, packetBytes, 0, bytesReceived);

                var packetString = Encoding.ASCII.GetString(packetBytes);

                packetString = packetString.Substring(0, packetString.Length - 1); // TODO: Make sure entire packet is here /0
                packetString = Constants.PacketFix.Replace(packetString, match => // Fix packet starting with numbers
                {
                    var replacement = Constants.PacketFixMap[match.Groups["number"].Value];

                    return $"<{replacement} ";
                });

                try
                {
                    var packetXml = XElement.Parse(packetString);
                    if (packetXml.Name.LocalName.Equals("policy-file-request"))
                    {
                        Logger.Debug("Received policy request");

                        ClientSocket.Send(Encoding.ASCII.GetBytes(CrossDomainPolicy.GetPolicy()));
                    }
                    else
                    {
                        PacketHandler.HandleIncomingMessage(new IncomingMessage(packetXml), this);
                    }
                }
                catch (XmlException exception)
                {
                    Logger.Error(exception, $"[{Username ?? "Anonymous"}] Invalid xml was received: '{packetString}'.");
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, $"[{Username ?? "Anonymous"}] Client generated an exception.");
            }
            finally
            {
                ReceiveData();
            }
        }

        public void SendMessage(OutgoingMessage outgoingMessage)
        {
            if (ClientSocket == null || !ClientSocket.Connected || Disposing)
            {
                return;
            }

            ClientSocket.Send(outgoingMessage.MessageBytes);
            
            Logger.Debug($"[{Username ?? "Anonymous"}] Sending {outgoingMessage.Header}.");
        }

        public void SendMessage(IEnumerable<OutgoingMessage> outgoingMessages)
        {
            foreach (var message in outgoingMessages)
            {
                SendMessage(message);
            }
        }

        public void Disconnect(string error = null)
        {
            if (error != null)
            {
                Logger.Error($"[{Username ?? "Anonymous"}] Client disconnected with error: '{error}'");
            }

            ClientSocket?.Disconnect(false);
            ClientSocket?.Dispose();
            Dispose(true);
        }

        public Player GetPlayer()
        {
            if (Username == null)
            {
                throw new NullReferenceException("Username was not set.");
            }

            return PlayerRoom.GetPlayer(Username);
        }

        public void Dispose(bool removeFromList)
        {
            Disposing = true;

            if (removeFromList)
            {
                Server?.RemoveClient(this);
            }

            Dispose();
        }

        public void Dispose()
        {
            Disposing = true;
            ClientSocket?.Dispose();
        }
    }
}
