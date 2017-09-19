using System.Xml.Linq;
using Pingu.Net.Handler;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerPong : Composer
    {
        public OutgoingMessage Compose()
        {
            return new OutgoingMessage(new XElement(Header));
        }
    }
}
