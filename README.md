# PersonalityQuiz

Making a Personality Quiz Bot for Telegram!

Uses the [.Net TelegramBot Project](https://github.com/TelegramBots/Telegram.Bot) to create personality quizzes directly in a telegram chat

Creates a number of Telegram native polls then as people fill them out will post the results in the chat

---

Quizzes are created in a .json format then called from the chat for example 'quiz example.json'

### Here's an small example of the format more detailed ones can be found in /quiz
<code>{
    "Questions": [
     {
        "QuestionField": "Are you short?",
        "Answers": [
         {
            "AnswerField": "yes",
            "Points": {
             "Yoda": 10
           }
          }
        ]
      }
    ],
    "Results": [
     {
       "Name": "Yoda",
        "Description": "Tiny Green Dude he hit with stick",
       "Imageurl": "https://static.wikia.nocookie.net/starwars/images/d/d6/Yoda_SWSB.png/revision/latest/scale-to-width-down/1000?cb=20150206140125"
     }
   ]
  }</code>


