using System.Xml.Linq;
using Pingu.Net.Handler;
using Pingu.Pingu;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerPlayerLeft : Composer
    {
        public OutgoingMessage Compose(Player player)
        {
            var xmlMessage = new XElement(Header,
                new XAttribute("name", player.Username)
            );

            return new OutgoingMessage(xmlMessage);
        }
    }
}
