using System.IO;
using System.Diagnostics;
using System.Text;
using NAI.Client.Pairing;

namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class PinCodeMessage : PairingCodeMessage
    {
        public PairingCode PinCode 
        {
            get
            {
                return _pairingCode;
            }
        }

        public PinCodeMessage()
            : base(MessageProtocol.PIN_CODE)
        { }

        protected override PairingCode ParsePairingCode(byte[] messageBody)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(messageBody));
            byte pinCodeLength = br.ReadByte();
            Debug.WriteLine("Parsing PIN code. Length " + pinCodeLength);
            byte[] pinBytes = br.ReadBytes(pinCodeLength);
            StringBuilder pin = new StringBuilder(PairingState.PIN_CODE_LENGTH);
            foreach (byte n in pinBytes)
            {
                pin.Append(n.ToString());
            }
            return new PairingCode(PairingCodeType.PIN_CODE,pin.ToString());
        }

        public override string ToString()
        {
            return this.PinCode.ToString();
        }
    }
}
