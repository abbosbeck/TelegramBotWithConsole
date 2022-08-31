using Newtonsoft.Json;
using System;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotWithConsole
{
    struct BotUpdate
    {
        public string text;
        public long id;
        public string? username;
    }
    public class Program
    {
        static TelegramBotClient Bot = new TelegramBotClient
            ("5361634854:AAE8zo6pPI0kRQ3T83FHGP1hBkP2nR8bWc8");
        
        static string fileName = "updates.json";
        
        static List<BotUpdate> botUpdates = new List<BotUpdate>();
        static void Main(string[] args)
        {
            //read all saved Updates
            try
            {
                var botUpdatesString = System.IO.File.ReadAllText(fileName);

                botUpdates = JsonConvert.DeserializeObject<List<BotUpdate>>(botUpdatesString) ??
                    botUpdates;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading or deserialize: {ex}");
            }

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage
                }
            };
            Bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);
            Console.ReadLine();

        }

        private static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            if(update.Type == UpdateType.Message)
            {
                if(update?.Message?.Type == MessageType.Text)
                {
                    //write an update to file
                    var _botUpdates = new BotUpdate
                    {
                        text = update.Message.Text,
                        id = update.Message.Chat.Id,
                        username = update.Message.Chat.Username
                    };
                    
                    botUpdates.Add(_botUpdates);

                }
            }
        }
    }
}