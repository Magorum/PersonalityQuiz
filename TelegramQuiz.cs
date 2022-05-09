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
        public TelegramQuiz(Question[] questions, Result[] results) : base(questions, results)
        {
            Questions = questions;
            Results = results;
            pollIds = new long[questions.Length];

        }

        public string updatePollResults(string username, long pollId, int option)
        {
            //will return "" if quiz results are not finished
            //will return username if quiz has been completly filled out
            return userResults.updateQuiz(username,pollId,option,Questions.Length);
        }

        public async void startBot(string accessKey)
        {
            var botClient = new TelegramBotClient(accessKey);
            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();



            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Only process Message updates: https://core.telegram.org/bots/api#message
                UpdateType[] valid =
                {
                    UpdateType.Message,
                    UpdateType.Poll,
                    UpdateType.PollAnswer
                };

                if (!(valid.Contains(update.Type)))
                    return;
                // Only process text messages

                if (update.Type == UpdateType.Message)
                {
                    if (update.Message.Type == MessageType.Text)
                    {
                        var chatId = update.Message.Chat.Id;

                        var messageText = update.Message.Text;

                        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

                        if (messageText == "quiz")
                        {
                            foreach (Question question in this.Questions)
                            {
                                String[] quizOptions = new String[question.Answers.Length];
                                int i = 0;
                                foreach (Answer answer in question.Answers)
                                {
                                    quizOptions[i] = answer.AnswerField;
                                    i++;
                                }
                                Message pollMessage = await botClient.SendPollAsync(
                                    chatId: chatId,
                                    question: question.QuestionField,
                                    options: quizOptions,
                                    cancellationToken: cancellationToken,
                                    isAnonymous: false);


                            }
                        }

                        // Echo received message text
                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "You said:\n" + messageText,
                            cancellationToken: cancellationToken);
                    }
                }
                    if (update.Type == UpdateType.PollAnswer)
                    {
                    //Should probubly figure out a good place to store this off
                    //Can't get it in the PollAnswer update type
                    //Get update.PollAnswer.PollId and compare to get the chat ID
                    var chatId = -1001639508913;
                        
                        String username = update.PollAnswer.User.Username;
                        String text = "";
               
                    if(update.PollAnswer.OptionIds.Length > 0)
                    {
                        String option = update.PollAnswer.OptionIds[0].ToString();
                        text = username + " voted for Option: " + option;
                    }
                    else
                    {
                        text = username + "has retracted their vote :(";
                    }
                       

                        Message sentMessage = await botClient.SendTextMessageAsync(
                             chatId: chatId,
                             text: text,
                             cancellationToken: cancellationToken);
                    }
                

            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }


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
                    if(userInfo.Count == pollCount)
                    {
                        return username;
                    }

                }
                
                
                return "";
            }
        }
    }
}
