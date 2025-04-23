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
        var movie = new Movie
        {
            ID = $"M{movieCounter:D3}",
            Title = txtTitle.Text.Trim().ToUpper(),
            Director = txtDirector.Text.Trim().ToUpper(),
            Genre = txtGenre.Text.Trim().ToUpper(),
            ReleaseYear = int.Parse(txtYear.Text)
        };

        try
        {
            service.AddMovie(movie);
            movieCounter++;
            RefreshMovieList();
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
                service.BorrowMovie(selectedMovie.ID, "User1");
                RefreshMovieList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (movieListBox.SelectedItem is not Movie selected)
        {
            MessageBox.Show("Please select a movie to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            var movie = service.SearchByID(selected.ID);

            if (movie == null)
            {
                MessageBox.Show("Movie not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (movie.IsAvailable)
            {
                MessageBox.Show("This movie is already marked as available.", "Already Available", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var nextUser = service.ReturnMovie(selected.ID);
            RefreshMovieList();

            MessageBox.Show(
                nextUser != null
                    ? $"Movie returned and assigned to {nextUser}."
                    : "Movie returned and is now available.",
                "Return Complete", MessageBoxButton.OK, MessageBoxImage.Information);
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
            var json = JsonSerializer.Serialize(service.GetAllMovies());
            File.WriteAllText(dialog.FileName, json);
            MessageBox.Show("Movies saved!");
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
                var loaded = JsonSerializer.Deserialize<List<Movie>>(json);

                if (loaded != null)
                {
                    service.ReplaceAll(loaded);
                    RefreshMovieList();

                    if (loaded.Any())
                    {
                        var maxId = loaded
                            .Select(m => int.TryParse(m.ID.TrimStart('M'), out int id) ? id : 0)
                            .Max();
                        movieCounter = maxId + 1;
                    }
                    else
                    {
                        movieCounter = 1;
                    }

                    MessageBox.Show("Movies loaded.");
                }
            }
            catch
            {
                MessageBox.Show("Failed to load file.");
            }
        }
    }
}
