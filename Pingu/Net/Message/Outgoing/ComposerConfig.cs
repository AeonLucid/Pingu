using System.Xml.Linq;
using Pingu.Net.Handler;

namespace Pingu.Net.Message.Outgoing
{
    internal class ComposerConfig : Composer
    {
        public OutgoingMessage Compose()
        {
            return new OutgoingMessage(new XElement(Header,
                new XAttribute("adConfigUrl", ""),
                new XAttribute("badWordsUrl", "https://aeonlucid.com/projects/bomberman/data/badwords.php"),
                new XAttribute("replacementChar", "*"),
                new XAttribute("deleteLine", true),
                new XAttribute("floodLimit", 0)
            ));
        }
    }
}
