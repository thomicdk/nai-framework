
namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class RequestCalibrationMessage : IncomingBodyLessMessage
    {
        public RequestCalibrationMessage()
            : base(MessageProtocol.REQUEST_CALIBRATION)
        { }

    }
}
