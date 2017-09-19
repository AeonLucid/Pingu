using System.Xml.Linq;
using Pingu.Net.Handler;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerConfig : Composer
    {
        public OutgoingMessage Compose()
        {
            XElement xmlMessage = new XElement(Header,
                new XAttribute("adConfigUrl", ""),
                new XAttribute("badWordsUrl", "https://aeonlucid.com/projects/bomberman/data/badwords.php"),
                new XAttribute("replacementChar", "*"),
                new XAttribute("deleteLine", true),
                new XAttribute("floodLimit", 0)
            );

            OutgoingMessage outgoingMessage = new OutgoingMessage(xmlMessage);

            return outgoingMessage;
        }
    }
}
