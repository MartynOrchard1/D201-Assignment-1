using MovieLibrary.Models;
using MovieLibrary.DataStructures;

namespace MovieLibrary.Services
{
    public class MovieService
    {
        private HashTable<string, Movie> movieTable = new();
        private MovieLibrary.DataStructures.LinkedList<Movie> movieList = new();
        private Dictionary<string, MovieLibrary.DataStructures.Queue<string>> waitingLists = new();

        
    }
}
