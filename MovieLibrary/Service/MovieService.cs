using MovieLibrary.Models;
using MovieLibrary.DataStructures;

namespace MovieLibrary.Services
{
    public class MovieService
    {
        private HashTable<string, Movie> movieTable = new();
        private MovieLibrary.DataStructures.LinkedList<Movie> movieList = new();
        private Dictionary<string, MovieLibrary.DataStructures.Queue<string>> waitingLists = new();

        public void AddMovie(Movie movie)
        {
            if (movieTable.ContainsKey(movie.MovieID))
                throw new Exception("Duplicate Movie ID");
            movieTable.Add(movie.MovieID, movie);
            movieList.Add(movie);
        }

        public List<Movie> GetAllMovies() => movieList.ToList();

        public Movie SearchByID(string id) => movieTable.Get(id);

        public List<Movie> SearchByTitle(string title)
        {
            return movieList.ToList().Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        
    }
}
