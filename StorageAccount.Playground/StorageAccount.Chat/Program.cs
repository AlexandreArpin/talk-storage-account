using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Configuration;

namespace StorageAccount.Chat
{
    class Program
    {
        public static Guid FastGuid(string value)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(value));
                return new Guid(hash);
            }
        }

        public static async Task GenerateInfiniteMessageAsync(ChatMessageQueue chatMessageQueue, Guid channelId)
        {
            var random = new Random();
            var fakeMessages = new Faker<ChatMessage>()
                .RuleFor(x => x.ChannelId, (f, m) => channelId)
                .RuleFor(x => x.Sent, (f, m) => DateTime.UtcNow)
                .RuleFor(x => x.From, (f, m) => f.Person.UserName)
                .RuleFor(x => x.MessageId, (f, m) => Guid.NewGuid())
                .RuleFor(x => x.Message, (f, m) => f.Rant.Review())
                .GenerateForever();

            foreach (var fakeMessage in fakeMessages)
            {
                await chatMessageQueue.EnqueueAsync(fakeMessage);
                await Task.Delay(random.Next(250, 4000));
            }

        }

        public static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.local.json", true, true)
                .Build();

            var chatMessageRepository = ChatMessageRepository.CreateRepository(configuration["AzureWebJobsStorage"]);
            var chatMessageQueue = ChatMessageQueue.CreateQueue(configuration["AzureWebJobsStorage"]);

            Console.WriteLine("Which channel would you like to join?");
            var channelId = FastGuid(Console.ReadLine());

            Console.WriteLine("Starting message generator...");
            var infiniteMessageTask = GenerateInfiniteMessageAsync(chatMessageQueue, channelId);

            Console.WriteLine("Loading channel - Press CTRL+C to exit");

            var initialHistory = chatMessageRepository.History(channelId).Take(10).Reverse();

            var lastMessageId = Guid.Empty;
            await foreach (var message in initialHistory)
            {
                Console.WriteLine($"[{message.Sent:s}] {message.From}: {message.Message}");
                lastMessageId = message.MessageId;
            }
            Console.WriteLine($"... You are now live! ...");

            while (true)
            {
                var newMessages = chatMessageRepository.History(channelId).TakeWhile(x => x.MessageId != lastMessageId).Reverse();

                await foreach (var message in newMessages)
                {
                    Console.WriteLine($"[{message.Sent:h:mm:ss}] {message.From}: {message.Message}");
                    lastMessageId = message.MessageId;
                }

                await Task.Delay(300);
            }
        }
    }
}
