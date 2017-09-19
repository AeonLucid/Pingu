using System.Xml.Linq;
using Pingu.Net.Handler;
using Pingu.Pingu;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerPlayerNew : Composer
    {
        public OutgoingMessage Compose(Player player)
        {
            XElement xmlMessage = new XElement(Header,
                new XAttribute("name", player.Username),
                new XAttribute("skill", "0/0/0/0/0"), // TODO: Skill
                new XAttribute("state", (int)player.Status)
            );

            OutgoingMessage outgoingMessage = new OutgoingMessage(xmlMessage);

            return outgoingMessage;
        }
    }
}
