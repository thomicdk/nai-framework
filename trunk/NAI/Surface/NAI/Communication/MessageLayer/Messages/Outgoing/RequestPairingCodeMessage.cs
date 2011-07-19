
using System.IO;
namespace NAI.Communication.MessageLayer.Messages.Outgoing
{
    internal sealed class RequestPairingCodeMessage : OutgoingMessage
    {
        private byte _pinCodeLength;
        private byte _colorCodeLength;

        public RequestPairingCodeMessage(byte pinCodeLength, byte colorCodeLength)
            : base(MessageProtocol.REQUEST_PAIRING_CODE)
        {
            this._pinCodeLength = pinCodeLength;
            this._colorCodeLength = colorCodeLength;
        }

        protected override byte[] PrepareMessageBody()
        {
            MemoryStream ms = new MemoryStream();
            // Length of PIN_CODE
            ms.WriteByte(_pinCodeLength);
            
            // Length of COLOR_CODE
            ms.WriteByte(_colorCodeLength);
            return ms.ToArray();
        }
    }
}
