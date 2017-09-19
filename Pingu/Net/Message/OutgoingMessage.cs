using System.Text;
using System.Xml.Linq;

namespace Pingu.Net.Message
{
    internal class OutgoingMessage
    {
        private readonly XElement _xmlDocument;

        public OutgoingMessage(XElement xmlDocument)
        {
            _xmlDocument = xmlDocument;
        }

        public string GetHeader()
        {
            return _xmlDocument.Name.LocalName;
        }

        public string GetMessage()
        {
            return _xmlDocument.ToString(SaveOptions.DisableFormatting) + "\0";
        }

        public byte[] GetBytes()
        {
            return Encoding.Default.GetBytes(GetMessage());
        }
    }
}
