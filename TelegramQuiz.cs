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
        private readonly long chatId;
        string[] pollIds = new string[0];
        UserResults userResults = new();

        public TelegramQuiz(Question[] questions, Result[] results,long chatId) : base(questions, results)
        {
            Questions = questions;
            Results = results;
            pollIds = new string[questions.Length];
            this.chatId = chatId;

        }

        public string updatePollResults(string username, string pollId, int option)
        {
            //will return "" if quiz results are not finished
            //will return username if quiz has been completly filled out
            if (!pollIds.Contains(pollId))
            {
                return "";
            }
            return userResults.updateQuiz(username,pollId,option,Questions.Length);
        }
       

        private String[] quizOptions(Question questionOut)
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

        public async void startQuiz(ITelegramBotClient telegramBot,CancellationToken cts,int delay)
        {
            int i = 0;
            foreach (Question question in Questions)
            {
                Message message = await telegramBot.SendPollAsync(
                    chatId: chatId,
                    cancellationToken: cts,
                    isAnonymous: false,
                    question:question.QuestionField,
                    options:quizOptions(question),
                    disableNotification:true
                    );
                if (message != null)
                {
                    pollIds[i] = message.Poll.Id;
                    i++;
                }
                Thread.Sleep(delay);
            }
           
        }

        public Result CalculateResult(string username)
        {
            string test = username + " ";
            int[] options = new int[pollIds.Count()];
            int i = 0;
            foreach (string pollId in pollIds)
            {
                test = test + " " + userResults.getOption(username,pollId);
                options[i] = userResults.getOption(username,pollId);
                i++;

            }
            Console.WriteLine(test);
            return CalculateResult(options);
        }

        private class UserResults
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, int>> results = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();

            public string updateQuiz(string username, string pollId, int option,int pollCount)
            {
                ConcurrentDictionary<string, int> userInfo;
                if (results.ContainsKey(username))
                {
                    userInfo = results[username];
                }
                else
                {
                    userInfo = new ConcurrentDictionary<string, int>();
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

            public int getOption(string username,string pollId)
            {
                ConcurrentDictionary<string, int> userInfo;
                userInfo = results[username];
                return userInfo[pollId];
            }



        }
        public long getChatId()
        {
            return chatId;
        }
    }

}
