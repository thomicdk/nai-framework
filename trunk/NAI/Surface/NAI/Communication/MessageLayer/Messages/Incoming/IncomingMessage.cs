using System.IO;
using System;
using System.Diagnostics;

namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal abstract class IncomingMessage : Message
    {
        public IncomingMessage(MessageProtocol command) : base(command)
        { }

        protected abstract void ParseMessage(byte[] incomingMessageBytes);

        public static IncomingMessage Parse(byte[] incomingMessageBytes)
        {
            IncomingMessage im = GetMessageType(incomingMessageBytes[0]);
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Incoming Message: " + im.GetType().Name);
            if (incomingMessageBytes.Length > 1)
            {
                byte[] messageBody = new byte[incomingMessageBytes.Length-1];
                Array.Copy(incomingMessageBytes, 1, messageBody, 0, incomingMessageBytes.Length - 1);
                im.ParseMessage(messageBody);
            }
            return im;
        }

        private static IncomingMessage GetMessageType(byte command)
        {
            switch (command)
            {
                // Response
                // --------------
                case (byte)MessageProtocol.CREDENTIALS:
                    return new CredentialsMessage();

                case (byte)MessageProtocol.PIN_CODE:
                    return new PinCodeMessage();

                case (byte)MessageProtocol.COLOR_CODE:
                    return new ColorCodeMessage();

                case (byte)MessageProtocol.CALIBRATION_ACCEPTED:
                    return new CalibrationAcceptedMessage();

                // Event
                // --------------
                case (byte)MessageProtocol.TOUCH_DOWN:
                    return new TouchDownMessage();

                case (byte)MessageProtocol.TOUCH_MOVE:
                    return new TouchMoveMessage();

                case (byte)MessageProtocol.TOUCH_UP:
                    return new TouchUpMessage();

                case (byte)MessageProtocol.CLIENT_PAUSED:
                    return new ClientPausedMessage();

                case (byte)MessageProtocol.CLIENT_RESUMED:
                    return new ClientResumedMessage();

                // Request
                // --------------
                case (byte)MessageProtocol.REQUEST_CALIBRATION:
                    return new RequestCalibrationMessage();

                default:
                    throw new NotSupportedException(string.Format("Unknown protocol command: '{0}'", command));
            }
        }


    }
}
