
namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal sealed class PincodeRejectedMessage : OutgoingBodyLessMessage
    {
        public PincodeRejectedMessage()
            : base(MessageProtocol.PINCODE_REJECTED)
        { }
    }
}
