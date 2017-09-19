using System.Xml.Linq;
using Pingu.Net.Handler;
using Pingu.Pingu;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerRequest : Composer
    {
        public OutgoingMessage Compose(Player fromPlayer)
        {
            XElement xmlMessage = new XElement(Header,
                new XAttribute("name", fromPlayer.Username)
            );

            OutgoingMessage outgoingMessage = new OutgoingMessage(xmlMessage);

            return outgoingMessage;
        }
    }
}
