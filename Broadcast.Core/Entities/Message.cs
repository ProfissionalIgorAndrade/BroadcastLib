namespace Broadcast.Core.Entities
{
    public class Message
    {
        public Message(string from = "", string to = "")
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
            From = from;
            To = to;
        }

        public Guid Id { get; init; }
        public DateTime CreationDate { get; init; }
        public string From { get; init; }
        public string To { get; init; }
    }
}