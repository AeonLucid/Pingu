using System;
using Pingu.Net.Handler;
using Pingu.Pingu;

namespace Pingu.Net.Message.Incoming
{
    internal class MessageEventMessageAll : IMessageEvent
    {
        // <msgAll name="AeonLucid" msg="hallo  " />
        public void HandleMessage(IncomingMessage message, ClientHandler clientHandler)
        {
            string name = message.GetDocument().Attribute("name").Value;

            if (name.Equals(clientHandler.Username))
            {
                string chatMessage = message.GetDocument().Attribute("msg").Value;

                if (chatMessage.Length > 70)
                    chatMessage = chatMessage.Substring(0, 70);

                PlayerRoom.SendChatMessage(clientHandler.Username, chatMessage);
            }
            else
            {
                // TODO: Send error
                Console.WriteLine("ERROR ERROR ERROR");
            }
        }
    }
}
