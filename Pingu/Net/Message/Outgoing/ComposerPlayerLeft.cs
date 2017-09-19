using System.Xml.Linq;
using Pingu.Net.Handler;
using Pingu.Pingu;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerPlayerLeft : Composer
    {
        public OutgoingMessage Compose(Player player)
        {
            XElement xmlMessage = new XElement(Header,
                new XAttribute("name", player.Username)
            );

            OutgoingMessage outgoingMessage = new OutgoingMessage(xmlMessage);

            return outgoingMessage;
        }
    }
}
