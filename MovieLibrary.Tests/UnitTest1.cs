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

    }
}
