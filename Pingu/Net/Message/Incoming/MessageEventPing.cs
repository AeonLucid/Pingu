using Pingu.Net.Handler;
using Pingu.Net.Message.Outgoing;

namespace Pingu.Net.Message.Incoming
{
    internal class MessageEventPing : IMessageEvent
    {
        public void HandleMessage(IncomingMessage message, ClientHandler clientHandler)
        {
            clientHandler.SendMessage(PacketHandler.GetComposer<ComposerPong>().Compose());
        }
    }
}
