
using System.IO;
using System.Net;
using System.Text;
namespace NAI.Client.Authentication
{
    public class BasicClientCredentials : ClientCredentials
    {
        public BasicClientCredentials(string userId)
            : base(userId)
        { }
    }
}
