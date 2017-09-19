using System.Xml.Linq;
using Pingu.Net.Handler;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerWarning : Composer
    {
        // TODO: Finish

        public OutgoingMessage Compose(string name, string message)
        {
            XElement xmlMessage = new XElement(Header,
                new XAttribute("name", name),
                new XAttribute("msg", message)
            );

            OutgoingMessage outgoingMessage = new OutgoingMessage(xmlMessage);

            return outgoingMessage;
        }
    }
}
