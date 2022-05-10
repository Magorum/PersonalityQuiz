# PersonalityQuiz

Making a Personality Quiz Bot for Telegram!

Uses the [.Net TelegramBot Project](https://github.com/TelegramBots/Telegram.Bot) to create personality quizzes directly in a telegram chat

Creates a number of Telegram native polls then as people fill them out will post the results in the chat

---

Quizzes are created in a .json format then called from the chat for example 'quiz example.json'

Each answer to a question can add any amount of points to a possible result
In the example below 'yes' would add 10 points to Yoda and 'no' would add 10 points to Darth Vader

<img src="https://i.imgur.com/dIAoIvy.jpeg" max-height="200">


The quiz will also post the results into the chat when finished
Will pull an image from the url in the json

<img src="https://i.imgur.com/5hvTm27.jpeg"  max-height="200">


### Here's an small example of the format more detailed ones can be found in /quiz
```json
{
    "Questions": [
     {
        "QuestionField": "Are you short?",
        "Answers": [
         {
            "AnswerField": "yes",
            "Points": {
             "Yoda": 10
           },
         {
            "AnswerField": "no",
            "Points": {
             "Darth Vader": 10
           }
          }
        ]
      }
    ],
    "Results": [
     {
      "Name": "Darth Vader",
      "Description": "He breath funny he a bit evil but not all the way",
      "Imageurl": "https://upload.wikimedia.org/wikipedia/en/0/0b/Darth_Vader_in_The_Empire_Strikes_Back.jpg"
     },
     {
       "Name": "Yoda",
        "Description": "Tiny Green Dude he hit with stick",
       "Imageurl": "https://static.wikia.nocookie.net/starwars/images/d/d6/Yoda_SWSB.png/revision/latest/scale-to-width-down/1000?cb=20150206140125"
     }
   ]
  }
  ```


