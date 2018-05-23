using System;
using System.Xml.Serialization;

namespace SharedLibrary.Messages
{
    [Serializable]
    public abstract class Message
    {
        public abstract void UpdateServer(ServerKnowledge serverKnowledge);
        public abstract void UpdateClient(ConsolePlus console);
        [XmlIgnore]
        public IMessageStream Sender { get; set; }
    }
}