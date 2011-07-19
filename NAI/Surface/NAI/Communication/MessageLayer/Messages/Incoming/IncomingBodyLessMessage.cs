
namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal abstract class IncomingBodyLessMessage : IncomingMessage
    {
        public IncomingBodyLessMessage(MessageProtocol command)
            : base(command)
        { }

        protected override void ParseMessage(byte[] messageBody)
        { }
    }
}
