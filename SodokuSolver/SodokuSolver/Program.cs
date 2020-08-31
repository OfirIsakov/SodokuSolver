using System;

namespace SodokuSolver
{
    class Program
    {
        /// <summary>
        /// this variable is the board, must be a squeare the size of 3x * 3x
        /// </summary>
        static int[,] board;

        /// <summary>
        /// function checks if the given number in the given position is a valid spot
        /// </summary>
        static bool IsPossible((int x, int y) position, int number)
        {
            // if the position is taken its not possible to place a umber there
            if (board[position.x, position.y] != 0)
                return false;

            // check the row of the position
            for (int x = 0; x < board.GetLength(0); x++)
                if (board[x, position.y] == number)
                    return false;

            // check the column of the position
            for (int y = 0; y < board.GetLength(0); y++)
                if (board[position.x, y] == number)
                    return false;

            // check the square its in
            (int x, int y) currentSquare = (position.x / 3 * 3, position.y / 3 * 3);
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board[x + currentSquare.x, y + currentSquare.y] == number)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// function prints the given string, gets input from the user,
        /// clears the console messages and retunrs the user input
        /// </summary>
        static string WriteReadAndClear(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            Console.Clear();
            return input;
        }

        /// <summary>
        /// get new board input grom the user
        /// <para>
        /// gets a board size from the user and all the slots of the board as lines without space and with '_' as empty
        /// </para>
        /// </summary>
        static void BoardParser()
        {
            int size;

            while (true)
            {
                try
                {
                    size = int.Parse(WriteReadAndClear("What board size would you like?(If the input isnt devisible by 3 it will round down.)"));

                    // if the size is invalid go to the except section
                    if (size <= 0)
                    {
                        throw new Exception();
                    }

                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid line size!");
                }
            }
            board = new int[size, size];

            string[] userInput = new string[size];
            int number;
            for (int x = 0; x < size; x++)
            {
                // prints the user input until now
                for (int line = 0; line < x; line++)
                {
                    Console.WriteLine(userInput[line]);
                }

                // ask for new lines
                userInput[x] = WriteReadAndClear($"Please input the numbers of the {x} row without spaces(e.g. 5_4_87___)");

                if (userInput[x].Length == size)
                {
                    for (int y = 0; y < size; y++)
                    {
                        if (userInput[x][y] == '_')
                        {
                            continue;
                        }

                        number = int.Parse(userInput[x][y].ToString());
                        if (IsPossible((x, y), number))
                        {
                            board[x, y] = number;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid number - {number} at: {y}");

                            // reset the line
                            for (int i = 0; i < y; i++)
                            {
                                board[x, y] = 0;
                            }
                            break;
                        }

                    }
                }
                else
                {
                    --x;
                    Console.WriteLine($"Invalid input! Please input {size} numbers numbers without spaces!");
                }
            }

        }

        /// <summary>
        /// function solves the global board, prints the answer and checks if there are more answers
        /// </summary>
        static void Solve()
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == 0)
                    {
                        for (int number = 1; number <= 9; number++) // loop the digits 1-9 as 0 is not possible
                        {
                            if (IsPossible((x, y), number))
                            {
                                board[x, y] = number;
                                Solve();
                                board[x, y] = 0;
                            }
                        }
                        return;
                    }
                }
            }
            Console.WriteLine("------------------------------------------------");
            PrintBoard();
            Console.WriteLine("Press any key to search for more solutions...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// function loops the board and prints the content
        /// </summary>
        static void PrintBoard()
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    Console.Write($"{board[x, y]} ");
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                if (WriteReadAndClear("Welcome! Would you like to use the default board?[y/n]").Trim().ToLower().Equals("y"))
                {
                    board = new int[9, 9]{
                    { 5, 0, 4, 0, 8, 7, 0, 0, 0 },
                    { 0, 3, 0, 0, 0, 9, 0, 4, 0 },
                    { 8, 6, 0, 0, 0, 1, 0, 0, 9 },
                    { 7, 0, 3, 0, 0, 0, 0, 1, 4 },
                    { 0, 1, 2, 0, 0, 0, 6, 8, 0 },
                    { 6, 4, 0, 0, 0, 0, 7, 0, 3 },
                    { 1, 0, 0, 4, 0, 0, 0, 3, 6 },
                    { 0, 2, 0, 8, 0, 0, 0, 7, 0 },
                    { 0, 0, 0, 5, 1, 0, 9, 0, 8 }};
                }
                else
                {
                    BoardParser();
                }
                Solve();
                Console.WriteLine("No more solutions found!");
                if (!WriteReadAndClear("Solve another one?[y/n]").Trim().ToLower().Equals("y"))
                {
                    break;
                }
            }
        }
    }
}
