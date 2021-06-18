using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Azure.Storage;
using Azure.Storage.Queues;
using Newtonsoft.Json;

namespace StorageAccount.Chat
{
    public class ChatMessageQueue
    {
        public QueueClient QueueClient { get; set; }

        public static ChatMessageQueue CreateQueue(string connectionString)
        {
            var queueClient = new QueueClient(connectionString, "chat-messages", new QueueClientOptions()
            {
                MessageEncoding = QueueMessageEncoding.Base64,
            });

            queueClient.CreateIfNotExists();

            return new ChatMessageQueue
            {
                QueueClient = queueClient,
            };
        }

        public Task EnqueueAsync(ChatMessage chatMessage)
        {
            return this.QueueClient.SendMessageAsync(BinaryData.FromObjectAsJson(chatMessage));
        }
    }
}
