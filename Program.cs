using System.Configuration;
using System.Text.Json;

namespace PersonalityQuizTelegram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string key = ConfigurationManager.AppSettings.Get("TelegramKey");
            try
            {
                PersonalityQuiz quiz = JsonSerializer.Deserialize<PersonalityQuiz>(File.ReadAllText(@"C:\Users\Tyler\Documents\GitHub\PersonalityQuiz\quiz\Shrek.json"));
                Console.WriteLine(quiz.Questions[0].QuestionField);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            TelegramBotQuizInterface telegramBotQuizInterface = new TelegramBotQuizInterface(key);

            Console.ReadLine();
            

        }
    }
}