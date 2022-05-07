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
            if (questions.Length == 0)
            {
                throw new ArgumentException("Questions can not be 0");
            }

            if (questions == null || results == null)
            {
                throw new ArgumentNullException(nameof(questions));
            }

        }



        public string[] RunConsoleQuiz()
        {
            string[] selections = new string[this.Questions.Length];
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


    }


}
