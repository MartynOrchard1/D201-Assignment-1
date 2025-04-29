# D201 Assignment 1 - Movie Library Management System

## Overview
The Movie Library Management System is a C# WPF desktop application that allows users to manage a collection of movies. Core functionality includes adding, searching, sorting, borrowing, and returning movies. The application also manages waiting lists for borrowed movies using a custom queue data structure.

## Project Structure
- Models/ - Movie model.
- Services/ - Business logic including Borrow/Return, Search, Sort.
- DataStructures/ - Custom LinkedList, Queue, and HashTable implementations.
- Views/ - WPF Frontend (MainWindow).
- Helpers/ - File handling and validation utilities.
- Tests/ - xUnit test project covering backend functionality.

## Features
- Add, edit, and delete movies.
- Search movies by Title or ID.
- Sort movies by:
  - Title (Bubble Sort)
  - Release Year (Merge Sort)
  - Movie ID 
- Borrow movies with queue management if unavailable.
- Return movies and automatically assign to the next user in the queue if applicable.
- Save and load movie collections from JSON files.
- UI feedback provided for all important user actions.


## Key Features Explained
### Search and Sort
- **Search**: Users can search movies by title (case insensitive) or ID.
- **Sort**: Users can sort movies:
  - By Title using Bubble Sort.
  - By Release Year using Merge Sort.
- Sort order can toggle between ascending and descending when clicked again.

### Borrow and Return
- **Borrow**:
  - If a movie is available, it is immediately borrowed.
  - If not available, the user is added to a waiting list (queue).
- **Return**:
  - If a waiting list exists, the movie is assigned to the next user automatically.
  - If no one is waiting, the movie becomes available again.

### Waiting List Management
- Custom `Queue` data structure manages users waiting for each movie.
- Users are notified through UI prompts when their turn comes.

### Validation
- Input validation for movie details (title, director, genre, year).
- Prevents duplicate movie IDs.
- Handles invalid data gracefully.

## Testing Strategy
- Unit tests created using xUnit.
- Tests cover:
  - Adding movies.
  - Searching and sorting.
  - Borrowing and returning movies.
  - Queue handling.
  - Boundary and edge cases such as:
    - Duplicate movie IDs.
    - Borrowing with no available movies.
    - Empty movie collection searches.


## Key Features Explained
### Search and Sort
- **Search**: Users can search movies by title (case insensitive) or ID.
- **Sort**: Users can sort movies:
  - By Title using Bubble Sort.
  - By Release Year using Merge Sort.
- Sort order can toggle between ascending and descending when clicked again.

### Borrow and Return
- **Borrow**:
  - If a movie is available, it is immediately borrowed.
  - If not available, the user is added to a waiting list (queue).
- **Return**:
  - If a waiting list exists, the movie is assigned to the next user automatically.
  - If no one is waiting, the movie becomes available again.

### Waiting List Management
- Custom `Queue` data structure manages users waiting for each movie.
- Users are notified through UI prompts when their turn comes.

### Validation
- Input validation for movie details (title, director, genre, year).
- Prevents duplicate movie IDs.
- Handles invalid data gracefully.

## Testing Strategy
- Unit tests created using xUnit.
- Tests cover:
  - Adding movies.
  - Searching and sorting.
  - Borrowing and returning movies.
  - Queue handling.
  - Boundary and edge cases such as:
    - Duplicate movie IDs.
    - Borrowing with no available movies.
    - Empty movie collection searches.

## Git Workflow
- Branch-based development approach:
  - Each major feature was developed on a separate branch.
  - Frequent commits with descriptive messages.
  - Features were merged back into the main branch after review and testing.

## Design Decisions
- **Data Structures**: Custom-built `HashTable`, `LinkedList`, and `Queue` classes were used instead of built-in collections for educational purposes.
- **Sorting Algorithms**: 
  - **Bubble Sort** was chosen for simplicity in sorting titles alphabetically.
  - **Merge Sort** was selected for efficiency in sorting by release year.
- **UI Design**: WPF used to build a simple, clear, and responsive interface.

## Setup Instructions
### Option 1:
1. Clone or download the repository.
2. Open `MovieLibraryFunctionalCode.sln` in Visual Studio 2022 or later.
3. Set `MovieLibrary` as the startup project.
4. Build and run the solution.
5. The app should now open up in a new window

### Option 2:
1. Clone or download the repository.
2. cd into the `movielibrary` folder
3. run `dotnet build`
4. run `dotnet run`
5. The app should now open up in a new window

## Future Improvements
- Add user accounts and authentication.
- Improve visual styling and theme for better UX.
- Store movie data in a database instead of JSON files.
- Allow dynamic user names instead of fixed "User1" borrowing.

## Author
- Developed by Martyn Orchard
- Bachelor of ICT, Major in Software Engineering, UCOL Palmerston North
