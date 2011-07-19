using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAI.Communication.MessageLayer
{
    internal interface IMessageLayerOutgoing
    {

        void SendAuthenticationAccepted();

        void SendAuthenticationRejected();

        void SendPictureStreamStart();

        void SendPictureStreamStop();

        void SendFrame(byte[] frame);

        void SendRequestPairingCode(byte pincodeLength, byte colorCodeLength);

        void SendPairingCodeAccepted();

        void SendPincodeRejected();
 
    }
}
