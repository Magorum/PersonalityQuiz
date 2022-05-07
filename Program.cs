using System.Collections;
using System.Text.Json;

namespace PersonalityQuizTelegram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            PersonalityQuiz quiz = PersonalityQuiz.GetPreGenQuiz();

            String output = JsonSerializer.Serialize(quiz);
            Console.WriteLine(output);

            ConsoleQuiz consoleQuiz = new ConsoleQuiz(quiz.Questions,quiz.Results);
            string[] answer = consoleQuiz.RunConsoleQuiz();
            Result result = consoleQuiz.CalculateResult(answer);
            Console.WriteLine(result.Name);
            Console.WriteLine(result.Description);
            Console.WriteLine(result.Imageurl);

        }
    }
}        