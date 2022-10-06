```
Author:     Nik McKnight
Partner:    None
Date:       16-March-2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  Nik-McKnight
Repo:       https://github.com/Nik-McKnight/Spreadsheet-Project.git
Commit #:   26
Solution:   Spreadsheet
Copyright:  CS 3500 and Nik McKnight - This work may not be copied for use in Academic Coursework.
```
# To Run

Open in Visual Studio, set startup project to 'GUI', and run.

# Overview of the Spreadsheet functionality

Spreadsheet cells will accept a number, a string, or a formula. The formula may contain case-insensitive cell names in the range A1-Z99.

# Time Expenditures:

    1. Assignment One:     Predicted Hours:          10        Actual Hours:       9
    2. Assignment Two:     Predicted Hours:          8         Actual Hours:       10
    3. Assignment Three:   Predicted Hours:          9         Actual Hours:       11
    4. Assignment Four:    Predicted Hours:          8         Actual Hours:       10
    5. Assignment Five:    Predicted Hours:          10        Actual Hours:       11
    6. Assignment Six:     Predicted Hours:          12        Actual Hours:       11

# Examples of Good Software Practice (GSP)

Spreadsheet -       In my cell class, I have one constructor that handles all three types. 
                    The cell class also has a method to get its dependees easily and getters/setters.
                    I also added the updateDependencies method so I could use it in all three SetCellContents methods.

Formula -           ParseInput() is used many times and contains a good amount of code that I did not have to repeat.
                    Same with Evaluate() and Equals().

DependencyGraph -   Having the hashtables to track dependents and dependees makes looking those up much more efficient.
                    All of the methods in this class helped me avoid repeating code and making mistakes.
