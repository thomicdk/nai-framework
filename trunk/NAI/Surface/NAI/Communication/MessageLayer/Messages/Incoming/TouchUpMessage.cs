
namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class TouchUpMessage : TouchEventMessage
    {
        public TouchUpMessage()
            : base(MessageProtocol.TOUCH_UP)
        { }

    }
}
