using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PersonalityQuizTelegram
{
    public class TelegramQuiz : PersonalityQuiz
    {
        readonly long chatId;
        long[] pollIds = new long[0];
        UserResults userResults = new();
        public TelegramQuiz(Question[] questions, Result[] results,long chatId) : base(questions, results)
        {
            Questions = questions;
            Results = results;
            pollIds = new long[questions.Length];
            this.chatId = chatId;

        }

        public string updatePollResults(string username, long pollId, int option)
        {
            //will return "" if quiz results are not finished
            //will return username if quiz has been completly filled out
            return userResults.updateQuiz(username,pollId,option,Questions.Length);
        }
        //TODO sending questions//Saving PollIds

        public String[] quizOptions(Question questionOut)
        {
            String[] quizOptions = new String[questionOut.Answers.Length];
                int i = 0;
                foreach (Answer answer in questionOut.Answers)
                {
                    quizOptions[i] = answer.AnswerField;
                    i++;
                }
            return quizOptions;
        }

        public Result CalculateResult(string username)
        {
            return CalculateResult(userResults.getOptions(username));
        }

        private class UserResults
        {
            ConcurrentDictionary<string, ConcurrentDictionary<long, int>> results = new ConcurrentDictionary<string, ConcurrentDictionary<long, int>>();

            public string updateQuiz(string username, long pollId, int option,int pollCount)
            {
                ConcurrentDictionary<long, int> userInfo;
                if (results.ContainsKey(username))
                {
                    userInfo = results[username];
                }
                else
                {
                    userInfo = new ConcurrentDictionary<long, int>();
                    results[username] = userInfo;
                }

                if (userInfo.ContainsKey(pollId))
                {
                    userInfo[pollId] = option;
                }
                else
                {
                    userInfo.TryAdd(pollId, option);
                    //Checks to see if poll has completed
                    if (userInfo.Count == pollCount)
                    {
                        return username;
                    }

                }
                return "";
            }
            
           public int[] getOptions(string username)
            { 
                return results[username].Values.ToArray();
            }
        }
    }
}
