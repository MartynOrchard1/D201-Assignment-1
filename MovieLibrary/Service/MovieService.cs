using MovieLibrary.Models;
using MovieLibrary.DataStructures;
using System.Collections.Generic;

namespace MovieLibrary.Services;

public class MovieService
{
    private HashTable<string, Movie> movieTable = new();
    private MovieLibrary.DataStructures.LinkedList<Movie> movieList = new();
    private readonly Dictionary<string, System.Collections.Generic.Queue<string>> waitingLists = new();
    private readonly System.Collections.Generic.Queue<string> notifications = new();

    public void AddMovie(Movie movie)
    {
        if (movieTable.ContainsKey(movie.ID))
            throw new Exception("Duplicate Movie ID");

        movieTable.Add(movie.ID, movie);
        movieList.Add(movie);
    }

    public List<Movie> GetAllMovies() => movieList.ToList();

    public Movie SearchByID(string id) => movieTable.Get(id);

    public List<Movie> SearchByTitle(string title) =>
        movieList.ToList()
                 .Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                 .ToList();

    public void BorrowMovie(string id, string user)
    {
        var movie = SearchByID(id);
        if (movie == null)
            throw new Exception("Movie not found");

        if (!movie.IsAvailable)
        {
            if (!waitingLists.ContainsKey(id))
                waitingLists[id] = new System.Collections.Generic.Queue<string>();

            if (!waitingLists[id].Contains(user))
            {
                waitingLists[id].Enqueue(user);
                LogActivity($"User '{user}' attempted to borrow '{movie.Title}' but was added to the waiting list.");
            }
            else
            {
                AddNotification($"User '{user}' is already in the waiting queue for movie '{movie.Title}'.");
                LogActivity($"User '{user}' attempted to borrow '{movie.Title}' but was already in the waiting list.");
            }

            return;
        }

        movie.IsAvailable = false; // Mark the movie as unavailable
        LogActivity($"User '{user}' borrowed movie '{movie.Title}'.");
    }


    public string ReturnMovie(string id)
        {
        var movie = SearchByID(id);
        if (movie == null)
            throw new Exception("Movie not found");

        movie.IsAvailable = true; // Mark the movie as available again
        LogActivity($"Movie '{movie.Title}' was returned.");

        if (waitingLists.ContainsKey(id) && waitingLists[id].Count > 0)
        {
            string nextUser = waitingLists[id].Dequeue();
            AddNotification($"Movie '{movie.Title}' is now available for user '{nextUser}'.");
            LogActivity($"Movie '{movie.Title}' is now available and next user '{nextUser}' was notified.");
            return nextUser;
        }

        return null;
    }


    public List<Movie> BubbleSortByTitle()
    {
        var list = movieList.ToList();
        for (int i = 0; i < list.Count - 1; i++)
            for (int j = 0; j < list.Count - i - 1; j++)
                if (list[j].Title.CompareTo(list[j + 1].Title) > 0)
                {
                    (list[j], list[j + 1]) = (list[j + 1], list[j]);
                }
        return list;
    }

    public List<Movie> MergeSortByYear() => MergeSort(movieList.ToList());

    public List<Movie> SortByID()
    {
        var list = movieList.ToList();
        list.Sort((a, b) => a.ID.CompareTo(b.ID));
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
            if (left[i].ReleaseYear <= right[j].ReleaseYear)
                result.Add(left[i++]);
            else
                result.Add(right[j++]);
        }
        result.AddRange(left.Skip(i));
        result.AddRange(right.Skip(j));
        return result;
    }

    public void DeleteMovieById(string id)
    {
        var allMovies = movieList.ToList();
        var updated = allMovies.Where(m => m.ID != id).ToList();

        movieList = new MovieLibrary.DataStructures.LinkedList<Movie>();
        movieTable = new MovieLibrary.DataStructures.HashTable<string, Movie>();

        foreach (var movie in updated)
        {
            movieList.Add(movie);
            movieTable.Add(movie.ID, movie);
        }
    }

    public void ReplaceAll(List<Movie> movies)
    {
        movieList = new MovieLibrary.DataStructures.LinkedList<Movie>();
        movieTable = new MovieLibrary.DataStructures.HashTable<string, Movie>();

        foreach (var movie in movies)
        {
            movieList.Add(movie);
            movieTable.Add(movie.ID, movie);
        }
    }

    public void AddToWaitingQueue(string movieId, string user)
    {
        if (!waitingLists.ContainsKey(movieId))
            waitingLists[movieId] = new System.Collections.Generic.Queue<string>();

        waitingLists[movieId].Enqueue(user);
    }

    public void AddNotification(string message)
    {
        notifications.Enqueue(message);
    }

    public List<string> ExportNotifications()
    {
        var exported = notifications.ToList();
        notifications.Clear();
        return exported;
    }

    // Activity Log to record borrw/return events
    private readonly List<string> activityLog = new();

    private void LogActivity(string activity)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        activityLog.Add($"[{timestamp}] {activity}");
    }

    public List<string> GetActivityLog() => new(activityLog); // Return a copy of the activity log

    public void ImportActivityLog(List<string> logs)
    {
        foreach (var entry in logs)
        {
            activityLog.Add(entry);
        }
    }
}
