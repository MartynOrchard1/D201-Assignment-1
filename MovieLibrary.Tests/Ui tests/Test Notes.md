# Manual UI Testing Report

## Each Test will have it's own unique ID for the user to search through tests easier

### Test Case TC01 – Add Movie

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter movie details: Title = "Inception", Director = "Nolan", Year = 2010
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

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter movie details: Title = "Inception", Director = "Nolan", Year = 2010
3. Click "Add" button

**Expected Result:**  
Movie appears in the movie list below the input fields

**Actual Result:**  
Movie is added successfully and listed at the bottom of thte grid

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/" alt="" height="">

**After**
<img src="../Ui tests/Images/" alt="" height="">

---
### Test Case TC06 – Add Movie

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter movie details: Title = "Inception", Director = "Nolan", Year = 2010
3. Click "Add" button

**Expected Result:**  
Movie appears in the movie list below the input fields

**Actual Result:**  
Movie is added successfully and listed at the bottom of thte grid

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/" alt="" height="">

**After**
<img src="../Ui tests/Images/" alt="" height="">

---
### Test Case TC07 – Add Movie

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter movie details: Title = "Inception", Director = "Nolan", Year = 2010
3. Click "Add" button

**Expected Result:**  
Movie appears in the movie list below the input fields

**Actual Result:**  
Movie is added successfully and listed at the bottom of thte grid

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/" alt="" height="">

**After**
<img src="../Ui tests/Images/" alt="" height="">

---
### Test Case 8 – Add Movie

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter movie details: Title = "Inception", Director = "Nolan", Year = 2010
3. Click "Add" button

**Expected Result:**  
Movie appears in the movie list below the input fields

**Actual Result:**  
Movie is added successfully and listed at the bottom of thte grid

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/" alt="" height="">

**After**
<img src="../Ui tests/Images/" alt="" height="">

---
### Test Case TC09 – Add Movie

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter movie details: Title = "Inception", Director = "Nolan", Year = 2010
3. Click "Add" button

**Expected Result:**  
Movie appears in the movie list below the input fields

**Actual Result:**  
Movie is added successfully and listed at the bottom of thte grid

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/" alt="" height="">

**After**
<img src="../Ui tests/Images/" alt="" height="">

---
### Test Case TC10 – Add Movie

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter movie details: Title = "Inception", Director = "Nolan", Year = 2010
3. Click "Add" button

**Expected Result:**  
Movie appears in the movie list below the input fields

**Actual Result:**  
Movie is added successfully and listed at the bottom of thte grid

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/" alt="" height="">

**After**
<img src="../Ui tests/Images/" alt="" height="">

---
### Test Case TC11 – Add Movie

**Feature:** Add Movie  
**Steps:**
1. Launch application
2. Enter movie details: Title = "Inception", Director = "Nolan", Year = 2010
3. Click "Add" button

**Expected Result:**  
Movie appears in the movie list below the input fields

**Actual Result:**  
Movie is added successfully and listed at the bottom of thte grid

**Pass/Fail:** ✅ Pass

#### Screenshots:

**Before**
<img src="../Ui tests/Images/" alt="" height="">

**After**
<img src="../Ui tests/Images/" alt="" height="">

---