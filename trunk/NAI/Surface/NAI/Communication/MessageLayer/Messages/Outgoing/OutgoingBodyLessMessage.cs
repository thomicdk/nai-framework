
namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal abstract class OutgoingBodyLessMessage : OutgoingMessage
    {
        public OutgoingBodyLessMessage(MessageProtocol command)
            : base(command)
        { }

        protected override byte[] PrepareMessageBody()
        { 
            return new byte[0];
        }
    }
}
