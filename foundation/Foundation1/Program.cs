using System;
using System.Collections.Generic;

// Comment Class
class Comment
{
    public string Name { get; set; }
    public string Text { get; set; }

    public Comment(string name, string text)
    {
        Name = name;
        Text = text;
    }

    public void Display()
    {
        Console.WriteLine($"Comment by {Name}: {Text}");
    }
}

// Video Class
class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; } // in seconds
    public List<Comment> Comments { get; set; }

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        Comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public int GetNumComments()
    {
        return Comments.Count;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {Length} seconds");
        Console.WriteLine($"Number of Comments: {GetNumComments()}");
        Console.WriteLine("Comments:");
        foreach (var comment in Comments)
        {
            comment.Display();
        }
        Console.WriteLine(); // For separation between videos
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        // Creating sample videos
        Video video1 = new Video("C# Basics Tutorial", "John Doe", 1200);
        Video video2 = new Video("Advanced JavaScript", "Jane Smith", 900);
        Video video3 = new Video("Introduction to Data Science", "Sam Lee", 1500);

        // Adding comments to video1
        video1.AddComment(new Comment("Alice", "Great video, very informative!"));
        video1.AddComment(new Comment("Bob", "I loved the explanation of loops."));
        video1.AddComment(new Comment("Charlie", "Could you cover more on OOP?"));

        // Adding comments to video2
        video2.AddComment(new Comment("Dave", "Super helpful!"));
        video2.AddComment(new Comment("Eve", "Clear and concise, thanks!"));

        // Adding comments to video3
        video3.AddComment(new Comment("Frank", "Awesome introduction to data science."));
        video3.AddComment(new Comment("Grace", "Thanks for the insights!"));

        // Storing videos in a list
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Displaying video information and comments
        foreach (var video in videos)
        {
            video.DisplayInfo();
        }
    }
}