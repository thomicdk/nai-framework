using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAI.Communication.SocketLayer
{
    internal interface ISocketLayerIncoming
    {
        void OnIncomingMessage(byte[] message);

        void OnConnectionLost();
    }
}
