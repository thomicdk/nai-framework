using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace NAI.Communication.MessageLayer.Messages.Incoming
{
    internal abstract class TouchEventMessage : IncomingMessage
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public TouchEventMessage(MessageProtocol command)
            : base(command)
        { }

        protected override void ParseMessage(byte[] messageBody)
        {
            // Read double from the stream
            BinaryReader br = new BinaryReader(new MemoryStream(messageBody), Encoding.UTF8);
            X = BitConverter.Int64BitsToDouble(IPAddress.NetworkToHostOrder(br.ReadInt64()));
            Y = BitConverter.Int64BitsToDouble(IPAddress.NetworkToHostOrder(br.ReadInt64()));
        }
    }
}
