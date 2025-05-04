using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MovieLibrary.Models;
using MovieLibrary.Services;

namespace MovieLibrary.Views;

public partial class MainWindow : Window
{
    private readonly MovieService service = new();
    private int movieCounter = 1;
    private bool isTitleAscending = true;
    private bool isYearAscending = true;
    private bool isIdAscending = true;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void AddMovie_Click(object sender, RoutedEventArgs e)
    {
        // Validate empty fields
        if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
            string.IsNullOrWhiteSpace(txtDirector.Text) ||
            string.IsNullOrWhiteSpace(txtGenre.Text) ||
            string.IsNullOrWhiteSpace(txtYear.Text))
        {
            MessageBox.Show("Please fill in all fields before adding a movie.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Validate year input
        if (!int.TryParse(txtYear.Text, out int year))
        {
            MessageBox.Show("Release Year must be a valid number (e.g., 2020).", "Invalid Year", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Validate title (letters, numbers, spaces allowed)
        if (!System.Text.RegularExpressions.Regex.IsMatch(txtTitle.Text, @"^[a-zA-Z0-9\s]+$"))
        {
            MessageBox.Show("Title can only contain letters, numbers, and spaces (no special characters).", "Invalid Title", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Validate director (letters and spaces only)
        if (!System.Text.RegularExpressions.Regex.IsMatch(txtDirector.Text, @"^[a-zA-Z\s]+$"))
        {
            MessageBox.Show("Director name can only contain letters and spaces.", "Invalid Director", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Validate genre (letters and spaces only)
        if (!System.Text.RegularExpressions.Regex.IsMatch(txtGenre.Text, @"^[a-zA-Z\s]+$"))
        {
            MessageBox.Show("Genre can only contain letters and spaces.", "Invalid Genre", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Check for Duplicate title
        bool isDuplicate = service.GetAllMovies()
        .Any(m => m.Title.Equals(txtTitle.Text.Trim(), StringComparison.OrdinalIgnoreCase));
        if (isDuplicate)
        {
            MessageBox.Show("A movie with this title already exists.", "Duplicate Title", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Make sure the year is realistic
        if (year < 1888 || year > DateTime.Now.Year + 1)
        {
            MessageBox.Show($"Please enter a realistic year between 1888 and {DateTime.Now.Year + 1}.", "Invalid Year", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Make sure the title is not too long
        if (txtTitle.Text.Trim().Length > 100)
        {
            MessageBox.Show("Title is too long. Please keep it under 100 characters.", "Title Too Long", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Make sure the director name is not too short
        if (txtDirector.Text.Trim().Length < 2)
        {
            MessageBox.Show("Director name is too short.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Make sure the genre is not too short
        if (txtGenre.Text.Trim().Length < 3)
        {
            MessageBox.Show("Genre must be at least 3 characters long.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Make sure the year is not in the future
        if (year > DateTime.Now.Year)
        {
            MessageBox.Show("The release year can't be in the future.", "Invalid Year", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Check for exact duplicate (same title, director, and year)
        bool exactDuplicate = service.GetAllMovies()
            .Any(m =>
                m.Title.Equals(txtTitle.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                m.Director.Equals(txtDirector.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                m.ReleaseYear == year);

        if (exactDuplicate)
        {
            MessageBox.Show("An identical movie already exists with the same title, director, and year.", "Duplicate Movie", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Create movie if all validations pass
        var movie = new Movie
        {
            ID = $"M{movieCounter:D3}",
            Title = txtTitle.Text.Trim(),
            Director = txtDirector.Text.Trim(),
            Genre = txtGenre.Text.Trim(),
            ReleaseYear = int.Parse(txtYear.Text)
        };

        try
        {
            service.AddMovie(movie);
            movieCounter++;
            RefreshMovieList();
            ClearInputFields();
            MessageBox.Show("Movie added successfully.", "Add Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void Borrow_Click(object sender, RoutedEventArgs e)
    {
        if (movieListBox.SelectedItem is Movie selectedMovie)
        {
            try
            {
                if (selectedMovie.IsAvailable)
                {
                    service.BorrowMovie(selectedMovie.ID, "User1");
                    MessageBox.Show($"Movie '{selectedMovie.Title}' has been borrowed successfully.", "Borrow Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    service.AddToWaitingQueue(selectedMovie.ID, "User1");
                    MessageBox.Show($"Movie '{selectedMovie.Title}' is currently unavailable. You have been added to the waiting queue.", "Added to Queue", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                RefreshMovieList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please select a movie to borrow.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (movieListBox.SelectedItem is not Movie selected)
        {
            MessageBox.Show("Please select a movie to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Show confirmation dialog
        var confirm = MessageBox.Show
        (
            $"Are you sure you want to edit the movie '{selected.Title}'?", 
            "Confirm Edit", 
            MessageBoxButton.YesNo, 
            MessageBoxImage.Question
        );

        // Confirmation dialog
        if ( confirm != MessageBoxResult.Yes)
        {
            // User chose not to edit
            return;
        }

        if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
            string.IsNullOrWhiteSpace(txtDirector.Text) ||
            string.IsNullOrWhiteSpace(txtGenre.Text) ||
            string.IsNullOrWhiteSpace(txtYear.Text))
        {
            MessageBox.Show("Please fill in all fields before editing.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        selected.Title = txtTitle.Text.Trim();
        selected.Director = txtDirector.Text.Trim();
        selected.Genre = txtGenre.Text.Trim();
        selected.ReleaseYear = int.Parse(txtYear.Text);

        RefreshMovieList();
        ClearInputFields();

        MessageBox.Show("Movie updated successfully.", "Edit Complete", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void Return_Click(object sender, RoutedEventArgs e)
    {
        if (movieListBox.SelectedItem is Movie selected)
        {
            try
            {
                var nextUser = service.ReturnMovie(selected.ID);
                RefreshMovieList();

                if (nextUser != null)
                {
                    MessageBox.Show($"Movie '{selected.Title}' has been returned and assigned to the next user in the queue: {nextUser}.", "Return Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                    service.AddNotification($"Movie '{selected.Title}' has been assigned to {nextUser}.");
                }
                else
                {
                    MessageBox.Show($"Movie '{selected.Title}' has been returned and is now available.", "Return Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please select a movie to return.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void SortTitle_Click(object sender, RoutedEventArgs e)
    {
        var sorted = service.BubbleSortByTitle();
        if (!isTitleAscending) sorted.Reverse();
        movieListBox.ItemsSource = sorted;
        isTitleAscending = !isTitleAscending;
    }

    private void SortYear_Click(object sender, RoutedEventArgs e)
    {
        var sorted = service.MergeSortByYear();
        if (!isYearAscending) sorted.Reverse();
        movieListBox.ItemsSource = sorted;
        isYearAscending = !isYearAscending;
    }

    private void SortID_Click(object sender, RoutedEventArgs e)
    {
        var sorted = service.SortByID();
        if (!isIdAscending) sorted.Reverse();
        movieListBox.ItemsSource = sorted;
        isIdAscending = !isIdAscending;
    }

    private void SortAvailability_Click(object sender, RoutedEventArgs e)
    {
        var sorted = service.SortByAvailability();
        movieListBox.ItemsSource = sorted;
    }
    
    private void SortGenre_Click(object sender, RoutedEventArgs e)
    {
        var sorted = service.SortByGenre();
        movieListBox.ItemsSource = sorted;
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        if (cmbSearchType.SelectedItem is ComboBoxItem selectedItem)
        {
            string searchType = selectedItem.Content.ToString();
            string searchText = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Please enter a search term.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (searchType == "Search by Title")
            {
                var results = service.SearchByTitle(searchText);
                if (results.Count == 0)
                {
                    MessageBox.Show("No movies found.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                movieListBox.ItemsSource = results;
            }
            else if (searchType == "Search by MovieID")
            {
                var result = service.GetAllMovies().FirstOrDefault(m => m.ID.Equals(searchText, StringComparison.OrdinalIgnoreCase));
                if (result == null)
                {
                    MessageBox.Show("No movie found with the given ID.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                movieListBox.ItemsSource = new List<Movie> { result };
            }
        }
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        RefreshMovieList();
        txtSearch.Clear();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (movieListBox.SelectedItem is Movie selected)
        {
            var confirm = MessageBox.Show($"Are you sure you want to delete '{selected.Title}'?", "Confirm Delete", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                service.DeleteMovieById(selected.ID);
                RefreshMovieList();
                ClearInputFields();
                MessageBox.Show("Movie deleted successfully.", "Delete Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            MessageBox.Show("Please select a movie to delete.");
        }
    }

    private void movieListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (movieListBox.SelectedItem is Movie selected)
        {
            txtTitle.Text = selected.Title;
            txtDirector.Text = selected.Director;
            txtGenre.Text = selected.Genre;
            txtYear.Text = selected.ReleaseYear.ToString();
        }
    }

    private void RefreshMovieList()
    {
        movieListBox.ItemsSource = null;
        movieListBox.ItemsSource = service.GetAllMovies();
    }

    private void ClearInputFields()
    {
        txtTitle.Clear();
        txtDirector.Clear();
        txtGenre.Clear();
        txtYear.Clear();
    }

    private void SaveToFile_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog dialog = new SaveFileDialog
        {
            Filter = "JSON Files (*.json)|*.json"
        };

        if (dialog.ShowDialog() == true)
        {
            var data = new
            {
                Movies = service.GetAllMovies(),
                Notifications = service.ExportNotifications(),
                ActivityLogs = service.GetActivityLog() // ✅ added
            };

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(dialog.FileName, json);
            MessageBox.Show("Movies, notifications, and logs saved successfully.", "Save Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void LoadFromFile_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog
        {
            Filter = "JSON Files (*.json)|*.json"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                var json = File.ReadAllText(dialog.FileName);
                var data = JsonSerializer.Deserialize<SaveData>(json);

                if (data?.Movies != null)
                {
                    // Validate each movie to ensure no null or invalid values
                    foreach (var movie in data.Movies)
                    {
                        if (string.IsNullOrWhiteSpace(movie.ID) ||
                            string.IsNullOrWhiteSpace(movie.Title) ||
                            string.IsNullOrWhiteSpace(movie.Director) ||
                            string.IsNullOrWhiteSpace(movie.Genre) ||
                            movie.ReleaseYear <= 0)
                        {
                            throw new InvalidDataException("The JSON file contains invalid or missing data.");
                        }
                    }

                    service.ReplaceAll(data.Movies);
                    RefreshMovieList();

                    if (data.Movies.Any())
                    {
                        var maxId = data.Movies
                            .Select(m => int.TryParse(m.ID.TrimStart('M'), out int id) ? id : 0)
                            .Max();
                        movieCounter = maxId + 1;
                    }
                    else
                    {
                        movieCounter = 1;
                    }

                    // Load notifications
                    if (data.Notifications != null)
                    {
                        foreach (var notification in data.Notifications)
                        {
                            service.AddNotification(notification);
                        }
                    }

                    // Load activity logs
                    if (data.ActivityLogs != null)
                    {
                        service.ImportActivityLog(data.ActivityLogs);
                    }

                    MessageBox.Show("Movies, notifications and activity logs loaded successfully.", "Load Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    throw new InvalidDataException("The JSON file is empty or invalid.");
                }
            }
            catch (InvalidDataException ex)
            {
                MessageBox.Show($"Failed to load file. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load file. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
