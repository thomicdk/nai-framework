using System.Text;
using System.IO;
using System.Net;

namespace NAI.Client.Authentication
{
    public class BasicAuthenticationHandler : IAuthenticationHandler
    {
        #region IAuthenticationHandler Members
        
        public ClientCredentials ParseCredentialsMessage(byte[] messageBody)
        {
            string userId = Encoding.UTF8.GetString(messageBody);
            return new BasicClientCredentials(userId);
        }

        public bool Authenticate(ClientCredentials credentials)
        {
            if (credentials.UserId.StartsWith("Joe"))
                return false;

            // Accept everyone! (except Joe :-)
            return true;
        }

        #endregion
    }
}
