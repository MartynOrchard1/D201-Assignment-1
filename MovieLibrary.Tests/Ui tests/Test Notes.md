# Manual UI Testing Report

## Each Test will have it's own unique ID for the user to search through tests easier

### Test Case TC01 – Add Movie

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter movie details: Title = "Test Case 1", Director = "First Test", Year = 2025
3. Click "Add" button

**Expected Result:**  
Movie appears in the movie list below the input fields

**Actual Result:**  
Movie is added successfully and listed at the bottom of thte grid

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/TC01.3.png" alt="Before Adding" height="">

**After**
<img src="../Ui tests/Images/TC01.1.png" alt ="After Adding - MessageBox" height="">
<img src="../Ui tests/Images/TC01.2.png" alt="After Adding" height="">
---

### Test Case TC02 – Search Movie by Title

**Feature:** Search
**Steps:**
1. Launch application
2. Click `Load Movies`
3. Load the `Star Wars.json` file provided
3. Select the `Search by title` in the option box
4. In the input field next to the option box type in 'star wars 1'
5. Click `Search`

**Expected Result:**  
Movie matching 'Star Wars 1' should appear as the only movie in the listbox

**Actual Result:**  
Star Wars 1 is the only movie shown in the listbox

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/TC02.1.png" alt="Before Searching" height="">

**After**
<img src="../Ui tests/Images/TC02.2.png" alt="After Searching" height="">

---

### Test Case TC03 – Borrow Movie

**Feature:** Borrow Movie
**Steps:**
1. Launch application
2. Select/Click on a Movie from the list
3. Click `Borrow` Button

**Expected Result:**  
Movie is marked unavailable and is assigned to the user who borrowed it

**Actual Result:**  
Movie marked unavailable and assigned to user

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/TC03.1.png" alt="Before Borrowing movie" height="">

**After**
<img src="../Ui tests/Images/TC03.2.png" alt="After Borrowing Movie" height="">
<img src="../Ui tests/Images/TC03.3.png" alt="After Borrowing Movie" height="">

---
### Test Case TC04 – Return Movie with Waiting Queue

**Feature:** Return Movie with a queue
**Steps:**
1. Launch application
2. Borrow a movie with User1
3. Attempt to borrow with User 2 --> Added to queue
4. Return Movie

**Expected Result:**  
Movie is reassigned to User2

**Actual Result:**  
User2 receives the movie

**Pass/Fail:** ✅ Pass

#### Screenshots:

<img src="../Ui tests/Images/TC04.1.png" alt="Added to Queue" height="">
<img src="../Ui tests/Images/TC04.2.png" alt="Is assigned to user2" height="">
<img src="../Ui tests/Images/TC04.3.png" alt="Available = False " height="">

---
### Test Case TC05 – Return Movie without Waiting Queue

**Feature:** Return Movie (no queue)
**Steps:**
1. Launch application
2. Borrow Movie
3. Return Movie

**Expected Result:**  
Movie becomes available again

**Actual Result:**  
Movie marked available

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/TC05.1.png" alt="Not Available" height="">

**After**
<img src="../Ui tests/Images/TC05.2.png" alt="Messagebox" height="">
<img src="../Ui tests/Images/TC05.3.png" alt="Movie Returned" height="">

---
### Test Case TC06 – Sort by Title

**Feature:** Sort 
**Steps:**
1. Launch application
2. Click `Sort by Title`

**Expected Result:**  
Movies sorted A → Z on the first click. On the second click movies sorted Z → A.

**Actual Result:**  
Sorted correctly

**Pass/Fail:** ✅ Pass

#### Screenshots:

**1st Click (a-z)**
<img src="../Ui tests/Images/TC06.1.png" alt="" height="">

**2nd Click (z-a)**
<img src="../Ui tests/Images/TC06.2.png" alt="" height="">

---
### Test Case TC07 – Sort by Release Year

**Feature:** Sort by Year
**Steps:**
1. Launch application
2. Click `Sort by Year`

**Expected Result:**  
Clicking the button the first time should sort the movies by oldest → newest. Clicking the button the second time should reverse the sort newest → oldest

**Actual Result:**  
Sorted correctly on both clicks

**Pass/Fail:** ✅ Pass

#### Screenshots:

**1st Click Oldest-Newest**
<img src="../Ui tests/Images/TC07.1.png" alt="1st Click" height="">

**2nd Click Newest-Oldest**
<img src="../Ui tests/Images/TC07.2.png" alt="" height="">

---
### Test Case 8 – Delete Movie

**Feature:** Delete a movie
**Steps:**
1. Launch application
2. Select a movie in the list
3. Click `Delete`

**Expected Result:**  
Movie removed from list

**Actual Result:**  
Movie no longer visible

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/TC08.1.png" alt="" height="">

**After**
<img src="../Ui tests/Images/TC08.2.png" alt="" height="">
<img src="../Ui tests/Images/TC08.3.png" alt="" height="">
<img src="../Ui tests/Images/TC08.4.png" alt="" height="">

---

### Test Case TC09 – Add Movie Error Checking (Input Checks)

**Feature:** Error Checking - Crash Prevention
**Steps:**
1. Launch application
2. Enter nothing into the movie details: Title = "", Director = "", Year = ""
3. Click "Add" button

**Expected Result:**  
Program should interrupt and tell the user to enter some valid input into the input field (Title, Director and year etc.)

**Actual Result:**  
Error Checking(s) is successfull and prevents the program from crashing, tells the user to try again/correctly enter the inputs defined above.

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/TC09.1.png" alt="" height="">

**After**
<img src="../Ui tests/Images/TC09.2.png" alt="" height="">

---
### Test Case TC10 – Add Movie Error Checking (Valid Year)

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter a ridiculous year no e.g. "2026 or 1800"
3. Click "Add" button

**Expected Result:**  
MessageBox should appear stating 'Release Year Can't be in the future' OR 'Please enter a realistic year BETWEEN 1888 and 2026'

**Actual Result:**  
Message Box appeared on both Inputs

**Pass/Fail:** ✅ Pass

#### Screenshots:

<img src="../Ui tests/Images/TC10.1.png" alt="" height="">
<img src="../Ui tests/Images/TC10.2.png" alt="" height="">

---
### Test Case TC11 – Delete Movie Error Checks

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. For ease of testing load the `Star Wars.json` File provided in the `images` folder inside of the `UI tests` folder. 
`MovieLibrary.tests/UI tests/Star Wars.json`
3. Select a movie from the list
4. Click `Delete`

**Expected Result:**  
Application should pop up with a MessaageBox: 'Are you sure you want to delete `{Movie Name}`'

**Actual Result:**  
MessageBox Appeared

**Pass/Fail:** ✅ Pass

#### Screenshots:

<img src="../Ui tests/Images/TC11.1.png" alt="" height="">

---

### Test Case TC12 – Sort by Movie ID

**Feature:** Sort by ID
**Steps:**
1. Launch application
2. For ease of testing load the `Star Wars.json` File provided in the `images` folder inside of the `UI tests` folder. 
`MovieLibrary.tests/UI tests/Star Wars.json`
3. Click `Sort by ID`

**Expected Result:**  
On first click app should sort movies by id in order of M002 --> M006. On Second click the app should sort movies in the reverse order

**Actual Result:**  
Sort Successful

**Pass/Fail:** ✅ Pass

#### Screenshots:

**1st Click**
<img src="../Ui tests/Images/TC12.1.png" alt="" height="">

**2nd Click**
<img src="../Ui tests/Images/TC12.2.png" alt="" height="">

--- 
## End of UI Tests