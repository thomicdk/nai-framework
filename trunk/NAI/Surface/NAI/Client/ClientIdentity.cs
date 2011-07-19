using Microsoft.Surface.Presentation;
using NAI.Client.Authentication;
using System.Text;
using NAI.UI.Controls;

namespace NAI.Client
{
    public sealed class ClientIdentity
    {
        public TagData TagData { get; internal set; }
        public ClientCredentials Credentials { get; internal set; }
        public IPersonalizedView PersonalizedView { get; internal set; }

        internal ClientIdentity() { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(TagData == null? "[No Tag]" : TagData.ToString());
            sb.AppendLine(Credentials.ToString());
            return sb.ToString();
        }
    }
}
