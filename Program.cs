using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDial
{
    public class Keypad
    {
        public long Width { get; }
        public long Height { get; }
        public List<string> KeyCannotBeLandedOnList{ get; }
        public List<string> KeyCannotBeStartingKeyList { get; }
        public string[,] Keys { get; }

        public Keypad(string[,] keys, string[] unusableKeys, string[] nonBeginningKeys)
        {
            this.Keys = keys;
            this.Height = keys.GetLongLength(0);
            this.Width = keys.GetLongLength(1);
            KeyCannotBeLandedOnList= new List<string>(unusableKeys);
            KeyCannotBeStartingKeyList = new List<string>(nonBeginningKeys);
        }

        public void DrawKeypad()
        {
            Console.WriteLine("This is the keyboard layout:");
            for (long yAxis = 0; yAxis < Height; yAxis++)
            {
                Console.WriteLine();
                for (long xAxis = 0; xAxis < Width; xAxis++)
                {
                    Console.Write(Keys[yAxis, xAxis] + " ");
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    public abstract class Pieces
    {
        // Each long [] entry in the list returned by GetNextMoves consists of two elements - the y axis co-ordinate [0] and the x axis co-ordinate [1] relating to an entry in the Keys array of 
        // Keypad objects.
        public abstract List<long[]> GetNextMoves(Keypad keypad, long yAxis, long xAxis);
        public abstract Dictionary<long[], List<long[]>> FindNextMoveDict { get; }

        // Returns a dictionary whose key is the co-ordinates of an element in the Keypad object's Keys array and whose value is a list of available next moves for those co-ordinates (also
        // expressed as co-ordinates). 
        public Dictionary<long[], List<long[]>> GetDict (Keypad keypad)
        {
            Dictionary<long[], List<long[]>> coOrdinatesToNextMoveDict = new Dictionary<long[], List<long[]>>();
            for (long yAxis = 0; yAxis < keypad.Height; yAxis++)
            {
                for (long xAxis = 0; xAxis < keypad.Width; xAxis++)
                {
                    long[] coOrdinates = { yAxis, xAxis };
                    coOrdinatesToNextMoveDict[coOrdinates] = this.GetNextMoves(keypad, coOrdinates[0], coOrdinates[1]);
                }
            }
            return coOrdinatesToNextMoveDict;
        }
    }
    public class Bishop : Pieces
    { 
        public override Dictionary<long[], List<long[]>> FindNextMoveDict { get; }

        public Bishop(Keypad keypad)
        {
            FindNextMoveDict = GetDict(keypad);
        }

        public override List<long[]> GetNextMoves(Keypad keypad, long yAxis, long xAxis)
        {
            List<long[]> listOfMoves = new List<long[]>();

            // Upward left move
            long currX = xAxis;
            long currY = yAxis;

            while (currX - 1 >= 0 && currY - 1 >= 0)
            {
                currX--;
                currY--;
                if (!keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX]))
                {
                    listOfMoves.Add(new long[] { currY, currX });
                }
            }

            // Upward right move
            currX = xAxis;
            currY = yAxis;

            while (currX + 1 < keypad.Width && currY - 1 >= 0)
            {
                currX++;
                currY--;
                if (!keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX]))
                {
                    listOfMoves.Add(new long[] { currY, currX });
                }
            }

            // Downward left move
            currX = xAxis;
            currY = yAxis;

            while (currX - 1 >= 0 && currY + 1 < keypad.Height)
            {
                currX--;
                currY++;
                if (!keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX]))
                {
                    listOfMoves.Add(new long[] { currY, currX });
                }
            }

            // Downward right move
            currX = xAxis;
            currY = yAxis;

            while (currX + 1 < keypad.Width && currY + 1 < keypad.Height)
            {
                currX++;
                currY++;
                if (!keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX]))
                {
                    listOfMoves.Add(new long[] { currY, currX });
                }
            }
            return listOfMoves;
        }

    }

    public class King : Pieces
    {
        public override Dictionary<long[], List<long[]>> FindNextMoveDict { get; }

        public King(Keypad keypad)
        {
            FindNextMoveDict = GetDict(keypad);
        }

        public override List<long[]> GetNextMoves(Keypad keypad, long yAxis, long xAxis)
        {
            List<long[]> listOfMoves = new List<long[]>();
            long keypadHeight = keypad.Height;
            long keypadWidth = keypad.Width;
            long currX = xAxis;
            long currY = yAxis;

            // Move one upwards (non-diagonal).
            if (currY - 1 >= 0 && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY - 1, currX]))
            {
                listOfMoves.Add(new long[] { currY - 1, currX });
            }

            // Move one downwards (non-diagonal).
            if (currY + 1 < keypadHeight && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY + 1, currX]))
            {
                listOfMoves.Add(new long[] { currY + 1, currX });
            }

            // Move one to right (non-diagonal).
            if (currX + 1 < keypadWidth && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX + 1]))
            {
                listOfMoves.Add(new long[] { currY, currX + 1 });
            }

            // Move one to left (non-diagonal).
            if (currX - 1 >= 0 && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX - 1]))
            {
                listOfMoves.Add(new long[] { currY, currX - 1 });
            }

            // Upward diagonal right move
            if (currX + 1 < keypadWidth && currY - 1 >= 0 && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY - 1, currX + 1]))
            {
                listOfMoves.Add(new long[] { currY - 1, currX + 1 });
            }

            // Upward diagonal left move
            if (currX - 1 >= 0 && currY - 1 >= 0 && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY - 1, currX - 1]))
            {
                listOfMoves.Add(new long[] { currY - 1, currX - 1 });
            }

            // Downward diagonal left move
            if (currX - 1 >= 0 && currY + 1 < keypadHeight && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY + 1, currX - 1]))
            {
                listOfMoves.Add(new long[] { currY + 1, currX - 1 });
            }

            // Downward diagonal right move
            if (currX + 1 < keypadWidth && currY + 1 < keypadHeight && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY + 1, currX + 1]))
            {
                listOfMoves.Add(new long[] { currY + 1, currX + 1 });
            }
            return listOfMoves;
        }
    }

    public class Knight : Pieces
    {
        public override Dictionary<long[], List<long[]>> FindNextMoveDict { get; }

        public Knight(Keypad keypad)
        {
            FindNextMoveDict = GetDict(keypad);
        }
        public override List<long[]> GetNextMoves(Keypad keypad, long yAxis, long xAxis)
        {
            List<long[]> listOfMoves = new List<long[]>();

            long keypadHeight = keypad.Height;
            long keypadWidth = keypad.Width;
            long currX = xAxis;
            long currY = yAxis;

            // Two up one across left
            if (currX - 1 >= 0 && currY - 2 >= 0 && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY - 2, currX - 1]))
            {
                listOfMoves.Add(new long[] { currY - 2, currX - 1 });
            }

            // Two up one across right
            if (currX + 1 < keypadWidth && currY - 2 >= 0 && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY - 2, currX + 1]))
            {
                listOfMoves.Add(new long[] { currY - 2, currX + 1 });
            }

            // Two down one across left
            if (currX - 1 >= 0 && currY + 2 < keypadHeight && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY + 2, currX - 1]))
            {
                listOfMoves.Add(new long[] { currY + 2, currX - 1 });
            }

            // Two down one across right
            if (currX + 1 < keypadWidth && currY + 2 < keypadHeight && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY + 2, currX + 1]))
            {
                listOfMoves.Add(new long[] { currY + 2, currX + 1 });
            }

            // One down and two across right
            if (currX + 2 < keypadWidth && currY + 1 < keypadHeight && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY + 1, currX + 2]))
            {
                listOfMoves.Add(new long[] { currY + 1, currX + 2 });
            }

            // One down and two across left
            if (currX - 2 >= 0 && currY + 1 < keypadHeight && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY + 1, currX - 2]))
            {
                listOfMoves.Add(new long[] { currY + 1, currX - 2 });
            }

            // One up and two across right
            if (currX + 2 < keypadWidth && currY - 1 >= 0 && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY - 1, currX + 2]))
            {
                listOfMoves.Add(new long[] { currY - 1, currX + 2 });
            }

            // One up and two across left
            if (currX - 2 >= 0 && currY - 1 >= 0 && !keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY - 1, currX - 2]))
            {
                listOfMoves.Add(new long[] { currY - 1, currX - 2 });
            }

            return listOfMoves;
        }
    }

    public class Pawn : Pieces
    {
        public override Dictionary<long[], List<long[]>> FindNextMoveDict { get; }

        public Pawn(Keypad keypad)
        {
            FindNextMoveDict = GetDict(keypad);
        }

        // The pawn can move fowards one square at a time ('forwards' from the persepctive of moving towards the top row, and then terminating). 
        public override List<long[]> GetNextMoves(Keypad keypad, long yAxis, long xAxis)
        {
            List<long[]> listOfMoves = new List<long[]>();

            long currX = xAxis;
            long currY = yAxis;

            if (currY - 1 >= 0)
            {
                listOfMoves.Add(new long[] { currY - 1, currX });
            }
            return listOfMoves;
        }
    }

    public class Queen : Pieces
    {
        public override Dictionary<long[], List<long[]>> FindNextMoveDict { get; }

        public Queen(Keypad keypad)
        {
            FindNextMoveDict = GetDict(keypad);
        }

        public override List<long[]> GetNextMoves(Keypad keypad, long yAxis, long xAxis)
        {
            List<long[]> listOfMoves = new List<long[]>();
            // Queen's moves are a combination of rook and bishop's moves
            Rook rook = new Rook(keypad);
            var rookMoves = rook.GetNextMoves(keypad, yAxis, xAxis);
            foreach (var rookMove in rookMoves)
            {
                listOfMoves.Add(rookMove);
            }
            Bishop bishop = new Bishop(keypad);
            var bishopMoves = bishop.GetNextMoves(keypad, yAxis, xAxis);
            foreach (var bishopMove in bishopMoves)
            {
                listOfMoves.Add(bishopMove);
            }
            return listOfMoves;
        }
    }

    public class Rook : Pieces
    {
        public override Dictionary<long[], List<long[]>> FindNextMoveDict { get; }

        public Rook(Keypad keypad)
        {
            FindNextMoveDict = GetDict(keypad);
        }

        public override List<long[]> GetNextMoves(Keypad keypad, long yAxis, long xAxis)
        {
            List<long[]> listOfMoves = new List<long[]>();
            long keypadHeight = keypad.Height;
            long keypadWidth = keypad.Width;

            // Horizontal left move
            long currX = xAxis;
            long currY = yAxis;

            while (currX - 1 >= 0)
            {
                currX--;
                if (!keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX]))
                {
                    listOfMoves.Add(new long[] { currY, currX });
                }
            }

            // Horizontal right move
            currX = xAxis;

            while (currX + 1 < keypadWidth)
            {
                currX++;
                if (!keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX]))
                {
                    listOfMoves.Add(new long[] { currY, currX });
                }
            }

            // Vertical up move
            currX = xAxis;

            while (currY - 1 >= 0)
            {
                currY--;
                if (!keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX]))
                {
                    listOfMoves.Add(new long[] { currY, currX });
                }
            }

            // Vertical down move
            currY = yAxis;

            while (currY + 1 < keypadHeight)
            {
                currY++;
                if (!keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[currY, currX]))
                {
                    listOfMoves.Add(new long[] { currY, currX });
                }
            }
            return listOfMoves;
        }
    }

    public enum AvailablePieces
    {
        Bishop,
        King,
        Knight,
        Pawn,
        Queen,
        Rook
    }

    public class Program
    {
        private static long pieceEnumVal;
        static void Main(string[] args)
        {
            // If a space in the keypad does not contain a digit, letter or symbol, you can use "" for that array entry. However, not adding anything for that 
            // element will cause a run-time error, as each row needs to contain the same number of elements as the other rows (ditto for columns). 
            string[,] keys = new string[,]
            {
                {"1", "2", "3"},
                {"4", "5", "6"},
                {"7", "8", "9" },
                {"x", "0", "#" },
            };
            string[] keyCannotBeLandedOnArray = new string[] { "x", "#", "" };
            string[] cannotBeStartingKeyArray = new string[] { "0", "1", "" };
            Keypad keypad = new Keypad(keys, keyCannotBeLandedOnArray, cannotBeStartingKeyArray);
            keypad.DrawKeypad();
            long howManyDigits = 7; // Number of digits to be included in each phone number.
            AskUserWhichPiece();
            AvailablePieces typeOfPiece = (AvailablePieces)pieceEnumVal;
            Pieces pieceObj = GetPiece(typeOfPiece, keypad);
            HashSet<string> combinations = GetCombinations(keypad, howManyDigits, pieceObj);
            long numCombinations = combinations.Count;
            Console.WriteLine("Number of valid " + howManyDigits + " digit numbers for the " + typeOfPiece.ToString().ToLower() + ": " + numCombinations);
            if (numCombinations > 0)
            {
                OfferToDisplayNums(combinations);
            }
        }

        private static void AskUserWhichPiece()
        {
            Console.WriteLine("Please select a piece type by entering a number:\n");
            long num = 0;
            foreach (AvailablePieces piece in (AvailablePieces[])Enum.GetValues(typeof(AvailablePieces)))
            {
                Console.WriteLine(++num + ". " + piece);
            }
            string userChoice = Console.ReadLine();
            if (!long.TryParse(userChoice, out long numChosen) || numChosen < 1 || numChosen > num)
            {
                Console.WriteLine("Invalid selection. Please try again.");
                AskUserWhichPiece();
            }
            else
            {
                pieceEnumVal = numChosen - 1;
            }
        }

        /*
         * Returns a set of strings consisting of the number combinations 'howManyDigits' in length for the concrete piece object that 'pieceObj' refers to.
         */
        public static HashSet<string> GetCombinations(Keypad keypad, long howManyDigits, Pieces pieceObj)
        {
            HashSet<string> allCombinations = new HashSet<string>();
            // Double 'for' loop to iterate through all possible starting co-ordinate combinations.
            for (long yAxis = 0; yAxis < keypad.Height; yAxis++)
            {
                for (long xAxis = 0; xAxis < keypad.Width; xAxis++)
                {
                    if (keypad.KeyCannotBeStartingKeyList.Contains(keypad.Keys[yAxis, xAxis]) || keypad.KeyCannotBeLandedOnList.Contains(keypad.Keys[yAxis, xAxis]))
                    {
                        continue;
                    }
                    // For each co-ordinate, creates a list of all possible co-ordinate combinations of 'howManyDigits' moves in length
                    long digitsCompleted = 0;
                    List<List<long[]>> combinationsSoFar = new List<List<long[]>>();
                    while (digitsCompleted < howManyDigits)
                    {
                        // If no digits have been processed yet, add the starting point xAxis, yAxis co-ordinate to the list of co-ordinates. 
                        if (digitsCompleted == 0)
                        {
                            List<long[]> longList = new List<long[]>();
                            long[] startingPoint = new long[] { yAxis, xAxis };
                            longList.Add(startingPoint);
                            combinationsSoFar.Add(longList);
                            digitsCompleted++;
                            continue;
                        }
                        /*
                         *  Array co-ordinates for the starting digit and for subsquent moves are stored in a list of co-ordinates. The co-ordinates are stored as long arrays in the form 
                         *  [y, x], where y and x are co-ordinates for the Keypad object's Keys array. The first co-ordinate array in the list is the starting key's co-ordinates, and the 
                         *  arrays for subsequent moves are added to the end of the list. 
                         *  
                         *  The co-ordinate arrays for the moves relating to the current starting co-ordinate and its moves are stored in 'combinationsSoFar'. 
                         *  
                         *  Where there is a valid next move for a list of co-ordinates in 'combinationsSoFar', the code below makes a clone of that list and appends the next move's 
                         *  co-ordinates to that clone. It puts this in a lists of lists, 'tempCombinationList'. It does this for every valid next move relating to the list of co-ordinates
                         *  in question, e.g. if there are 4 possible next moves, there will be four cloned lists, each with a different appendages, all of which are added to 
                         *  'tempCombinationList'.  
                         *  
                         *  Once this process has been completed for all the lists in 'combinationsSoFar', 'tempCombinationList' will contain a list of all the combinations of moves where a 
                         *  next move was possible (and nothing else). 'combinationsSoFar' is then set equal 'tempCombinationList', meaning it now contains that data and only that data. 
                         *  
                         *  'tempCombinationList' is garbage collected after its data is transferred to 'combinationsSoFar', and re-created during the next 'while' loop, meaning that it will 
                         *  start out in an empty state during the next loop. 
                         *  
                         *  Once the digit combinations for the current starting co-ordinate have been produced, each list of arrays in 'combinationsSoFar' are converted into a string and 
                         *  the string stored in the master list of strings that this method returns. The 'combinationsSoFar' list expires when the 'for' loop moves onto the next starting 
                         *  co-ordinate and is subsequently re-created (with the process repeating). 
                         *  
                         */

                        else
                        {
                            List<List<long[]>> tempCombinationList = new List<List<long[]>>();
                            foreach (List<long[]> list in combinationsSoFar)
                            {
                                long[] finalCoOrdsThisList = list[list.Count - 1];
                                List<long[]> nextMoveList = pieceObj.GetNextMoves(keypad, finalCoOrdsThisList[0], finalCoOrdsThisList[1]);
                                // If there are no valid next moves, the list is not copied to 'tempCombinationList'.
                                if (nextMoveList.Count > 0)
                                {
                                    foreach (long[] nextMove in nextMoveList)
                                    {
                                        List<long[]> listIncNextMove = GetClonePlusNextMove(list, nextMove);
                                        tempCombinationList.Add(listIncNextMove);
                                    }
                                }
                            }
                            digitsCompleted++;
                            combinationsSoFar = tempCombinationList; //Eliminates from further consideration lists of co-ordinates with (digitsCompleted - 1) entries
                        }
                    }
                    // Parses the lists of co-ordinates to create strings of digits. It adds them to the master set of strings (which the method returns). 
                    foreach (List<long[]> list in combinationsSoFar)
                    {
                        StringBuilder builder = new StringBuilder();
                        foreach (long[] entry in list)
                        {
                            builder.Append(keypad.Keys[entry[0], entry[1]]);
                        }
                        allCombinations.Add(builder.ToString());
                    }
                }
            }

            return allCombinations;
        }

        private static void OfferToDisplayNums(HashSet<string> allCombinations)
        {
            Console.WriteLine("Would you like to view the combinations?");
            Console.WriteLine("Press Y for 'Yes' or N for 'No'");
            string userChoice = Console.ReadLine();
            if (userChoice == "Y" || userChoice == "y")
            {
                Console.WriteLine("Here you go:");
                foreach (string str in allCombinations)
                {
                    Console.WriteLine(str);
                }
                return;
            }
            if (userChoice == "N" || userChoice == "n")
            {
                Console.WriteLine("No problem. Have a nice day. :)");
            }
            else
            {
                Console.WriteLine("That is not a valid option. Please try again.");
                OfferToDisplayNums(allCombinations);
            }
        }

        /* For each possible next move, the method below produces a copy of the existing list of moves with the new move appended to it.
         * The new list and the original list occupy different spaces in memory, so changes to the former do not affect the latter.
         */
        public static List<long[]> GetClonePlusNextMove(List<long[]> list, long[] nextMove)
        {
            List<long[]> tempList = new List<long[]>();
            foreach (long[] elem in list)
            {
                tempList.Add(elem);
            }
            tempList.Add(nextMove);
            return tempList;
        }

        private static Pieces GetPiece(AvailablePieces piece, Keypad keypad)
        {
            switch (piece)
            {
                case (AvailablePieces.Bishop):
                    {
                        return new Bishop(keypad);
                    }
                case (AvailablePieces.Knight):
                    {
                        return new Knight(keypad);
                    }
                case (AvailablePieces.Queen):
                    {
                        return new Queen(keypad);
                    }
                case (AvailablePieces.King):
                    {
                        return new King(keypad);
                    }
                case (AvailablePieces.Rook):
                    {
                        return new Rook(keypad);
                    }
                case (AvailablePieces.Pawn):
                    {
                        return new Pawn(keypad);
                    }
                default:
                    throw new NotImplementedException("Selection not present in GetPiece method - debugging required!");
            }
        }
    }
}
