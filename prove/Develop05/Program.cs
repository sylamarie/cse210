using System;
using System.Collections.Generic;
using System.Threading;

// Base class that contains common functionality
// MeditationActivity class inheriting from Activity base class
abstract class Activity
{
    protected int duration;

    public void StartMessage(string activityName, string description)
    {
        Console.WriteLine($"\nWelcome to the {activityName}.");
        Console.WriteLine(description);

        Console.Write("\nHow long, in seconds, would you like for your session? ");
        duration = int.Parse(Console.ReadLine());

        Console.WriteLine("\nGet ready...");
        AnimateLoading();
    }

    public void EndMessage(string activityName)
    {
        Console.WriteLine("\nWell done!!");
        AnimateLoading();
        Console.WriteLine($"You have completed another {duration} seconds of the {activityName}.");
        AnimateLoading();
        Console.WriteLine();
    }

    protected void Countdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i);
            Thread.Sleep(1000);
            Console.Write("\b \b");
        }
    }

    protected void AnimateLoading()
    {
        List<string> animationStrings = new List<string> { "|", "/", "-", "\\" };
        for (int i = 0; i < 8; i++)
        {
            foreach (string anim in animationStrings)
            {
                Console.Write(anim);
                Thread.Sleep(200);
                Console.Write("\b");
            }
        }
    }

    public abstract void PerformActivity();
}

// ReflectionActivity class inheriting from Activity base class
class ReflectionActivity : Activity
{
    public override void PerformActivity()
    {
        StartMessage("Reflecting activity", 
            "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.");

        List<string> prompts = new List<string>
        {
            "--- Think of a time when you stood up for someone else. ---",
            "--- Think of a time when you did something really difficult. ---",
            "--- Think of a time when you helped someone in need. ---",
            "--- Think of a time when you did something truly selfless. ---"
        };

        List<string> questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };

        Random rand = new Random();
        string selectedPrompt = prompts[rand.Next(prompts.Count)];

        Console.WriteLine($"\nConsider the following prompt:\n{selectedPrompt}\n");
        Console.WriteLine("When you have something in mind, press enter to continue.");
        Console.ReadLine();

        Console.WriteLine("Now ponder on each of the following questions as they relate to this experience.");
        Console.WriteLine("You may begin in: ");
        Countdown(3);

        int timeLeft = duration;
        while (timeLeft > 0)
        {
            string question = questions[rand.Next(questions.Count)];
            Console.WriteLine($"\n>{question}");
            AnimateLoading();  // This adds a delay with animation between questions
            timeLeft -= 10;  // Each question cycle takes 10 seconds
        }

        EndMessage("Reflecting Activity");
    }
}

// BreathingActivity class inheriting from Activity base class
class BreathingActivity : Activity
{
    public override void PerformActivity()
    {
        StartMessage("Breathing Activity", 
            "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.");

        int timeLeft = duration;
        while (timeLeft > 0)
        {
            Console.WriteLine("\nBreathe in...");
            Countdown(4);  // Countdown 4 seconds for breathing in

            Console.WriteLine("Now breathe out...");
            Countdown(4);  // Countdown 4 seconds for breathing out

            timeLeft -= 8;  // Each cycle of breathing takes 8 seconds
        }

        EndMessage("Breathing Activity");
    }
}

// ListingActivity class inheriting from Activity base class
class ListingActivity : Activity
{
    public override void PerformActivity()
    {
        StartMessage("Listing Activity", 
            "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.");

        List<string> prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };

        Random rand = new Random();
        string selectedPrompt = prompts[rand.Next(prompts.Count)];

        Console.WriteLine($"\nConsider the following prompt:\n{selectedPrompt}\n");
        Console.WriteLine("You may begin listing in: ");
        Countdown(3);

        int itemCounter = 0;
        int timeLeft = duration;

        while (timeLeft > 0)
        {
            Console.Write("> ");
            Console.ReadLine();  // The user lists an item
            itemCounter++;

            timeLeft -= 5;  // Assume each entry takes 5 seconds
        }

        Console.WriteLine($"\nYou listed {itemCounter} items.");
        EndMessage("Listing Activity");
    }
}

// MeditationActivity class inheriting from Activity base class
class MeditationActivity : Activity
{
    public override void PerformActivity()
    {
        StartMessage("Meditation Activity", 
            "This activity will help you calm your mind and focus on the present moment.");

        Console.WriteLine("Find a comfortable position and close your eyes.");
        Console.WriteLine("Focus on your breath. Inhale deeply and exhale slowly.");
        Console.WriteLine("You may begin in: ");
        Countdown(5);

        int timeLeft = duration;

        while (timeLeft > 0)
        {
            Console.WriteLine("\nMeditate...");
            AnimateLoading();  // This adds a delay with animation during meditation
            timeLeft -= 5;  // Each meditation cycle takes 5 seconds
        }

        EndMessage("Meditation Activity");
    }
}

// Program with the main menu and activity selection
class Program
{
    static void Main(string[] args)
    {
        bool quit = false;
        while (!quit)
        {
            Console.WriteLine("Menu Options:");
            Console.WriteLine("    1. Start Breathing Activity");
            Console.WriteLine("    2. Start Reflecting Activity");
            Console.WriteLine("    3. Start Listing Activity");
            Console.WriteLine("    4. Start Meditation Activity");  // Added meditation option
            Console.WriteLine("    5. Quit");
            Console.Write("Select a choice from the menu: ");
            string choice = Console.ReadLine();

            Activity activity = null;

            switch (choice)
            {
                case "1":
                    activity = new BreathingActivity();
                    break;
                case "2":
                    activity = new ReflectionActivity();
                    break;
                case "3":
                    activity = new ListingActivity();
                    break;
                case "4":
                    activity = new MeditationActivity();  // Instantiate the new activity
                    break;
                case "5":
                    quit = true;
                    continue;
                default:
                    Console.WriteLine("Invalid option, please select a valid option.");
                    continue;
            }

            if (activity != null)
            {
                activity.PerformActivity();
            }
        }
    }
}