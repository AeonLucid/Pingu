using System.Xml.Linq;

namespace Pingu.Net.Message
{
    internal class IncomingMessage
    {
        public IncomingMessage(XElement xmlDocument)
        {
            Document = xmlDocument;
            Size = xmlDocument.ToString(SaveOptions.DisableFormatting).Length;
            Header = xmlDocument.Name.LocalName;
        }

        public XElement Document { get; }

        public int Size { get; }

        public string Header { get; }
    }
}
