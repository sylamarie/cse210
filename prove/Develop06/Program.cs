/*Player levels increase with score (Level 1: 0-99, Level 2: 100-299, Level 3: 300+).
 *- Player info displays name, score, and level.*/

using System;
using System.Collections.Generic;
using System.IO;

// Base Goal class
public abstract class Goal
{
    protected string _shortName; // Remain as protected for encapsulation
    protected string _description;
    protected int _points;

    public Goal(string name, string description, int points)
    {
        _shortName = name;
        _description = description;
        _points = points;
    }

    // Abstract methods to be implemented in derived classes
    public abstract void RecordEvent();
    public abstract bool IsComplete();
    public virtual string GetDetailsString()
    {
        return $"[{(IsComplete() ? "X" : " ")}] {_shortName}: {_description}";
    }
    public abstract string GetStringRepresentation();

    public int GetPoints()
    {
        return _points;
    }

    public string ShortName  // Change to public property
    {
        get { return _shortName; }
    }
}

// SimpleGoal class (derived from Goal)
public class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string description, int points)
        : base(name, description, points)
    {
        _isComplete = false;
    }

    public override void RecordEvent()
    {
        if (!_isComplete)
        {
            _isComplete = true;
        }
    }

    public override bool IsComplete()
    {
        return _isComplete;
    }

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal,{_shortName},{_description},{_points},{_isComplete}";
    }

    public bool IsGoalComplete
    {
        get { return _isComplete; }
    }
}

// EternalGoal class (derived from Goal)
public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points)
        : base(name, description, points)
    {
    }

    public override void RecordEvent()
    {
        // Eternal goals add points every time they are recorded.
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override string GetStringRepresentation()
    {
        return $"EternalGoal,{_shortName},{_description},{_points}";
    }
}

// ChecklistGoal class (derived from Goal)
public class ChecklistGoal : Goal
{
    private int _amountCompleted;
    private int _target;
    private int _bonus;

    public ChecklistGoal(string name, string description, int points, int target, int bonus)
        : base(name, description, points)
    {
        _amountCompleted = 0;
        _target = target;
        _bonus = bonus;
    }

    public override void RecordEvent()
    {
        if (_amountCompleted < _target)
        {
            _amountCompleted++;
        }
    }

    public override bool IsComplete()
    {
        return _amountCompleted >= _target;
    }

    public override string GetDetailsString()
    {
        return base.GetDetailsString() + $" ({_amountCompleted}/{_target})";
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal,{_shortName},{_description},{_points},{_amountCompleted},{_target},{_bonus}";
    }

    public int AmountCompleted
    {
        get { return _amountCompleted; }
    }

    public int GetBonus()
    {
        return IsComplete() ? _bonus : 0;
    }
}

// NegativeGoal class (derived from Goal)
public class NegativeGoal : Goal
{
    public NegativeGoal(string name, string description, int points)
        : base(name, description, points)
    {
    }

    public override void RecordEvent()
    {
        // Deduct points when this goal is recorded.
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override string GetStringRepresentation()
    {
        return $"NegativeGoal,{_shortName},{_description},{_points}";
    }
}

// Player class to manage player information and score
public class Player
{
    private string _name;
    private int _score;
    private int _level;

    // Constructor
    public Player(string name)
    {
        _name = name;
        _score = 0; // Player starts with 0 points
        _level = 1; // Player starts at level 1
    }

    // Property to get player's name
    public string Name
    {
        get { return _name; }
    }

    // Property to get and set player's score
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            LevelUp();
        }
    }

    // Property to get player's level
    public int Level
    {
        get { return _level; }
    }

    // Method to display player info
    public string GetPlayerInfo()
    {
        return $"Player: {_name}, Score: {_score} points, Level: {_level}";
    }

    // Level up based on score
    private void LevelUp()
    {
        if (_score >= 100) _level = 2;
        if (_score >= 300) _level = 3;
        if (_score >= 600) _level = 4;
        // You can add more levels as needed
    }
}

// GoalManager class
public class GoalManager
{
    private List<Goal> _goals;
    private Player _player;  // Add a Player object
    private static GoalManager _instance;  // Singleton instance

    // Singleton implementation
    public static GoalManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Console.Write("Enter your name: ");
                string playerName = Console.ReadLine();
                _instance = new GoalManager(new Player(playerName));  // Initialize with player's name
            }
            return _instance;
        }
    }

    // Private constructor
    private GoalManager(Player player)
    {
        _goals = new List<Goal>();
        _player = player;  // Assign the player to the GoalManager
    }

    // Start the main menu loop
    public void Start()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n1. Display Player Info\n2. List Goals\n3. Create Goal\n4. Record Event\n5. Save Goals\n6. Load Goals\n7. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayPlayerInfo();
                    break;
                case "2":
                    ListGoalDetails();
                    break;
                case "3":
                    CreateGoal();
                    break;
                case "4":
                    RecordEvent();
                    break;
                case "5":
                    Console.Write("Enter filename to save goals: ");
                    string saveFile = Console.ReadLine();
                    SaveGoals(saveFile);
                    break;
                case "6":
                    Console.Write("Enter filename to load goals: ");
                    string loadFile = Console.ReadLine();
                    LoadGoals(loadFile);
                    break;
                case "7":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    // Display the player's current score and level
    public void DisplayPlayerInfo()
    {
        Console.WriteLine($"\n{_player.GetPlayerInfo()}");
    }

    // List the details of each goal
    public void ListGoalDetails()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("\nNo goals available.");
            return;
        }

        Console.WriteLine("\nGoals:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }
    }

    // Create a new goal and add it to the list
    public void CreateGoal()
    {
        Console.WriteLine("\n1. Simple Goal\n2. Eternal Goal\n3. Checklist Goal\n4. Negative Goal");
        Console.Write("Choose a goal type: ");
        string choice = Console.ReadLine();

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();
        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();
        Console.Write("Enter goal points: ");
        int points = int.Parse(Console.ReadLine());

        Goal newGoal = null;

        switch (choice)
        {
            case "1":
                newGoal = new SimpleGoal(name, description, points);
                break;
            case "2":
                newGoal = new EternalGoal(name, description, points);
                break;
            case "3":
                Console.Write("Enter target completions: ");
                int target = int.Parse(Console.ReadLine());
                Console.Write("Enter bonus points: ");
                int bonus = int.Parse(Console.ReadLine());
                newGoal = new ChecklistGoal(name, description, points, target, bonus);
                break;
            case "4":
                newGoal = new NegativeGoal(name, description, points);
                break;
            default:
                Console.WriteLine("Invalid goal type.");
                return;
        }

        _goals.Add(newGoal);
        Console.WriteLine("Goal created successfully.");
    }

    // Record an event for a goal
    public void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals available to record.");
            return;
        }

        Console.WriteLine("\nWhich goal did you accomplish?");
        ListGoalDetails();
        int goalIndex = int.Parse(Console.ReadLine()) - 1;

        if (goalIndex < 0 || goalIndex >= _goals.Count)
        {
            Console.WriteLine("Invalid goal selection.");
            return;
        }

        Goal goal = _goals[goalIndex];
        goal.RecordEvent();

        // Calculate the points earned
        int pointsEarned = goal.GetPoints();
        _player.Score += pointsEarned;  // Add points to player score

        // If the goal is a checklist goal, check for completion and add bonus
        if (goal is ChecklistGoal checklistGoal)
        {
            if (checklistGoal.IsComplete())
            {
                pointsEarned += checklistGoal.GetBonus(); // Add bonus points if completed
            }
        }

        Console.WriteLine($"Recorded accomplishment for goal '{goal.ShortName}'. Points earned: {pointsEarned}.");
    }

    // Save goals to a file
    public void SaveGoals(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (Goal goal in _goals)
            {
                writer.WriteLine(goal.GetStringRepresentation());
            }
        }
        Console.WriteLine($"Goals saved to {filename}.");
    }

    // Load goals from a file
    public void LoadGoals(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        using (StreamReader reader = new StreamReader(filename))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                switch (parts[0])
                {
                    case "SimpleGoal":
                        _goals.Add(new SimpleGoal(parts[1], parts[2], int.Parse(parts[3])));
                        break;
                    case "EternalGoal":
                        _goals.Add(new EternalGoal(parts[1], parts[2], int.Parse(parts[3])));
                        break;
                    case "ChecklistGoal":
                        _goals.Add(new ChecklistGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5])));
                        break;
                    case "NegativeGoal":
                        _goals.Add(new NegativeGoal(parts[1], parts[2], int.Parse(parts[3])));
                        break;
                }
            }
        }
        Console.WriteLine($"Goals loaded from {filename}.");
    }
}

// Main program
class Program
{
    static void Main(string[] args)
    {
        GoalManager.Instance.Start();
    }
}