using System;

namespace StorageAccount.Chat
{
    public class ChatMessage
    {
        public Guid ChannelId { get; set; }
        public Guid MessageId { get; set; }
        public string From { get; set; }
        public DateTime Sent { get; set; }
        public string Message { get; set; }
    }
}