using Pingu.Net.Handler;
using Pingu.Pingu;

namespace Pingu.Net.Message.Incoming
{
    internal class MessageEventMessageAll : IMessageEvent
    {
        // <msgAll name="AeonLucid" msg="hallo  " />
        public void HandleMessage(IncomingMessage message, ClientHandler clientHandler)
        {
            var name = message.Document?.Attribute("name")?.Value;
            if (name == null || !name.Equals(clientHandler.Username))
            {
                // Spoof attempt
                return;
            }

            var chatMessage = message.Document?.Attribute("msg")?.Value;
            if (chatMessage == null)
            {
                // Empty chat message
                return;
            }

            if (chatMessage.Length > 70)
            {
                chatMessage = chatMessage.Substring(0, 70);
            }

            PlayerRoom.SendChatMessage(clientHandler.Username, chatMessage);
        }
    }
}
