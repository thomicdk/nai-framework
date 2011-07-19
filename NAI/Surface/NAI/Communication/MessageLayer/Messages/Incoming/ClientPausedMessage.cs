
namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class ClientPausedMessage : IncomingBodyLessMessage
    {
        public ClientPausedMessage()
            : base(MessageProtocol.CLIENT_PAUSED)
        { }
    }
}
