using System.Collections;
using System.Configuration;
using System.Text.Json;

namespace PersonalityQuizTelegram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = ConfigurationManager.AppSettings.Get("TelegramKey");
            PersonalityQuiz quiz = PersonalityQuiz.GetPreGenQuiz();
            TelegramQuiz telegram = new(quiz.Questions,quiz.Results);
            telegram.startBot(key);
            String output = JsonSerializer.Serialize(quiz);
            Console.WriteLine(output);

            ConsoleQuiz consoleQuiz = new(quiz.Questions, quiz.Results);
            string[] answer = consoleQuiz.RunConsoleQuiz();
            Result result = consoleQuiz.CalculateResult(answer);
            Console.WriteLine(result.Name);
            Console.WriteLine(result.Description);
            Console.WriteLine(result.Imageurl);

        }
    }
}        