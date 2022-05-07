﻿using System;
using System.Collections;


public class PersonalityQuiz
{
    public PersonalityQuiz(Question[] questions, Result[] results)
    {
        this.Questions = questions;
        this.Results = results;
    }

    public Question[] Questions { get; set; }
    public Result[] Results { get; set; }

    public static PersonalityQuiz GetPreGenQuiz()
    {
        Result[]
        results = {
        new Result("Darth Vader", "He breath funny he a bit evil but not all the way", "https://upload.wikimedia.org/wikipedia/en/0/0b/Darth_Vader_in_The_Empire_Strikes_Back.jpg"),
                new Result("Luke Skywalker", "No hand :( but he fly x wing good", "https://i.redd.it/x7f4u19956e41.jpg"),
                new Result("Yoda", "Tiny Green Dude he hit with stick", "https://static.wikia.nocookie.net/starwars/images/d/d6/Yoda_SWSB.png/revision/latest/scale-to-width-down/1000?cb=20150206140125")
            };

        Hashtable points1 = new Hashtable();
        points1.Add("Yoda", 10);
        Hashtable points2 = new Hashtable();
        points2.Add("Luke Skywalker", 10);
        points2.Add("Darth Vader", 10);
        Answer[] answers1 =
        {
                new Answer("yes",points1),
                new Answer("no",points2)

            };
        points1.Add("Luke Skywalker", 10);
        points2.Remove("Luke Skywalker");
        Answer[] answers2 =
        {
                new Answer("yes",points1),
               new Answer("no",points2)
            };
        Question[] questions =
         {
                new Question("Are you short?",answers1),
                new Question("Are you a Jedi Master?",answers2)

            };
        PersonalityQuiz quiz = new PersonalityQuiz(questions, results);

        return quiz;

    }
    public String[] RunConsoleQuiz()
    {
        String[] answers = new String[this.Questions.Count()];
        int i = 0;
        foreach (Question question in this.Questions)
        {
            Console.WriteLine(question.QuestionField);
            foreach (Answer answer in question.Answers)
            {
                Console.WriteLine(answer.AnswerField);
            }
            Console.WriteLine("Enter your choice");
            answers[i] = Console.ReadLine();
            i++;
        }
        return answers;
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
{    public Result(string name, string description, string imageurl)
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
