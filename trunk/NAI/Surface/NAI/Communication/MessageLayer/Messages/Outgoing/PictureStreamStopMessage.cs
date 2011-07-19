
namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal sealed class PictureStreamStopMessage : OutgoingBodyLessMessage
    {
        public PictureStreamStopMessage()
            : base(MessageProtocol.PICTURE_STREAM_STOP)
        { }

    }
}
