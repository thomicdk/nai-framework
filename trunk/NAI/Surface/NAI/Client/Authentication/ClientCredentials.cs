
namespace NAI.Client.Authentication
{
    public abstract class ClientCredentials
    {
        public string UserId { get; private set; }

        internal ClientCredentials(string userId)
        {
            this.UserId = userId;
        }

        public override string ToString()
        {
            return UserId;
        }
    }
}
