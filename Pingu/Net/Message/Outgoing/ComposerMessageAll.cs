using System.Xml.Linq;
using Pingu.Net.Handler;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerMessageAll : Composer
    {
        public OutgoingMessage Compose(string name, string message)
        {
            return new OutgoingMessage(new XElement(Header,
                new XAttribute("name", name),
                new XAttribute("msg", message)
            ));
        }
    }
}
