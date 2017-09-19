using System.Xml.Linq;
using Pingu.Net.Handler;
using Pingu.Pingu;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerUserList : Composer
    {
        public OutgoingMessage Compose()
        {
            XElement xmlMessage = new XElement(Header);

            foreach (Player player in PlayerRoom.Players.Values)
            {
                xmlMessage.Add(
                    new XElement("player",
                        new XAttribute("name", player.Username),
                        new XAttribute("skill", "0/0/0/0/0"),
                        new XAttribute("state", (int)player.Status)
                    )
                );
            }

            OutgoingMessage outgoingMessage = new OutgoingMessage(xmlMessage);

            return outgoingMessage;
        }
    }
}
