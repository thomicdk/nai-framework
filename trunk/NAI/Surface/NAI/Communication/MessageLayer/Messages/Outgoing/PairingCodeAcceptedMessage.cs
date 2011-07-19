
namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal sealed class PairingCodeAcceptedMessage : OutgoingBodyLessMessage
    {
        public PairingCodeAcceptedMessage()
            : base(MessageProtocol.PAIRING_CODE_ACCEPTED)
        { }
    }
}
