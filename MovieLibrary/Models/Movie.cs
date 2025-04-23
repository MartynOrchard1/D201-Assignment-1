using System.Diagnostics.CodeAnalysis;
namespace MovieLibrary.Models;
using System.Collections.Generic;

public class Movie 
{
    public string ID { get; set; }
    public required string Title { get; set; } 
    public required string Director { get; set; } 
    public required string Genre { get; set; } 
    public int ReleaseYear { get; set; }
    public bool IsAvailable { get; set; } = true;
}

public class SaveData
{
    public List<Movie> Movies { get; set; }
    public List<string> Notifications { get; set; }
}