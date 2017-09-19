using Pingu.Net.Message;

namespace Pingu.Net.Handler
{
    internal interface IMessageEvent
    {

        void HandleMessage(IncomingMessage message, ClientHandler clientHandler);

    }
}
