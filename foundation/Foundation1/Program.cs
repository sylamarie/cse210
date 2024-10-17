using System;
using System.Collections.Generic;

// Comment Class
class Comment
{
    public string _name { get; set; }
    public string _text { get; set; }

    public Comment(string name, string text)
    {
        _name = name;
        _text = text;
    }

    public void Display()
    {
        Console.WriteLine($"Comment by {_name}: {_text}");
    }
}

// Video Class
class Video
{
    public string _title { get; set; }
    public string _author { get; set; }
    public int _length { get; set; } // in seconds
    public List<Comment> _comments { get; set; }

    public Video(string title, string author, int length)
    {
        _title = title;
        _author = author;
        _length = length;
        _comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetNumComments()
    {
        return _comments.Count;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {_title}");
        Console.WriteLine($"Author: {_author}");
        Console.WriteLine($"Length: {_length} seconds");
        Console.WriteLine($"Number of Comments: {GetNumComments()}");
        Console.WriteLine("Comments:");
        foreach (var comment in _comments)
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
        Video video1 = new Video("Learn C# in 10 Minutes", "John Doe", 600);
        Video video2 = new Video("Understanding LINQ", "Jane Smith", 900);
        Video video3 = new Video("Mastering ASP.NET", "Alex Brown", 1200);

        // Adding comments to video1
        video1.AddComment(new Comment("Alice", "Great introduction!"));
        video1.AddComment(new Comment("Bob", "Really helpful, thanks!"));
        video1.AddComment(new Comment("Charlie", "I learned a lot in just 10 minutes!"));

        // Adding comments to video2
        video2.AddComment(new Comment("Dave", "LINQ is confusing, but this video helped."));
        video2.AddComment(new Comment("Eve", "Thanks for the clear explanation!"));
        video2.AddComment(new Comment("Frank", "Could you make a follow-up on advanced LINQ?"));

        // Adding comments to video3
        video3.AddComment(new Comment("Grace", "This ASP.NET tutorial is a lifesaver!"));
        video3.AddComment(new Comment("Helen", "Clear and concise. I appreciate it."));
        video3.AddComment(new Comment("Ivy", "I'm ready to build web apps now!"));

        // Storing videos in a list
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Displaying video information and comments
        foreach (var video in videos)
        {
            video.DisplayInfo();
        }
    }
}