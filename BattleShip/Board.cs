using System;
//todo add minigame
namespace BattleshipGame
{
    public class Board
    {
        private char[,] grid;
        private bool[,] hitGrid;
        private int[] shipSizes = { 5, 4, 3, 3, 2 }; // Sizes of ships

        public enum AttackResult
        {
            Miss,
            Hit
        }

        public AttackResult LastAttackResult { get; private set; }
        public int LastAttackRow { get; private set; }
        public int LastAttackCol { get; private set; }

        public Board()
        {
            grid = new char[10, 10];
            hitGrid = new bool[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grid[i, j] = '~'; // Initialize grid with water
                }
            }
        }

        public void PlaceShips()
        {
            Random random = new Random();
            foreach (int size in shipSizes)
            {
                bool placed = false;
                while (!placed)
                {
                    int row = random.Next(0, 10);
                    int col = random.Next(0, 10);
                    bool horizontal = random.Next(2) == 0;

                    if (CanPlaceShip(row, col, size, horizontal))
                    {
                        PlaceShip(row, col, size, horizontal);
                        placed = true;
                    }
                }
            }
        }

        private bool CanPlaceShip(int row, int col, int size, bool horizontal)
        {
            if (horizontal)
            {
                if (col + size > 10)
                    return false;
                for (int i = col; i < col + size; i++)
                {
                    if (grid[row, i] != '~')
                        return false;
                }
            }
            else
            {
                if (row + size > 10)
                    return false;
                for (int i = row; i < row + size; i++)
                {
                    if (grid[i, col] != '~')
                        return false;
                }
            }
            return true;
        }

        private void PlaceShip(int row, int col, int size, bool horizontal)
        {
            if (horizontal)
            {
                for (int i = col; i < col + size; i++)
                {
                    grid[row, i] = 'O'; // Mark ship on grid
                }
            }
            else
            {
                for (int i = row; i < row + size; i++)
                {
                    grid[i, col] = 'O'; // Mark ship on grid
                }
            }
        }

        public void Attack(int row, int col)
        {
            if (grid[row, col] == 'O')
            {
                grid[row, col] = 'X'; // Mark hit on grid
                LastAttackResult = AttackResult.Hit;
            }
            else
            {
                grid[row, col] = 'm'; // Mark miss on grid
                LastAttackResult = AttackResult.Miss;
            }
            hitGrid[row, col] = true; // Mark this cell as hit
            LastAttackRow = row;
            LastAttackCol = col;
        }

        public bool AllShipsSunk()
        {
            foreach (char cell in grid)
            {
                if (cell == 'O')
                    return false; // If any ship remains, return false
            }
            return true; // All ships sunk
        }

        public void Display(bool showShips)
        {
            Console.WriteLine("   1 2 3 4 5 6 7 8 9 10");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((char)('A' + i) + "  ");
                for (int j = 0; j < 10; j++)
                {
                    if (grid[i, j] == 'O' && !showShips)
                        Console.Write("~ "); // Don't reveal ships if not needed
                    else
                        Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public char GetGridValue(int row, int col)
        {
            return grid[row, col];
        }

        public bool HasBeenAttacked(int row, int col)
        {
            return hitGrid[row, col];
        }

        public void RevealShips()
        {
            // Display the board with ships revealed
            Console.Clear();
            Console.WriteLine("Board (with ships revealed):");
            Display(true); // Display the board with ships revealed
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public void Nuke()
        {
            // Destroy all enemy ships
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (grid[i, j] == 'O')
                        grid[i, j] = 'X'; // Mark ship as destroyed
                }
            }
            Console.WriteLine("All enemy ships have been destroyed!");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public void CheatReveal()
        {
            // Display the board with ships revealed
            Console.Clear();
            Console.WriteLine("Board (with ships revealed):");
            Display(true); // Display the board with ships revealed
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
