using System;
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
    internal class ClientHandler
    {
        public string Username;

        private readonly Logger _logger;
        private readonly Socket _clientSocket;
        private readonly byte[] _buffer;

        public ClientHandler(Socket clientSocket)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _logger.Debug("Initializing ClientHandler..");

            _clientSocket = clientSocket;
            _buffer = new byte[1024];
        }

        public void Handle()
        {
            WaitForData();
        }

        private void WaitForData()
        {
            if (_clientSocket != null && _clientSocket.Connected)
            {
                _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int bytesReceived = _clientSocket.EndReceive(ar);

                if (bytesReceived == 0)
                {
                    if (Username != null)
                    {
                        _logger.Info($"'{Username}' has disconnected from the server.");

                        PlayerRoom.RemovePlayer(Username);
                    }

                    Disconnect();
                    return;
                }

                byte[] packetBytes = new byte[bytesReceived];
                Buffer.BlockCopy(_buffer, 0, packetBytes, 0, bytesReceived);

                string packetString = Encoding.Default.GetString(packetBytes);
                try
                {
                    XElement packetXml = XElement.Parse(packetString.Substring(0, packetString.Length - 1)); // TODO: Make sure entire packet is here /0

                    if (packetXml.Name.LocalName.Equals("policy-file-request"))
                    {
                        _logger.Debug("Received policy request");
                        _clientSocket.Send(Encoding.Default.GetBytes(CrossDomainPolicy.GetPolicy()));
                    }
                    else
                    {
                        PacketHandler.HandleIncomingMessage(new IncomingMessage(packetXml), this);
                    }
                }
                catch (XmlException)
                {
                    // ignored
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Client generated an exception.", exception);
            }
            finally
            {
                WaitForData();
            }
        }

        public void SendMessage(OutgoingMessage outgoingMessage)
        {
            _clientSocket.Send(outgoingMessage.GetBytes());

            _logger.Debug(Username == null
                ? $"Sending {outgoingMessage.GetHeader()}."
                : $"[{Username}] Sending {outgoingMessage.GetHeader()}.");
        }

        public void SendMessages(OutgoingMessage[] outgoingMessages)
        {
            foreach (OutgoingMessage outgoingMessage in outgoingMessages)
                SendMessage(outgoingMessage);
        }

        public void Disconnect(string reason = null)
        {
            if(reason != null)
                _logger.Error($"Client disconnected with reason: {reason}");

            _clientSocket?.Disconnect(false);
            _clientSocket?.Dispose();
        }

        public Player GetPlayer()
        {
            if (Username != null)
            {
                return PlayerRoom.GetPlayer(Username);
            }

            throw new NullReferenceException("Username was not set.");
        }
    }
}
