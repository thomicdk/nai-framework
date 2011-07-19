
namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class ClientResumedMessage : IncomingBodyLessMessage
    {
        public ClientResumedMessage()
            : base(MessageProtocol.CLIENT_RESUMED)
        { }
    }
}
