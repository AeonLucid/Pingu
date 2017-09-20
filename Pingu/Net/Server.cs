using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;

namespace Pingu.Net
{
    internal class Server : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Socket _socket;

        private readonly ManualResetEvent _acceptResetEvent;

        private readonly List<ClientHandler> _clients;

        private bool _keepLooping;

        private bool _started;

        private bool _disposing;

        public Server()
        {
            Logger.Debug("Initializing Server..");

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, 5898));
            _socket.Listen(10);
            _acceptResetEvent = new ManualResetEvent(true);
            _clients = new List<ClientHandler>();
            _keepLooping = false;
            _started = false;
            _disposing = false;
        }

        public void Start()
        {
            if (_started)
            {
                throw new Exception("Server has already been started.");
            }

            _keepLooping = true;
            _started = true;

            new Thread(AcceptConnections)
            {
                IsBackground = true
            }.Start();
        }

        public void Stop()
        {
            _keepLooping = false;
        }

        private void AcceptConnections()
        {
            if (_socket == null || !_socket.IsBound || _disposing)
            {
                return;
            }

            while (_keepLooping)
            {
                Logger.Debug("Waiting for new connection..");

                _acceptResetEvent.Reset();
                _socket.BeginAccept(AcceptConnectionCallback, null);
                _acceptResetEvent.WaitOne();
            }
        }

        private void AcceptConnectionCallback(IAsyncResult ar)
        {
            if (_socket == null || !_socket.IsBound || _disposing)
            {
                return;
            }

            var incomingClientSocket = _socket.EndAccept(ar);

            Logger.Debug($"Accepting connection from {incomingClientSocket.RemoteEndPoint}..");
            _acceptResetEvent.Set();

            var clientHandler = new ClientHandler(this, incomingClientSocket);

            new Thread(AddClient(clientHandler).Listen)
            {
                IsBackground = true
            }.Start();
        }

        public ClientHandler AddClient(ClientHandler client)
        {
            if (_clients.Contains(client))
            {
                throw new ArgumentException("Clients list already contains the specified client.");
            }

            _clients.Add(client);

            return client;
        }

        public void RemoveClient(ClientHandler client)
        {
            if (!_clients.Contains(client))
            {
                return;
            }

            _clients.Remove(client);
        }

        public void Dispose()
        {
            _disposing = true;
            _socket?.Dispose();
            _acceptResetEvent?.Dispose();

            foreach (var client in _clients)
            {
                client.Dispose(false);
            }
        }
    }
}
