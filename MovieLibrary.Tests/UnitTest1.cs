using Xunit;
using MovieLibrary.Models;
using MovieLibrary.Services; 
using MovieLibrary.DataStructures;
using CustomQueue = MovieLibrary.DataStructures.Queue<string>;


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

            // First borrow to make unavailable
            _service.BorrowMovie("M011", "User1");

            // Second user joins the queue
            _service.BorrowMovie("M011", "User2");

            // Act
            var assignedUser = _service.ReturnMovie("M011");

            // Assert
            Assert.Equal("User2", assignedUser);
        }

        [Fact]
        public void ReturnMovie_ShouldMarkMovieAvailable_WhenNoQueue()
        {
            // Arrange
            var movie = new Movie
            {
                ID = "M012",
                Title = "Following",
                Director = "Christopher Nolan",
                Genre = "Thriller",
                ReleaseYear = 1998,
                IsAvailable = true
            };

            _service.AddMovie(movie);

            // Borrow the movie to make it unavailable
            _service.BorrowMovie("M012", "User1");

            // Act
            var assignedUser = _service.ReturnMovie("M012");

            // Assert
            var result = _service.SearchByID("M012");
            Assert.NotNull(result);
            Assert.True(result.IsAvailable); 
            Assert.Null(assignedUser); 
        }

        [Fact]
        public void BubbleSortByTitle_ShouldSortMoviesAlphabetically()
        {
            // Arrange
            var movie1 = new Movie
            {
                ID = "M013",
                Title = "Zodiac",
                Director = "David Fincher",
                Genre = "Thriller",
                ReleaseYear = 2007
            };

            var movie2 = new Movie
            {
                ID = "M014",
                Title = "A Beautiful Mind",
                Director = "Ron Howard",
                Genre = "Drama",
                ReleaseYear = 2001
            };

            var movie3 = new Movie
            {
                ID = "M015",
                Title = "Memento",
                Director = "Christopher Nolan",
                Genre = "Thriller",
                ReleaseYear = 2000
            };

            _service.AddMovie(movie1);
            _service.AddMovie(movie2);
            _service.AddMovie(movie3);

            // Act
            var sortedMovies = _service.BubbleSortByTitle();

            // Assert
            Assert.Equal(3, sortedMovies.Count);
            Assert.Equal("A Beautiful Mind", sortedMovies[0].Title);
            Assert.Equal("Memento", sortedMovies[1].Title);
            Assert.Equal("Zodiac", sortedMovies[2].Title);
        }

        [Fact]
        public void MergeSortByYear_ShouldSortMoviesByReleaseYear()
        {
            // Arrange
            var movie1 = new Movie
            {
                ID = "M016",
                Title = "The Social Network",
                Director = "David Fincher",
                Genre = "Drama",
                ReleaseYear = 2010
            };

            var movie2 = new Movie
            {
                ID = "M017",
                Title = "Se7en",
                Director = "David Fincher",
                Genre = "Thriller",
                ReleaseYear = 1995
            };

            var movie3 = new Movie
            {
                ID = "M018",
                Title = "Gone Girl",
                Director = "David Fincher",
                Genre = "Thriller",
                ReleaseYear = 2014
            };

            _service.AddMovie(movie1);
            _service.AddMovie(movie2);
            _service.AddMovie(movie3);

            // Act
            var sortedMovies = _service.MergeSortByYear();

            // Assert
            Assert.Equal(3, sortedMovies.Count);
            Assert.Equal("Se7en", sortedMovies[0].Title); // 1995
            Assert.Equal("The Social Network", sortedMovies[1].Title); // 2010
            Assert.Equal("Gone Girl", sortedMovies[2].Title); // 2014
        }
        [Fact]
        public void SortByID_ShouldSortMoviesByID()
        {
            // Arrange
            var movie1 = new Movie
            {
                ID = "M003",
                Title = "Movie C",
                Director = "Director 3",
                Genre = "Genre 3",
                ReleaseYear = 2003
            };

            var movie2 = new Movie
            {
                ID = "M001",
                Title = "Movie A",
                Director = "Director 1",
                Genre = "Genre 1",
                ReleaseYear = 2001
            };

            var movie3 = new Movie
            {
                ID = "M002",
                Title = "Movie B",
                Director = "Director 2",
                Genre = "Genre 2",
                ReleaseYear = 2002
            };

            _service.AddMovie(movie1);
            _service.AddMovie(movie2);
            _service.AddMovie(movie3);

            // Act
            var sortedMovies = _service.SortByID();

            // Assert
            Assert.Equal(3, sortedMovies.Count);
            Assert.Equal("M001", sortedMovies[0].ID);
            Assert.Equal("M002", sortedMovies[1].ID);
            Assert.Equal("M003", sortedMovies[2].ID);
        }

        [Fact]
        public void DeleteMovieById_ShouldRemoveCorrectMovie()
        {
            // Arrange
            var movie1 = new Movie
            {
                ID = "M020",
                Title = "Movie One",
                Director = "Director One",
                Genre = "Genre One",
                ReleaseYear = 2001
            };

            var movie2 = new Movie
            {
                ID = "M021",
                Title = "Movie Two",
                Director = "Director Two",
                Genre = "Genre Two",
                ReleaseYear = 2002
            };

            _service.AddMovie(movie1);
            _service.AddMovie(movie2);

            // Act
            _service.DeleteMovieById("M020");

            // Assert
            var deletedMovie = _service.SearchByID("M020");
            var remainingMovie = _service.SearchByID("M021");

            Assert.Null(deletedMovie); // M020 should be gone
            Assert.NotNull(remainingMovie); // M021 should still exist
            Assert.Equal("Movie Two", remainingMovie.Title);
        }

        [Fact]
        public void ReplaceAll_ShouldReplaceAllMoviesSuccessfully()
        {
            // Arrange
            var originalMovie = new Movie
            {
                ID = "M030",
                Title = "Old Movie",
                Director = "Old Director",
                Genre = "Old Genre",
                ReleaseYear = 1990
            };

            _service.AddMovie(originalMovie);

            var newMovies = new List<Movie>
            {
                new Movie
                {
                    ID = "M031",
                    Title = "New Movie 1",
                    Director = "Director 1",
                    Genre = "Genre 1",
                    ReleaseYear = 2020
                },
                new Movie
                {
                    ID = "M032",
                    Title = "New Movie 2",
                    Director = "Director 2",
                    Genre = "Genre 2",
                    ReleaseYear = 2021
                }
            };

            // Act
            _service.ReplaceAll(newMovies);

            // Assert
            var oldMovie = _service.SearchByID("M030");
            var newMovie1 = _service.SearchByID("M031");
            var newMovie2 = _service.SearchByID("M032");

            Assert.Null(oldMovie); // Old movie should be gone
            Assert.NotNull(newMovie1); // New movies should exist
            Assert.NotNull(newMovie2);
        }
    }
    public class QueueTests {
        [Fact]
        public void Enqueue_ShouldIncreaseCount()
        {
            var queue = new CustomQueue();
            queue.Enqueue("First");
            Assert.Equal(1, queue.Count());
        }

        [Fact]
        public void Dequeue_ShouldReturnFirstItem()
        {
            var queue = new CustomQueue();
            queue.Enqueue("First");
            queue.Enqueue("Second");

            var item = queue.Dequeue();
            Assert.Equal("First", item);
            Assert.Equal(1, queue.Count()); // Fixed
        }

        [Fact]
        public void IsEmpty_ShouldReturnTrue_WhenQueueIsEmpty()
        {
            var queue = new CustomQueue();
            Assert.True(queue.isEmpty());
        }
        
        [Fact]
        public void IsEmpty_ShouldReturnFalse_WhenQueueHasItems()
        {
            var queue = new CustomQueue();
            queue.Enqueue("Item");
            Assert.False(queue.isEmpty());
        }
    }
    public class newReleaseTests 
    {
        // These tests are for release V1.1.0
        [Fact]
        public void BorrowMovie_ShouldAddLogEntry()
        {
            // Arrange
            var service = new MovieService();
            var movie = new Movie { ID = "M001", Title = "Inception", Director = "Nolan", Genre = "Thriller", ReleaseYear = 2010 };
            service.AddMovie(movie);

            // Act
            service.BorrowMovie("M001", "User1");

            // Assert
            var logs = service.GetActivityLog();
            Assert.Single(logs);
            Assert.Contains("User 'User1' borrowed movie 'Inception'", logs[0]);
        }

        [Fact]
        public void ReturnMovie_ShouldAddLogEntry()
        {
            // Arrange
            var service = new MovieService();
            var movie = new Movie { ID = "M002", Title = "Matrix", Director = "Wachowski", Genre = "Sci-Fi", ReleaseYear = 1999, IsAvailable = false };
            service.AddMovie(movie);

            // Act
            service.ReturnMovie("M002");

            // Assert
            var logs = service.GetActivityLog();
            Assert.Single(logs);
            Assert.Contains("Movie 'Matrix' was returned", logs[0]);
        }
                
        [Fact]
        public void SortByAvailability_ShouldSortWithAvailableMoviesFirst()
        {
            // Arrange
            var service = new MovieService();
            service.AddMovie(new Movie { ID = "M1", Title = "A", IsAvailable = false, Director = "Test", Genre = "Test", ReleaseYear = 2000 });
            service.AddMovie(new Movie { ID = "M2", Title = "B", IsAvailable = true, Director = "Test", Genre = "Test", ReleaseYear = 2000 });

            // Act
            var result = service.SortByAvailability();

            // Assert
            Assert.Equal("B", result[0].Title); // Available movie should be first
        }

        [Fact]
        public void SortByGenre_ShouldSortAlphabeticallyByGenreThenTitle()
        {
            // Arrange
            var service = new MovieService();
            service.AddMovie(new Movie { ID = "M1", Title = "A", IsAvailable = false, Director = "Test", Genre = "Horror", ReleaseYear = 2000 });
            service.AddMovie(new Movie { ID = "M2", Title = "A", IsAvailable = false, Director = "Test", Genre = "Action", ReleaseYear = 2000 });

            // Act
            var result = service.SortByGenre();

            // Assert
            Assert.Equal("A", result[0].Title); // Action comes before Horror
        }

        [Fact]
        public void ImportActivityLog_ShouldRestoreLogsCorrectly()
        {
            // Arrange
            var service = new MovieService();
            var testLogs = new List<string>
            {
                "[2025-05-04 12:00:00] User 'X' borrowed movie 'Y'."
            };

            // Act
            service.ImportActivityLog(testLogs);
            var result = service.GetActivityLog();

            // Assert
            Assert.Single(result);
            Assert.Equal(testLogs[0], result[0]);
        }
        [Fact]
        public void ClearNotifications_ShouldEmptyNotificationList()
        {
            // Arrange
            var service = new MovieService();
            var movie = new Movie { ID = "M1", Title = "Test", Director = "Test", Genre = "Test", ReleaseYear = 2000 };
            service.AddMovie(movie);
            service.AddToWaitingQueue("M1", "User1"); // Adds one notification

            // Act
            service.ClearNotifications();
            var result = service.ExportNotifications();

            // Assert
            Assert.Empty(result); // Notifications should be cleared
        }

        [Fact]
        public void BorrowMovie_ShouldNotDuplicateUserInQueue_AndLogNotification()
        {
            // Arrange
            var service = new MovieService();
            var movie = new Movie { ID = "M1", Title = "TestMovie", Director = "Test", Genre = "Test", ReleaseYear = 2024 };
            service.AddMovie(movie);

            // First borrow to make it unavailable
            service.BorrowMovie("M1", "User1");

            // Add the same user to the waiting queue
            service.AddToWaitingQueue("M1", "User1");

            // Act
            service.BorrowMovie("M1", "User1"); // Should hit ELSE block

            // Assert
            var logs = service.GetActivityLog();
            var exported = service.ExportNotifications();

            Assert.Contains(logs, log => log.Contains("already in the waiting list"));
            Assert.Contains(exported, msg => msg.Contains("already in the waiting queue"));
        }

        [Fact]
        public void AddToWaitingQueue_ShouldAddUserToNewQueue()
        {
            // Arrange
            var service = new MovieService();
            var movie = new Movie { ID = "M1", Title = "Blade Runner", Director = "Ridley Scott", Genre = "Sci-Fi", ReleaseYear = 1982 };
            service.AddMovie(movie);

            // Act
            service.AddToWaitingQueue("M1", "User42");

            // Re-borrow movie to check notification triggers queue
            service.BorrowMovie("M1", "SomeoneElse"); // Ensure it's unavailable
            service.BorrowMovie("M1", "User42");

            var exported = service.ExportNotifications();

            // Assert
            Assert.Contains(exported, msg => msg.Contains("already in the waiting queue"));
        }

        [Fact]
        public void ExportNotifications_ShouldReturnAndClearNotifications()
        {
            // Arrange
            var service = new MovieService();
            var movie = new Movie
            {
                ID = "M1",
                Title = "Avatar",
                Director = "Cameron",
                Genre = "Action",
                ReleaseYear = 2009
            };

            service.AddMovie(movie);

            // Borrow the movie once to make it unavailable
            service.BorrowMovie("M1", "UserX");

            // Manually add the user to the waiting queue (simulate they are waiting)
            service.AddToWaitingQueue("M1", "UserX");

            // Try borrowing again as the same user - triggers notification
            service.BorrowMovie("M1", "UserX");

            // Act
            var exported = service.ExportNotifications();
            var exportedAgain = service.ExportNotifications(); // Should be empty

            // Assert
            Assert.NotEmpty(exported);   
            Assert.Empty(exportedAgain); 
        }

        [Fact]
        public void SaveDataModel_ShouldStoreAndRetrieveAllProperties()
        {
            // Arrange
            var movieList = new List<Movie>
            {
                new Movie { ID = "M1", Title = "Interstellar", Director = "Nolan", Genre = "Sci-Fi", ReleaseYear = 2014 }
            };

            var notifications = new List<string> { "Test notification" };
            var activityLogs = new List<string> { "Log entry 1" };

            var saveData = new SaveData
            {
                Movies = movieList,
                Notifications = notifications,
                ActivityLogs = activityLogs
            };

            // Assert
            Assert.Single(saveData.Movies);
            Assert.Equal("Interstellar", saveData.Movies[0].Title);

            Assert.Single(saveData.Notifications);
            Assert.Equal("Test notification", saveData.Notifications[0]);

            Assert.Single(saveData.ActivityLogs);
            Assert.Equal("Log entry 1", saveData.ActivityLogs[0]);
        }

    }
}
