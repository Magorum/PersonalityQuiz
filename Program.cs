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

            Console.ReadLine();
            

        }
    }
}