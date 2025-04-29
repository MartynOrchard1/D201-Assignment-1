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
    }
}
