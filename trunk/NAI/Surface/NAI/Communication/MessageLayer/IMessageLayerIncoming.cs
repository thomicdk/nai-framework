using NAI.Communication.MessageLayer.Messages.Incoming;

namespace NAI.Communication.MessageLayer
{
    internal interface IMessageLayerIncoming
    {
        void OnIncomingMessage(IncomingMessage message);

        void OnConnectionLost();
    }
}
