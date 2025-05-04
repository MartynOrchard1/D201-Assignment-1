# D201 Assignment 1 - Movie Library Management System

## Overview
The Movie Library Management System is a C# WPF desktop application that allows users to manage a collection of movies. Core functionality includes adding, searching, sorting, borrowing, and returning movies. The application also manages waiting lists for borrowed movies using a custom queue data structure.

## Project Structure
- **Models/** – Movie model and SaveData contract
- **Services/** – Business logic including Borrow/Return, Search, Sort, Logging
- **DataStructures/** – Custom LinkedList, Queue, and HashTable implementations
- **Views/** – WPF Frontend (MainWindow.xaml and code-behind)
- **Helpers/** – File handling and validation utilities
- **MovieLibrary.Tests/** – xUnit test project covering backend functionality, users can also find the UI Tests inside of this folder `Ui Tests/`
- **images/** – Screenshots from manual UI tests
- **Test Notes.md** – Manual testing documentation this can be found inside of the Ui Tests/ folder
- **CHANGELOG.md** – Tracks changes between all versions | currently only: v1.0.0 and v1.1.0

## Features
- Add, edit, and delete movies
- Search movies by Title or ID
- Sort movies by:
  - Title (Bubble Sort)
  - Release Year (Merge Sort)
  - Movie ID (default sort)
  - Genre (alphabetically)
  - Availability (available movies listed first)
- Borrow movies with queue management if unavailable
- Return movies and automatically assign to the next user in the queue if applicable
- Save and load movie collections from JSON files
- UI feedback provided for all important user actions
- **Activity Log** – Track all borrow and return actions with timestamp
- **Notification Center UI** – Replaces MessageBoxes for persistent feedback
- **Clear Notifications Button** – Clears notification panel manually
- **Toggleable Notification Panel** – Show/hide notifications on demand
- **Sorting Toggle** – Repeated clicks on sort buttons reverse the order
- **SaveData** class used to persist full state (movies, logs, notifications)

## Key Features Explained

### Search and Sort
- **Search**: Users can search movies by title (case insensitive) or ID.
- **Sort**: Users can sort movies:
  - By Title using Bubble Sort.
  - By Release Year using Merge Sort.
  - By ID (default sort).
  - By Genre (A-Z)
  - By Availability (available movies shown first)
  - Sort order toggles between ascending and descending.

### Borrow and Return
- **Borrow**:
  - If a movie is available, it is immediately borrowed.
  - If not, the user is added to a waiting list.
- **Return**:
  - If a queue exists, the movie is assigned to the next user.
  - If no one is waiting, the movie becomes available again.
- **Activity logs**: Each action is logged with a timestamp.

### Waiting List Management
- A custom `Queue` data structure manages waiting users per movie.
- Users are notified via Notification Center when assigned a returned movie.
- Attempting to rejoin a queue logs a system message instead of adding duplicates.

### Validation
- Validates inputs for title, director, genre, and year.
- Prevents duplicate movie IDs.
- Handles empty or invalid entries gracefully.

## Testing Strategy

### Unit Testing
- Tests created using **xUnit** for backend logic.
- Coverage includes:
  - Add, delete, and replace movies
  - Search (title and ID)
  - Sort (title, year, ID, genre, availability)
  - Borrow and return flow
  - Queue logic and notification export
  - Activity log export/import
  - SaveData binding
  - Edge cases (duplicates, empty states)

### Manual UI Testing
- 12 manual test cases documented in `manual-tests.md`
- Each test includes:
  - Feature tested
  - Test steps
  - Expected vs actual outcome
  - Screenshots: before/after or expected/actual

➡ See [manual-tests.md](manual-tests.md) for full documentation. PATH: `MovieLibrary.Tests/Ui tests/Test Notes.md`  
➡ Screenshots available in the `/Images` directory within the Ui Tests folder. PATH to folder: `MovieLibrary.Tests/Ui tests/Images`

## Git Workflow
- Branch-based workflow used for all feature development:
  - Each feature was developed on a dedicated branch
  - Commits included descriptive messages
  - Branches merged into `main` after successful testing
- Issues and bug fixes tracked using GitHub Issues
- Pull requests linked to relevant issue numbers (e.g., `Fixes #3`)
- Releases tagged using GitHub (`v1.0.0`, `v1.1.0`)
- `CHANGELOG.md` added to document all version history

## Design Decisions
- **Custom Data Structures**:
  - `Queue`, `LinkedList`, and `HashTable` used instead of built-in collections for learning purposes.
- **Sorting Algorithms**:
  - Bubble Sort (simple and suitable for small datasets) used for sorting titles
  - Merge Sort (efficient) used for sorting by release year
- **UI**:
  - WPF was used to create a simple, clean desktop UI.
  - User feedback shown via Notification Center (persistent) and labels.
  - Notification panel can be toggled to improve focus.

## Setup Instructions

### Option 1: Using Visual Studio
1. Clone or download the repository.
2. Open `MovieLibraryFunctionalCode.sln` in Visual Studio 2022+.
3. Set `MovieLibrary` as the startup project.
4. Build and run the solution.
5. The app window should launch.

### Option 2: Using Terminal / CLI
1. Clone or download the repository.
2. Open a terminal and `cd` into the `movielibrary` folder.
3. Run `dotnet build`.
4. Run `dotnet run`.
5. The application will start.

## Future Improvements
- Add user authentication and multi-user profiles
- Store movies in a database (e.g., SQLite or PostgreSQL) instead of JSON
- Enhance UI layout and theme for better user experience
- Add advanced filters (e.g., by genre, year range, director)
- Allow dynamic usernames instead of hardcoded "User1"

## Author
Developed by **Martyn Orchard**  
Bachelor of ICT, Major in Software Engineering  
UCOL Palmerston North
