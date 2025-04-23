using System.ComponentModel.Design;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieLibrary.Views
{
    // !! Currently not used - just placeholders !! 

    // private readonly MovieService service = new();
    // private interface movieCounter = 1;     
    // private bool isTitleAscending = true;
    // private bool isYearAscending = true;
    // private bool isIdAscending = true;
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void AddMovie_Click(object sender, RoutedEventArgs e)
        {
            var movie = new Movie
            {
                MovieID = $"M{movieCounter:D3}",
                Title = txtTitle.Text.Trim().ToUpper(),
                Director = txtDirector.Text.Trim().ToUpper(),
                Genre = txtGenre.Text.Trim().ToUpper(),
                ReleaseYear = year
            };

            try
            {
                service.AddMovie(movie);
                movieCounter++;
                RefreshMovieList();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            if (movieList.SelectedItem is Movie selectedMovie)
            {
                try
                {
                    service.BorrowMovie(selectedMovie.MovieID);
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

            // Validate fields (same as AddMovie_Click)
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtDirector.Text) ||
                string.IsNullOrWhiteSpace(txtGenre.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text))
            {
                MessageBox.Show("Please fill in all fields before editing.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update the selected movie
            selected.Title = txtTitle.Text.Trim();
            selected.Director = txtDirector.Text.Trim();
            selected.Genre = txtGenre.Text.Trim();
            selected.ReleaseYear = year;

            // RefreshMovieList();
            // ClearInputFields();

            MessageBox.Show("Movie updated successfully.", "Edit Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            if (movieListBox.SelectedItem is Movie selected)
            {
                var movie = service.SearchByID(selected.MovieID);

                if (movie == null)
                {
                    MessageBox.Show("Movie not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (movie.IsAvailable)
                {
                    MessageBox.Show("This movie is already marked as available. It may have already been returned.", "Already Available", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var nextUser = service.ReturnMovie(selected.MovieID);
                RefreshMovieList();

                if (nextUser != null)
                {
                    MessageBox.Show($"Movie returned successfully and has been assigned to {nextUser}.", "Next User Notified", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Movie returned successfully and is now available for borrowing.", "Returned", MessageBoxButton.OK, MessageBoxImage.Information);
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

            if (!isTitleAscending)
                sorted.Reverse();

            movieListBox.ItemsSource = sorted;
            isTitleAscending = !isTitleAscending; // Toggle sort direction
        }

        private void SortYear_Click(object sender, RoutedEventArgs e)
        {
            var sorted = service.MergeSortByYear();

            if (!isYearAscending)
                sorted.Reverse();

            movieListBox.ItemsSource = sorted;
            isYearAscending = !isYearAscending; // Toggle sort direction
        }

        private void SortID_Click(object sender, RoutedEventArgs e)
        {
            var sorted = service.SortByID();

            if (!isIdAscending)
                sorted.Reverse();

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
                        MessageBox.Show("No movies found with the given title.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    movieListBox.ItemsSource = results;
                }
                else if (searchType == "Search by MovieID")
                {
                    var result = service.GetAllMovies().FirstOrDefault(m => m.MovieID.Equals(searchText, StringComparison.OrdinalIgnoreCase));
                    if (result == null)
                    {
                        MessageBox.Show("No movie found with the given MovieID.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    service.DeleteMovieById(selected.MovieID);
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

        private void movieListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (movieListBox.SelectedItem is Movie selected)
            {
                txtTitle.Text = selected.Title;
                txtDirector.Text = selected.Director;
                txtGenre.Text = selected.Genre;
                txtYear.Text = selected.ReleaseYear.ToString();
            }
        }

        // Refresh the movie list in the UI
        private void RefreshMovieList()
        {
            movieListBox.ItemsSource = null; // Clear the current list
            movieListBox.ItemsSource = service.GetAllMovies(); // Load the updated list of movies
        }
    }
}