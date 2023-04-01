using System.Text;
using System.Text.Json;
using Broadcast.Core.Entities;

namespace Broadcast.Core.Logics
{
    public static class ConvertMessage
    {
        public static string ToJson(byte[] bodyBytes)
        {
            return System.Text.Encoding.UTF8.GetString(bodyBytes);
        }

        public static byte[] ToByte<T>(T message) where T : Message
        {
            var jsonMessage = JsonSerializer.Serialize(message, message.GetType());
            return Encoding.UTF8.GetBytes(jsonMessage);
        }
    }
}