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

    }
}