using System.Xml.Linq;

namespace Pingu.Net.Message
{
    class IncomingMessage
    {
        private readonly XElement _xmlDocument;

        public int Size { get; }
        public string Header { get; }

        public IncomingMessage(XElement xmlDocument)
        {
            _xmlDocument = xmlDocument;
            Size = _xmlDocument.ToString(SaveOptions.DisableFormatting).Length;
            Header = _xmlDocument.Name.LocalName;
        }

        public XElement GetDocument()
        {
            return _xmlDocument;
        }

    }
}
