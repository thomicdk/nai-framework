using System.IO;

namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal abstract class OutgoingMessage : Message
    {
        public OutgoingMessage(MessageProtocol command)
            : base(command)
        { }

        protected abstract byte[] PrepareMessageBody();

        public byte[] PrepareMessage()
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)this.Command);
            byte[] msgBody = PrepareMessageBody();
            ms.Write(msgBody, 0, msgBody.Length);
            return ms.ToArray();
        }
    }
}
