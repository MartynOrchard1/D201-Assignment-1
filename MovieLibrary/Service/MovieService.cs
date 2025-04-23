using MovieLibrary.Models;
using MovieLibrary.DataStructures;

namespace MovieLibrary.Services;
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

    public void BorrowMovie(string id, string user)
    {
        var movie = SearchByID(id);
        if (movie == null) return;
        if (movie.IsAvailable) movie.IsAvailable = false;
        else
        {
            if (!waitingLists.ContainsKey(id)) waitingLists[id] = new MovieLibrary.DataStructures.Queue<string>();
            waitingLists[id].Enqueue(user);
        }
    }

    public string ReturnMovie(string id)
    {
        var movie = SearchByID(id);
        if (movie == null) return null;
        if (waitingLists.ContainsKey(id) && !waitingLists[id].IsEmpty())
            return waitingLists[id].Dequeue();
        movie.IsAvailable = true;
        return null;
    }

    public List<Movie> BubbleSortByTitle()
    {
        var list = movieList.ToList();
        for (int i = 0; i < list.Count - 1; i++)
            for (int j = 0; j < list.Count - i - 1; j++)
                if (list[j].Title.CompareTo(list[j + 1].Title) > 0)
                    (list[j], list[j + 1]) = (list[j + 1], list[j]);
        return list;
    }

    public List<Movie> MergeSortByYear()
    {
        return MergeSort(movieList.ToList());
    }

    public List<Movie> SortByID()
    {
        var list = movieList.ToList();
        list.Sort((a, b) => a.MovieID.CompareTo(b.MovieID));
        return list;
    }


    private List<Movie> MergeSort(List<Movie> list)
    {
        if (list.Count <= 1) return list;
        int mid = list.Count / 2;
        var left = MergeSort(list.GetRange(0, mid));
        var right = MergeSort(list.GetRange(mid, list.Count - mid));
        return Merge(left, right);
    }

    private List<Movie> Merge(List<Movie> left, List<Movie> right)
    {
        List<Movie> result = new();
        int i = 0, j = 0;
        while (i < left.Count && j < right.Count)
        {
            if (left[i].ReleaseYear <= right[j].ReleaseYear) result.Add(left[i++]);
            else result.Add(right[j++]);
        }
        result.AddRange(left.Skip(i));
        result.AddRange(right.Skip(j));
        return result;
    }

    public void DeleteMovieById(string id)
    {
        var allMovies = movieList.ToList();
        var updated = allMovies.Where(m => m.MovieID != id).ToList();

        // Rebuild the linked list
        movieList = new MovieLibrary.DataStructures.LinkedList<Movie>();

        // Rebuild the hash table
        movieTable = new MovieLibrary.DataStructures.HashTable<string, Movie>();

        foreach (var movie in updated)
        {
            movieList.Add(movie);
            movieTable.Add(movie.MovieID, movie);
        }
    }

    public void ReplaceAll(List<Movie> movies)
    {
        movieList = new MovieLibrary.DataStructures.LinkedList<Movie>();
        movieTable = new MovieLibrary.DataStructures.HashTable<string, Movie>();

        foreach (var movie in movies)
        {
            movieList.Add(movie);
            movieTable.Add(movie.MovieID, movie);
        }
    }
}
