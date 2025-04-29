using Xunit;
using MovieLibrary.Models;
using MovieLibrary.Services; 

namespace MovieLibrary.Tests
{
    public class UnitTest1
    {
        private readonly MovieService _service;

        public UnitTest1()
        {
            _service = new MovieService();
        }

        [Fact]
        public void AddMovie_ShouldAddMovieSuccessfully()
        {
            // Arrange
            var movie = new Movie
            {
                ID = "M001",
                Title = "Inception",
                Director = "Christopher Nolan",
                Genre = "Sci-Fi",
                ReleaseYear = 2010
            };

            // Act
            _service.AddMovie(movie);

            // Assert
            var result = _service.SearchByID("M001");
            Assert.NotNull(result);
            Assert.Equal("Inception", result.Title);
            Assert.Equal("Christopher Nolan", result.Director);
            Assert.Equal("Sci-Fi", result.Genre);
            Assert.Equal(2010, result.ReleaseYear);
        }

        [Fact]
        public void AddMovie_ShouldThrowException_WhenDuplicateID()
        {
            // Arrange
            var movie1 = new Movie
            {
                ID = "M001",
                Title = "Inception",
                Director = "Christopher Nolan",
                Genre = "Sci-Fi",
                ReleaseYear = 2010
            };

            var movie2 = new Movie
            {
                ID = "M001", // Same ID as movie1
                Title = "Interstellar",
                Director = "Christopher Nolan",
                Genre = "Sci-Fi",
                ReleaseYear = 2014
            };

            _service.AddMovie(movie1);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _service.AddMovie(movie2));
            Assert.Equal("Duplicate Movie ID", exception.Message);
        }

        [Fact]
        public void SearchByTitle_ShouldReturnMatchingMovies()
        {
            // Arrange
            var movie1 = new Movie
            {
                ID = "M002",
                Title = "The Dark Knight",
                Director = "Christopher Nolan",
                Genre = "Action",
                ReleaseYear = 2008
            };

            var movie2 = new Movie
            {
                ID = "M003",
                Title = "Dark Waters",
                Director = "Todd Haynes",
                Genre = "Drama",
                ReleaseYear = 2019
            };

            var movie3 = new Movie
            {
                ID = "M004",
                Title = "Inception",
                Director = "Christopher Nolan",
                Genre = "Sci-Fi",
                ReleaseYear = 2010
            };

            _service.AddMovie(movie1);
            _service.AddMovie(movie2);
            _service.AddMovie(movie3);

            // Act
            var results = _service.SearchByTitle("Dark");

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count); // "The Dark Knight" and "Dark Waters" should match
            Assert.Contains(results, m => m.Title == "The Dark Knight");
            Assert.Contains(results, m => m.Title == "Dark Waters");
        }

        [Fact]
        public void SearchByTitle_ShouldReturnEmptyList_WhenNoMatches()
        {
            // Arrange
            var movie1 = new Movie
            {
                ID = "M005",
                Title = "Interstellar",
                Director = "Christopher Nolan",
                Genre = "Sci-Fi",
                ReleaseYear = 2014
            };

            var movie2 = new Movie
            {
                ID = "M006",
                Title = "Memento",
                Director = "Christopher Nolan",
                Genre = "Thriller",
                ReleaseYear = 2000
            };

            _service.AddMovie(movie1);
            _service.AddMovie(movie2);

            // Act
            var results = _service.SearchByTitle("Avengers"); // No movie with this title

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results); // The list should be empty
        }

        [Fact]
        public void SearchByID_ShouldReturnMovie_WhenExists()
        {
            // Arrange
            var movie = new Movie
            {
                ID = "M007",
                Title = "Tenet",
                Director = "Christopher Nolan",
                Genre = "Sci-Fi",
                ReleaseYear = 2020
            };

            _service.AddMovie(movie);

            // Act
            var result = _service.SearchByID("M007");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tenet", result.Title);
            Assert.Equal("Christopher Nolan", result.Director);
            Assert.Equal("Sci-Fi", result.Genre);
            Assert.Equal(2020, result.ReleaseYear);
        }

        [Fact]
        public void SearchByID_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var movie = new Movie
            {
                ID = "M008",
                Title = "Dunkirk",
                Director = "Christopher Nolan",
                Genre = "War",
                ReleaseYear = 2017
            };

            _service.AddMovie(movie);

            // Act
            var result = _service.SearchByID("M999"); // Random non-existent ID

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BorrowMovie_ShouldMarkMovieUnavailable()
        {
            // Arrange
            var movie = new Movie
            {
                ID = "M009",
                Title = "Batman Begins",
                Director = "Christopher Nolan",
                Genre = "Action",
                ReleaseYear = 2005,
                IsAvailable = true // Start as available
            };

            _service.AddMovie(movie);

            // Act
            _service.BorrowMovie("M009", "User1");

            // Assert
            var result = _service.SearchByID("M009");
            Assert.NotNull(result);
            Assert.False(result.IsAvailable); // Should now be unavailable
        }

        [Fact]
        public void BorrowMovie_ShouldAddUserToQueue_WhenMovieUnavailable()
        {
            // Arrange
            var movie = new Movie
            {
                ID = "M010",
                Title = "The Prestige",
                Director = "Christopher Nolan",
                Genre = "Drama",
                ReleaseYear = 2006,
                IsAvailable = true
            };

            _service.AddMovie(movie);

            // First borrow to make it unavailable
            _service.BorrowMovie("M010", "User1");

            // Act - Second user tries to borrow
            _service.BorrowMovie("M010", "User2"); // Should add to queue

            // Assert
            var assignedUser = _service.ReturnMovie("M010");
            Assert.Equal("User2", assignedUser); 
        }
        [Fact]
        public void ReturnMovie_ShouldAssignMovieToNextUserInQueue()
        {
            // Arrange
            var movie = new Movie
            {
                ID = "M011",
                Title = "Oppenheimer",
                Director = "Christopher Nolan",
                Genre = "Drama",
                ReleaseYear = 2023,
                IsAvailable = true
            };

            _service.AddMovie(movie);

        }
    }
}
