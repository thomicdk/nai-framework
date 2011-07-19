using System.IO;

namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal sealed class FrameMessage : OutgoingMessage
    {
        private byte[] _frame;

        public FrameMessage(byte[] frame) : base(MessageProtocol.FRAME)
        {
            this._frame = frame;
        }

        protected override byte[] PrepareMessageBody()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(System.Net.IPAddress.HostToNetworkOrder(_frame.Length));
            bw.Write(_frame);
            return ms.ToArray();
        }
    }
}
