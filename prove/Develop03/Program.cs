// This program helps users memorize scriptures by hiding random words, prompting guesses, and displaying feedback.
// It supports multiple verses and allows for loading scriptures from a file, enhancing user engagement.
// Missing words are numbered, enabling users to guess specific words by their number, reducing confusion during the guessing process.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ScriptureReference
{
    public string Book { get; private set; }
    public int Chapter { get; private set; }
    public int VerseStart { get; private set; }
    public int VerseEnd { get; private set; }

    public ScriptureReference(string book, int chapter, int verse)
    {
        Book = book;
        Chapter = chapter;
        VerseStart = verse;
        VerseEnd = verse;
    }

    public ScriptureReference(string book, int chapter, int verseStart, int verseEnd)
    {
        Book = book;
        Chapter = chapter;
        VerseStart = verseStart;
        VerseEnd = verseEnd;
    }

    public override string ToString()
    {
        if (VerseStart == VerseEnd)
            return $"{Book} {Chapter}:{VerseStart}";
        else
            return $"{Book} {Chapter}:{VerseStart}-{VerseEnd}";
    }
}

public class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string Display()
    {
        return _isHidden ? new string('_', _text.Length) : _text;
    }

    public string GetText()
    {
        return _text;
    }
}

public class ScriptureText
{
    public ScriptureReference Reference { get; private set; }
    private List<Word> _words;
    private Random _random = new Random();
    private List<int> _hiddenIndices = new List<int>();

    public ScriptureText(ScriptureReference reference, string text)
    {
        Reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public void Display()
    {
        Console.WriteLine(Reference);
        foreach (var word in _words)
        {
            if (word.IsHidden())
            {
                Console.Write($"{GetWordPosition(_words.IndexOf(word))}_{new string('_', word.GetText().Length - 1)} ");
            }
            else
            {
                Console.Write($"{word.Display()} ");
            }
        }
        Console.WriteLine();
    }

    public List<int> HideRandomWords(int count)
    {
        var unhiddenWords = _words.Where(w => !w.IsHidden()).ToList();
        if (unhiddenWords.Count == 0)
            return null;

        List<int> newHiddenIndices = new List<int>();
        for (int i = 0; i < count && unhiddenWords.Count > 0; i++)
        {
            int index;
            do
            {
                index = _random.Next(_words.Count);
            } while (_words[index].IsHidden());

            _words[index].Hide();
            _hiddenIndices.Add(index);
            newHiddenIndices.Add(index);
        }

        return newHiddenIndices;
    }

    public bool CheckUserGuess(string userInput, int wordIndex)
    {
        return _words[wordIndex].GetText().Equals(userInput, StringComparison.OrdinalIgnoreCase);
    }

    public void DisplayMissingWordPositions()
    {
        Console.WriteLine("Missing words:");
        
        // Sort hidden indices to maintain order and get unique numbering
        List<int> sortedIndices = _hiddenIndices.OrderBy(i => GetWordPosition(i)).ToList();
        foreach (var index in sortedIndices)
        {
            Console.WriteLine($"Word {GetWordPosition(index)}: {new string('_', _words[index].GetText().Length)}");
        }
    }

    public void RemoveMoreWords(int count)
    {
        HideRandomWords(count);
    }

    public int GetHiddenCount()
    {
        return _hiddenIndices.Count;
    }

    public int GetWordPosition(int index)
    {
        int position = 1;
        for (int i = 0; i < index; i++)
        {
            if (!IsPunctuation(_words[i].GetText()))
            {
                position++;
            }
        }
        return position;
    }

    private bool IsPunctuation(string text)
    {
        char lastChar = text.LastOrDefault();
        return lastChar == ':' || lastChar == ';' || lastChar == '.' || lastChar == ',';
    }

    public List<int> GetHiddenWordIndices()
    {
        return _hiddenIndices.ToList();
    }

    public void ResetHiddenIndices()
    {
        _hiddenIndices.Clear();
        for (int i = 0; i < _words.Count; i++)
        {
            if (_words[i].IsHidden())
            {
                _hiddenIndices.Add(i);
            }
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        List<ScriptureText> scriptures = LoadScriptures("scripture.txt");

        if (scriptures.Count == 0)
        {
            Console.WriteLine("No scriptures found in the file.");
            return;
        }

        Random random = new Random();
        ScriptureText selectedScripture = scriptures[random.Next(scriptures.Count)];

        Console.Clear();
        selectedScripture.Display();

        while (true)
        {
            Console.WriteLine("Press Enter to hide words or type 'quit' to exit.");
            string input = Console.ReadLine();
            if (input == "quit")
                break;

            // Hide random words (initial hiding)
            List<int> hiddenWords = selectedScripture.HideRandomWords(4);
            if (hiddenWords == null)
            {
                Console.WriteLine("All words are hidden. Well done!");
                break;
            }

            // Clear console and display scripture with hidden words
            Console.Clear();
            selectedScripture.Display();

            // Show missing words positions
            selectedScripture.DisplayMissingWordPositions();

            // Ask user to fill in the missing words one by one
            foreach (var wordIndex in hiddenWords.OrderBy(index => selectedScripture.GetWordPosition(index)))
            {
                while (true) // Loop until the user gets the word correct
                {
                    Console.WriteLine($"What is word number {selectedScripture.GetWordPosition(wordIndex)}?");
                    string userGuess = Console.ReadLine();

                    if (selectedScripture.CheckUserGuess(userGuess, wordIndex))
                    {
                        Console.WriteLine("Correct!");
                        break; // Exit the loop if the guess is correct
                    }
                    else
                    {
                        Console.WriteLine("Wrong answer. Try again.");
                    }
                }
            }

            // All answers are correct, hide more words and reset indices
            selectedScripture.RemoveMoreWords(5);
            selectedScripture.ResetHiddenIndices();

            // Show all missing words after hiding more
            Console.Clear();
            selectedScripture.Display();
            selectedScripture.DisplayMissingWordPositions();

            // Ask user to guess all hidden words one by one
            List<int> allHiddenWords = selectedScripture.GetHiddenWordIndices();
            foreach (var wordIndex in allHiddenWords.OrderBy(index => selectedScripture.GetWordPosition(index)))
            {
                while (true) // Loop until the user gets the word correct
                {
                    Console.WriteLine($"What is word number {selectedScripture.GetWordPosition(wordIndex)}?");
                    string userGuess = Console.ReadLine();

                    if (selectedScripture.CheckUserGuess(userGuess, wordIndex))
                    {
                        Console.WriteLine("Correct!");
                        break; // Exit the loop if the guess is correct
                    }
                    else
                    {
                        Console.WriteLine("Wrong answer. Try again.");
                    }
                }
            }
        }
    }

    public static List<ScriptureText> LoadScriptures(string filename)
    {
        List<ScriptureText> scriptures = new List<ScriptureText>();

        try
        {
            string[] lines = File.ReadAllLines(filename);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2)
                {
                    string reference = parts[0];
                    string text = parts[1];

                    string[] refParts = reference.Split(' ');
                    string book = refParts[0];
                    string[] chapterAndVerses = refParts[1].Split(':');
                    int chapter = int.Parse(chapterAndVerses[0]);

                    string[] verses = chapterAndVerses[1].Split('-');
                    int verseStart = int.Parse(verses[0]);
                    int verseEnd = verseStart;
                    if (verses.Length > 1)
                    {
                        verseEnd = int.Parse(verses[1]);
                    }

                    ScriptureReference scriptureReference = new ScriptureReference(book, chapter, verseStart, verseEnd);
                    ScriptureText scripture = new ScriptureText(scriptureReference, text);
                    scriptures.Add(scripture);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading scriptures: {ex.Message}");
        }

        return scriptures;
    }
}