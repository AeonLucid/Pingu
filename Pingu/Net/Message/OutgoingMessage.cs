using System.Text;
using System.Xml.Linq;

namespace Pingu.Net.Message
{
    internal class OutgoingMessage
    {
        public OutgoingMessage(XElement xmlDocument)
        {
            Document = xmlDocument;
            Header = xmlDocument.Name.LocalName;
            Message = xmlDocument.ToString(SaveOptions.DisableFormatting) + "\0";
            MessageBytes = Encoding.ASCII.GetBytes(Message);
        }

        public XElement Document { get; }
        
        public string Header { get; }

        public string Message { get; }

        public byte[] MessageBytes { get; }
    }
}
