using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

namespace StorageAccount.Chat
{
    public class ChatMessageRepository
    {
        public TableClient TableClient { get; set; }

        public static ChatMessageRepository CreateRepository(string connectionString)
        {
            var tableClient = new TableClient(connectionString, "chat0messages");

            tableClient.CreateIfNotExists();

            return new ChatMessageRepository
            {
                TableClient = tableClient,
            };
        }

        public Task InsertAsync(ChatMessage chatMessage)
        {
            var entity = new ChatMessageTableEntity()
            {
                PartitionKey = $"{chatMessage.ChannelId}",
                RowKey = $"{DateTime.UtcNow.ToReverseTimestamp()}.{chatMessage.MessageId}",
                From = chatMessage.From,
                Sent = chatMessage.Sent,
                Message = chatMessage.Message,
            };

            return this.TableClient.AddEntityAsync(entity);
        }

        public IAsyncEnumerable<ChatMessage> History(Guid channelId)
        {
            return this.TableClient
                .QueryAsync<ChatMessageTableEntity>(x => x.PartitionKey == $"{channelId}")
                .Select(x => new ChatMessage
                {
                    ChannelId = new Guid(x.PartitionKey),
                    MessageId = new Guid(x.RowKey.Split('.')[1]),
                    From = x.From,
                    Sent = x.Sent,
                    Message = x.Message
                });
        }

        private class ChatMessageTableEntity : ITableEntity
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public DateTimeOffset? Timestamp { get; set; }
            public ETag ETag { get; set; }

            public string From { get; set; }
            public DateTime Sent { get; set; }
            public string Message { get; set; }
        }
    }
}
