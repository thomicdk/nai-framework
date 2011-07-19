using System.IO;
using System;
using System.Linq;
using NAI.Client.Pairing;

namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal sealed class ColorCodeMessage : PairingCodeMessage
    {
        internal PairingCode ColorCode
        {
            get
            {
                return _pairingCode;
            }
        }

        public ColorCodeMessage()
            : base(MessageProtocol.COLOR_CODE)
        { }

        protected override PairingCode ParsePairingCode(byte[] messageBody)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(messageBody));
            byte codeLength = br.ReadByte();
            byte[] bytes = br.ReadBytes(codeLength);
            string code = String.Join(",", bytes.Select(x => x.ToString()).ToArray());
            return new PairingCode(PairingCodeType.COLOR_CODE, code);
        }
    }
}
