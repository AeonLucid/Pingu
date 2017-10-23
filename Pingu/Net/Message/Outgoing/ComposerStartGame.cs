using System.Xml.Linq;
using Pingu.Net.Handler;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerStartGame : Composer
    {
        public OutgoingMessage Compose(string opponent)
        {
            return new OutgoingMessage(new XElement(Header,
                new XAttribute("name", opponent)
            ));
        }
    }
}
