using System;
using System.Collections.Generic;
using System.IO;

public class Entry
{
    public string Content { get; set; }
    public string Prompt { get; set; }
    public string Date { get; set; }
    public string Category { get; set; }

    // Creating a new entry
    public Entry(string content, string prompt, string category)
    {
        Content = content;
        Prompt = prompt;
        Category = category;
        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public override string ToString()
    {
        return $"Date: {Date}\nCategory: {Category}\nPrompt: {Prompt}\nEntry: {Content}\n";
    }

    public string ToTextFormat()
    {
        return $"Date: {Date}\nCategory: {Category}\nPrompt: {Prompt}\nEntry: {Content}\n";
    }
}

public class Journal
{
    public List<Entry> Entries { get; set; } = new List<Entry>();

    // Add new entry to the journal (category)
    public void AddEntry(string content, string prompt, string category)
    {
        Entry entry = new Entry(content, prompt, category);
        Entries.Add(entry);
    }

    // Display journal entries
    public void DisplayEntries()
    {
        if (Entries.Count == 0)
        {
            Console.WriteLine("No entries available.");
        }
        else
        {
            foreach (Entry entry in Entries)
            {
                Console.WriteLine(entry);
            }
        }
    }

    // Save to a text file
    public void SaveToText(string filename)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filename, true, System.Text.Encoding.UTF8)) // Appending to file
            {
                foreach (Entry entry in Entries)
                {
                    writer.WriteLine(entry.ToTextFormat());
                }
            }
            Console.WriteLine($"Journal saved to {filename} in text format.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the journal: {ex.Message}");
        }
    }

    // Load from a text file
    public void LoadFromText(string filename)
    {
        try
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.");
                return;
            }

            List<Entry> newEntries = new List<Entry>();
            string[] lines = File.ReadAllLines(filename);

            string date = "", category = "", prompt = "", content = "";
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Date: "))
                {
                    date = lines[i].Replace("Date: ", "").Trim();
                }
                else if (lines[i].StartsWith("Category: "))
                {
                    category = lines[i].Replace("Category: ", "").Trim();
                }
                else if (lines[i].StartsWith("Prompt: "))
                {
                    prompt = lines[i].Replace("Prompt: ", "").Trim();
                }
                else if (lines[i].StartsWith("Entry: "))
                {
                    content = lines[i].Replace("Entry: ", "").Trim();
                }

                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(prompt) && !string.IsNullOrEmpty(content))
                {
                    newEntries.Add(new Entry(content, prompt, category) { Date = date });

                    // Reset variables for the next entry
                    date = "";
                    category = "";
                    prompt = "";
                    content = "";
                }
            }

            Entries.AddRange(newEntries);
            Console.WriteLine($"Journal loaded from {filename} in text format.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading the journal: {ex.Message}");
        }
    }

    // Prompt generator
    public string GeneratePrompt()
    {
        string[] prompts = {
            "Who was the most challenging part of the day?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "What did I learn today?"
        };
        Random rand = new Random();
        return prompts[rand.Next(prompts.Length)];
    }
}

class Program
{
    static void Main(string[] args)
    {
        Journal myJournal = new Journal();
        string lastLoadedFilename = null; 
        bool running = true;

        // Load existing entries if the file exists
        while (running)
        {
            Console.WriteLine("\nJournal Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display all entries");
            Console.WriteLine("3. Save journal to text file");
            Console.WriteLine("4. Load journal from text file");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    string prompt = myJournal.GeneratePrompt();
                    Console.WriteLine($"\nPrompt: {prompt}");
                    Console.Write("Write your journal entry: ");
                    string content = Console.ReadLine();
                    Console.Write("Enter a category for this entry (e.g., Personal, Work, Reflection): ");
                    string category = Console.ReadLine();
                    myJournal.AddEntry(content, prompt, category);
                    break;

                case "2":
                    myJournal.DisplayEntries();
                    break;

                case "3":
                    // Ask for the filename to save the entries
                    Console.Write("Enter the filename to save the journal (with .txt extension): ");
                    string saveFilename = Console.ReadLine();
                    myJournal.SaveToText(saveFilename);
                    lastLoadedFilename = saveFilename; 
                    break;

                case "4":
                    Console.Write("Enter the filename to load the journal (with .txt extension): ");
                    string loadFilename = Console.ReadLine();
                    myJournal.LoadFromText(loadFilename);
                    lastLoadedFilename = loadFilename;
                    break;

                case "5":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}