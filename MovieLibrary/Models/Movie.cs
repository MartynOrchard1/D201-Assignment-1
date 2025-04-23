using System.Diagnostics.CodeAnalysis;
namespace MovieLibrary.Models;

public class Movie 
{
    public string ID { get; set; }
    public required string Title { get; set; } 
    public required string Director { get; set; } 
    public required string Genre { get; set; } 
    public int ReleaseYear { get; set; }
    public bool IsAvailable { get; set; } = true;
}
