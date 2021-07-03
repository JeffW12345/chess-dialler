# chess-dialler
How many combinations of numbers are possible when a chess piece moves around a phone dialler?

INTRODUCTION
------------

This program, which is written in C#, takes a keypad as shown below:

123
456
789
x0#

The program takes each number as a starting number in turn. From that number, it can move from button to button in accorance with the moves of a chess piece specified by the user. 

For example, if the starting button is 3 and the chess piece is a bishop, the program can move to 5 and to 7 (and diagonally from those numbers to other numbers). 

The purpose of the program is to find out how many possible combinations of seven digits there are. 

The following numbers cannot be starting numbers: 0 and 1.

The following symbols cannot be included in the combination: * or #

EXTENDING THE PROGRAM
---------------------

Changes to the number of digits, the keypad layout, the keys excluded from being starting keys and the keys that cannot be alighted on can be made via the Main method of
the Program class. 

The length of the combinations can be changed from 7 by changing the value of the 'howManyDigits' variable. However, some values of 'howManyDigits' will produce so many combinations that 
there will be insufficient memory space for the program to run to a successful conclusion.

A different keypad layout can be entered by amending the 2 dimensional 'keys' array in the Main method of the Program class. There cannot be any missing entries in the array - each row must
have the same number as elements as every other row, and the same with columns. If you want a key to be missing, enter it as "".

The keyCannotBeStartingKeyArray and keyCannotBeLandedOnArray arrays can be amended as required, including having no entries. However, they cannot not deleted, as they are referenced elsewhere in the program.

If you want to add a new piece, you need to create an enum for it in the enum class, as well as creating a dedicated class that extends abstract class Pieces, with the methods required by iPieces.


