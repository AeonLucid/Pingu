using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;

namespace Pingu.Net
{
    internal class Server : IDisposable
    {
        public ManualResetEvent KeepRunning = new ManualResetEvent(false);
        public bool KeepLooping = true;

        private readonly ManualResetEvent _acceptResetEvent;

        private readonly Logger _logger;
        private readonly Socket _socket;

        public Server()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _logger.Debug("Initializing Server..");

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, 5898));
            _socket.Listen(10);

            _acceptResetEvent = new ManualResetEvent(true);

            new Thread(AcceptConnection)
            {
                IsBackground = true
            }.Start();
        }

        private void AcceptConnection()
        {
            if (_socket != null && _socket.IsBound)
            {
                while (KeepLooping)
                {
                    _acceptResetEvent.Reset();

                    _logger.Debug("Waiting for new connection..");
                    _socket.BeginAccept(AcceptCallback, null);

                    _acceptResetEvent.WaitOne();
                }
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            if (_socket != null && _socket.IsBound)
            {
                Socket incomingClientSocket = _socket.EndAccept(ar);

                _logger.Debug($"Accepting connection from {incomingClientSocket.RemoteEndPoint}..");
                _acceptResetEvent.Set();

                ClientHandler clientHandler = new ClientHandler(incomingClientSocket);
                new Thread(clientHandler.Handle)
                {
                    IsBackground = true
                }.Start();
            }
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}
