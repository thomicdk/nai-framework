
namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class CalibrationAcceptedMessage : IncomingBodyLessMessage
    {
        public CalibrationAcceptedMessage()
            : base(MessageProtocol.CALIBRATION_ACCEPTED)
        { }
    }
}
