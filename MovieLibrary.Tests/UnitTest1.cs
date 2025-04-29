using Xunit;
using MovieLibrary.Models;
using MovieLibrary.Service;

namespace MovieLibrary.Tests {
    public class MovieServiceTests
    {
        private readonly MovieService _service;

        public MovieServiceTests()
        {
            _service = new MovieService();
        }

        [Fact]
        public void AddMovie_ShouldAddMovieSuccessfully() 
        {
            // Arrange
            var movie = new Movie
            {
                MovieID = "M001",
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
    }
}