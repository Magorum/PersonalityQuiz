using System.Configuration;
using System.Text.Json;

namespace PersonalityQuizTelegram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = ConfigurationManager.AppSettings.Get("TelegramKey");
            TelegramBotQuizInterface telegramBotQuizInterface = new TelegramBotQuizInterface(key);

            Console.WriteLine("Hey can you see this?");
            while (true)
            {
                Console.WriteLine("Heartbeat...");
                Thread.Sleep(1000);
            } ;
            

        }
    }
}