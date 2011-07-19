
namespace NAI.Communication.MessageLayer.Messages
{
    internal abstract class Message
    {
        public MessageProtocol Command { get; private set; }
     
        public Message(MessageProtocol command)
        {
            this.Command = command;
        }
    }
}
