using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using SharedLibrary.Messages;

namespace SharedLibrary
{
    public static class SerializerHelper
    {
        private const byte _etb = 23;
        public static byte[] Serialize(Message message)
        {
            MemoryStream rawMessage = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(message.GetType());
            serializer.Serialize(rawMessage, message);
            rawMessage.WriteByte(_etb);
            return rawMessage.ToArray();
        }
        public static Message Deserialize(byte[] rawMessage)
        {
            XmlDocument doc = new XmlDocument();
            StringBuilder xmlMessage = new StringBuilder();
            MemoryStream stream = new MemoryStream(rawMessage, 0, rawMessage.Length);
            try
            {
                doc.Load(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cannot deserialize message to xml.\n{e.Message}");
                return null;
            }
            XmlWriter writer = XmlWriter.Create(xmlMessage);
            doc.Save(writer);
            Type messageType = Assembly.GetAssembly(typeof(Message)).GetTypes()
                .FirstOrDefault(t => t.IsSubclassOf(typeof(Message)) && !t.IsAbstract && t.Name == doc.DocumentElement.Name);
            if (messageType == null)
            {
                Console.WriteLine($"Unkown message name {doc.DocumentElement.Name}.");
                return null;
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(messageType);
                return serializer.Deserialize(new StringReader(xmlMessage.ToString())) as Message;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cannot deserialize \"{doc.DocumentElement.Name}\" to xml.\n{e.Message}");
                return null;
            }
        }
    }
}