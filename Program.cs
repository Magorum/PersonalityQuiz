using System;
using System.Collections;
using Telegram.Bot;
using System.Text.Json;

namespace PersonalityQuizTelegram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient("Tyler_Don't_Leak_Keys");

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
            PersonalityQuiz quiz = PersonalityQuiz.GetPreGenQuiz();

            String output = JsonSerializer.Serialize(quiz);
            Console.Write(output);

        }
    }
}         