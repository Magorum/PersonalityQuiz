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
            Hashtable results  = new Hashtable();
            foreach (Result result in quiz.Results)
            {
                results.Add(result.Name, 0);
            }
            String[] answer = quiz.RunConsoleQuiz();
   


        }
    }
}         