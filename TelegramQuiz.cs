using System;
using Telegram.Bot;

namespace PersonalityQuizTelegram
{
	public class TelegramQuiz : PersonalityQuiz
	{
		public TelegramQuiz(Question[] questions, Result[] results) : base(questions, results)
		{
			Questions = questions;
			Results = results;

		}
		public Question[] questions { get; set; }
		public Result[] results { get; set; }

		public async void startBot(string accessKey)
        {
			var botClient = new TelegramBotClient(accessKey);
			var me = await botClient.GetMeAsync();
			Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");


		}

	}
}
