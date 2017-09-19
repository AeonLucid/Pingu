using Pingu.Net.Handler;

namespace Pingu.Net.Message.Incoming
{
    internal class MessageEventBeat : IMessageEvent
    {
        public void HandleMessage(IncomingMessage message, ClientHandler clientHandler)
        {
            // Just checking if connection is alive
        }
    }
}
