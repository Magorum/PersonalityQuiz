using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalityQuizTelegram
{
    public class ConsoleQuiz : PersonalityQuiz
    {


        public ConsoleQuiz(Question[] questions, Result[] results) : base(questions, results)
        {
        }



        public string[] RunConsoleQuiz()
        {
            string[] selections = new string[this.Questions.Count()];
            int i = 0;
            foreach (Question question in this.Questions)
            {
                Console.WriteLine(question.QuestionField);
                foreach (Answer answer in question.Answers)
                {
                    Console.WriteLine(answer.AnswerField);
                }
                Console.WriteLine("Enter your choice");
                selections[i] = Console.ReadLine();
                i++;
            }
            return selections;
        }

        public Result CalculateResult(string[] selections)
        {
            Hashtable resultsTally = new Hashtable();
            foreach (Result result in this.Results)
            {
                resultsTally.Add(result.Name, 0);
            }
            int i = 0;
            foreach (Question question in this.Questions)
            {
                foreach (Answer answer in question.Answers)
                {
                    if (selections[i].Equals(answer.AnswerField))
                    {
                        foreach(DictionaryEntry pointValue in answer.Points)
                        {
                            int v = (int)pointValue.Value + (int)resultsTally[pointValue.Key];
                            resultsTally[pointValue.Key] = v;
                        }
                    }
                }

                i++;
            }
            string finalKey = "";
            int finalValue = 0;
            foreach(DictionaryEntry result in resultsTally)
            {
                if((int)result.Value > finalValue)
                {
                    finalKey = (string)result.Key;
                    finalValue = (int)result.Value;
                }
            }

            foreach(Result result in this.Results)
            {
                if (finalKey.Equals(result.Name))
                {
                    return result;
                }
            }
            return null;
        }
    }


}
