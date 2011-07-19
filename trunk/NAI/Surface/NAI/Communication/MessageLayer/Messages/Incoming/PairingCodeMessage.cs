using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAI.Client.Pairing;

namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal abstract class PairingCodeMessage : IncomingMessage
    {
        protected PairingCode _pairingCode;

        public PairingCodeMessage(MessageProtocol pairingCodeType)
            : base(pairingCodeType)
        {
            if (pairingCodeType != MessageProtocol.PIN_CODE && pairingCodeType != MessageProtocol.COLOR_CODE)
            {
                throw new ArgumentException("Unknown pairing code type: " + pairingCodeType);
            }
        }

        protected abstract PairingCode ParsePairingCode(byte[] messageBody);

        protected override void ParseMessage(byte[] messageBody)
        {
            this._pairingCode = ParsePairingCode(messageBody);
        }
    }
}
