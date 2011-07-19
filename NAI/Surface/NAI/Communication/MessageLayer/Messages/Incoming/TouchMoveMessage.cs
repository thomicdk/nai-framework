
namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class TouchMoveMessage : TouchEventMessage
    {
        public TouchMoveMessage()
            : base(MessageProtocol.TOUCH_MOVE)
        { }
    }
}
