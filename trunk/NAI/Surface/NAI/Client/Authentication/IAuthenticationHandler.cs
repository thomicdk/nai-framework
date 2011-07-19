
namespace NAI.Client.Authentication
{
    public interface IAuthenticationHandler
    {
        /// <summary>
        /// Read/parse credentials from a Stream passed as an argument
        /// </summary>
        /// <param name="inputStream">The stream to read the credentials from</param>
        /// <returns>The ClientCredentials object representing the credentials read from the stream</returns>
        ClientCredentials ParseCredentialsMessage(byte[] messageBody);

        /// <summary>
        /// Determines whether the credentials passed as an argument can be authenticated
        /// </summary>
        /// <param name="credentials">The credentials to be authenticated</param>
        /// <returns>True, if authenticated. False otherwise</returns>
        bool Authenticate(ClientCredentials credentials);
    }
}
