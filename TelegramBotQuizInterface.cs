using Newtonsoft.Json;
using System;
using System.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PersonalityQuizTelegram
{
    public class TelegramBotQuizInterface
    {
        TelegramQuiz telegramQuiz;
        string location = ConfigurationManager.AppSettings.Get(@"FolderLocation");
        public TelegramBotQuizInterface(string key)
        {
            var botClient = new TelegramBotClient(key);
            using var cts = new CancellationTokenSource();
            botClient.SendTextMessageAsync(
                              chatId: -1001639508913,
                              text: "I got here"
                             );


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


            Console.WriteLine($"Start listening...");
            while (true)
            {
            };

            // Send cancellation request to stop bot
            cts.Cancel();
        }
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
                    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "You said:\n" + messageText,
        cancellationToken: cancellationToken);

                    if (messageText.Contains("quiz"))
                    {
                        if (messageText == "quiz pregen")
                        {
                            PersonalityQuiz quiz = PersonalityQuiz.GetPreGenQuiz();
                            telegramQuiz = new TelegramQuiz(quiz.Questions, quiz.Results,update.Message.Chat.Id);
                            telegramQuiz.startQuiz(botClient, cancellationToken, 10);
                        }
                        else
                        {
                            string fileLocation = location+messageText.Remove(0,5);
                            try
                            {
                                PersonalityQuiz quiz;
                                using (StreamReader file = System.IO.File.OpenText(fileLocation))
                                {
                                    JsonSerializer serializer = new JsonSerializer();
                                    quiz = (PersonalityQuiz)serializer.Deserialize(file, typeof(PersonalityQuiz));

                                    telegramQuiz = new TelegramQuiz(quiz.Questions, quiz.Results, update.Message.Chat.Id);
                                    telegramQuiz.startQuiz(botClient, cancellationToken, 10);


                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                }
            }
            if (update.Type == UpdateType.PollAnswer)
            {
                if (telegramQuiz != null)
                {
                    var chatId = telegramQuiz.getChatId();
                    String username = update.PollAnswer.User.FirstName;
                    string pollId = update.PollAnswer.PollId;

                    if (update.PollAnswer.OptionIds.Length > 0)
                    {
                        if (username != null)
                        {
                            int option = update.PollAnswer.OptionIds[0];
                            string userReturn = telegramQuiz.updatePollResults(username, pollId, option);
                            if (userReturn != "")
                            {
                                Result result = telegramQuiz.CalculateResult(userReturn);
                                if (result != null)
                                {
                                    String url = result.Imageurl;
                                    String caption = result.Description;
                                    String name = result.Name;
                                    Message photoMessage = await botClient.SendPhotoAsync(
                                        chatId: chatId,
                                        photo: url,
                                        caption: userReturn + " your result is " + name,
                                        cancellationToken: cancellationToken);
                                    Message textMessage = await botClient.SendTextMessageAsync(
                              chatId: chatId,
                              text: caption,
                              cancellationToken: cancellationToken);

                                }
                            }
                        }
                    }
                }
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
}