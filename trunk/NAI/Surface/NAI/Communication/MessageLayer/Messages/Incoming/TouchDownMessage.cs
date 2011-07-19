
namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class TouchDownMessage : TouchEventMessage
    {
        public TouchDownMessage()
            : base(MessageProtocol.TOUCH_DOWN)
        { }
    }
}
