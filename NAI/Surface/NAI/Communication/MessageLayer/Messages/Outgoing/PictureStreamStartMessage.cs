
namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal sealed class PictureStreamStartMessage : OutgoingBodyLessMessage
    {
        public PictureStreamStartMessage()
            : base(MessageProtocol.PICTURE_STREAM_START)
        { }
    }
}
