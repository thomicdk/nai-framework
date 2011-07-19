using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAI.Communication.SocketLayer
{
    internal interface ISocketLayerOutgoing
    {
        void SendMessage(byte[] message);
    }
}
