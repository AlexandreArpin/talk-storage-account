using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StorageAccount.Chat;

namespace StorageAccount.BackgroundProcessing
{
    public class QueueTriggers
    {
        private readonly ChatMessageRepository _chatMessageRepository;

        public QueueTriggers(IConfiguration configuration)
        {
            this._chatMessageRepository = ChatMessageRepository.CreateRepository(configuration["AzureWebJobsStorage"]);
        }

        [FunctionName("ChatMessage")]
        public async Task OnChatMessageReceived([QueueTrigger("chat-messages", Connection = "AzureWebJobsStorage")] ChatMessage chatMessage, ILogger log)
        {
            await this._chatMessageRepository.InsertAsync(chatMessage);
            log.LogWarning($"[{chatMessage.ChannelId}|{chatMessage.Sent:s}] {chatMessage.From}: {chatMessage.Message}");
        }
    }
}
