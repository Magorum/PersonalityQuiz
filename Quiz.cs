using System.Collections;

namespace PersonalityQuizTelegram
{
    public class PersonalityQuiz
    {
        public PersonalityQuiz(Question[] questions, Result[] results)
        {
            if (questions == null || results == null)
            {
                throw new ArgumentNullException(nameof(questions));
            }
            this.Questions = questions;
            this.Results = results;
        }

        public Question[]? Questions { get; set; }
        public Result[]? Results { get; set; }

        public Result? CalculateResult(int[] selections)
        {
            Hashtable resultsTally = new();
            foreach (Result result in this.Results)
            {
                resultsTally.Add(result.Name, 0);
            }
            int i = 0;
            foreach (Question question in this.Questions)
            {
               foreach (DictionaryEntry pointValue in question.Answers[selections[i]].Points)
                {
                    int v = (int)pointValue.Value + (int)resultsTally[pointValue.Key];
                    resultsTally[pointValue.Key] = v;
                }
                i++;
            }
            return totalResult(resultsTally);
        }
        public Result? CalculateResult(string[] selections)
        {
            Hashtable resultsTally = new();
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
                        foreach (DictionaryEntry pointValue in answer.Points)
                        {
                            int v = (int)pointValue.Value + (int)resultsTally[pointValue.Key];
                            resultsTally[pointValue.Key] = v;
                        }
                    }
                }

                i++;
            }
            return totalResult(resultsTally);
        }
        private Result totalResult(Hashtable resultsTally) { 
            string finalKey = "";
            int finalValue = 0;
            foreach (DictionaryEntry result in resultsTally)
            {
                if ((int)result.Value > finalValue)
                {
                    finalKey = (string)result.Key;
                    finalValue = (int)result.Value;
                }
            }

            foreach (Result result in Results)
            {
                if (finalKey.Equals(result.Name))
                {
                    return result;
                }
            }
            return null;
        }

        public static PersonalityQuiz GetPreGenQuiz()
        {
            Result[]
            results = {
        new Result("Darth Vader", "He breath funny he a bit evil but not all the way", "https://upload.wikimedia.org/wikipedia/en/0/0b/Darth_Vader_in_The_Empire_Strikes_Back.jpg"),
                new Result("Luke Skywalker", "No hand :( but he fly x wing good", "https://i.redd.it/x7f4u19956e41.jpg"),
                new Result("Yoda", "Tiny Green Dude he hit with stick", "https://static.wikia.nocookie.net/starwars/images/d/d6/Yoda_SWSB.png/revision/latest/scale-to-width-down/1000?cb=20150206140125")
            };

            Hashtable points1 = new();
            points1.Add("Yoda", 10);
            Hashtable points2 = new();
            points2.Add("Luke Skywalker", 10);
            points2.Add("Darth Vader", 10);
            Hashtable points3 = new();
            points3.Add("Yoda", 10);
            points3.Add("Luke Skywalker", 10);
            Hashtable points4 = new();
            points4.Add("Darth Vader", 10);
            Answer[] answers1 =
            {
                new Answer("yes",points1),
                new Answer("no",points2)

            };
            Answer[] answers2 =
            {
                new Answer("yes",points3),
               new Answer("no",points4)
            };
            Question[] questions =
             {
                new Question("Are you short?",answers1),
                new Question("Are you a Jedi Master?",answers2)

            };
            PersonalityQuiz quiz = new(questions, results);

            return quiz;

        }

    }




    public class Question
    {
        public Question(string questionField, Answer[] answers)
        {
            this.QuestionField = questionField;
            this.Answers = answers;
        }

        public string? QuestionField { get; set; }
        public Answer[]? Answers { get; set; }
    }
    public class Result
    {
        public Result(string name, string description, string imageurl)
        {
            this.Name = name;
            this.Description = description;
            this.Imageurl = imageurl;
        }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Imageurl { get; set; }
    }
    public class Answer
    {
        public Answer(string answerField, Hashtable points)
        {
            this.AnswerField = answerField;
            this.Points = points;
        }

        public string? AnswerField { get; set; }
        public Hashtable? Points { get; set; }
    }
}