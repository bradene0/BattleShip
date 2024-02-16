using System;
using System.Collections.Generic;

namespace BattleshipGame
{
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    public class Game
    {
        private Board playerBoard;
        private Board computerBoard;
        private Random random;

        private DifficultyLevel difficultyLevel;

        public Game(DifficultyLevel difficulty)
        {
            random = new Random();
            difficultyLevel = difficulty;
        }

        public void Initialize()
        {
            playerBoard = new Board();
            computerBoard = new Board();

            // Place ships on boards
            playerBoard.PlaceShips();
            computerBoard.PlaceShips();
        }

        public void Play()
        {
            while (!playerBoard.AllShipsSunk() && !computerBoard.AllShipsSunk())
            {
                Console.Clear();

                // Player's turn
                Console.WriteLine("Player's turn:");
                Console.WriteLine("Computer's Board:");
                computerBoard.Display(false); // Display computer's board without showing ships
                Console.WriteLine("Player's Board:");
                playerBoard.Display(true); // Display player's board showing ships

                PlayerTurn();

                if (computerBoard.AllShipsSunk())
                    break;

                Console.Clear();

                // Computer's turn
                Console.WriteLine("Computer's turn:");
                Console.WriteLine("Player's Board:");
                playerBoard.Display(true); // Display player's board showing ships
                Console.WriteLine("Computer's Board:");
                computerBoard.Display(false); // Display computer's board without showing ships

                ComputerTurn();

                if (playerBoard.AllShipsSunk())
                    break;
            }

            // Game over...
            Console.Clear();
            Console.WriteLine("Game Over!");
            if (playerBoard.AllShipsSunk())
            {
                Console.WriteLine("Computer wins!");
            }
            else
            {
                Console.WriteLine("Player wins!");
            }
        }

        private void PlayerTurn()
        {
            Console.WriteLine("Enter coordinates to attack (e.g., A1), or type a cheat command:");

            string input = Console.ReadLine().ToUpper();

            switch (input)
            {
                case "REVEAL":
                    Console.Clear();
                    Console.WriteLine("Computer's Board (with ships revealed):");
                    computerBoard.Display(true); // Display computer's board with ships revealed
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    return;

                case "NUKE":
                    Console.WriteLine("Are you sure you want to trigger a nuclear strike? This action will destroy all enemy ships! (Y/N)");
                    string confirm = Console.ReadLine().ToUpper();
                    if (confirm == "Y")
                    {
                        Console.Clear();
                        Console.WriteLine("Player triggered a nuclear strike! All enemy ships are destroyed!");
                        computerBoard.Nuke(); // Destroy all enemy ships
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                    }
                    return;

                default:
                    if (input.Length == 2 && char.IsLetter(input[0]) && char.IsDigit(input[1]))
                    {
                        int row = input[0] - 'A';
                        int col = input[1] - '1';
                        computerBoard.Attack(row, col); // Apply the attack on the computer's board
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter coordinates in the format 'A1' or type a cheat command.");
                        Console.ReadLine();
                        PlayerTurn(); // Retry player's turn
                    }
                    break;
            }
        }


        private void ComputerTurn()
        {
            switch (difficultyLevel)
            {
                case DifficultyLevel.Easy:
                    EasyComputerTurn();
                    break;
                case DifficultyLevel.Medium:
                    MediumComputerTurn();
                    break;
                case DifficultyLevel.Hard:
                    HardComputerTurn();
                    break;
            }
        }

        private void EasyComputerTurn()
        {
            int row = random.Next(0, 10);
            int col = random.Next(0, 10);
            playerBoard.Attack(row, col);
        }

        private void MediumComputerTurn()
        {
            // Prioritize attacking adjacent cells after hitting a ship
            if (playerBoard.LastAttackResult == Board.AttackResult.Hit)
            {
                List<Tuple<int, int>> adjacentCells = GetAdjacentCells(playerBoard.LastAttackRow, playerBoard.LastAttackCol);
                foreach (var cell in adjacentCells)
                {
                    int row = cell.Item1;
                    int col = cell.Item2;
                    if (!playerBoard.HasBeenAttacked(row, col))
                    {
                        playerBoard.Attack(row, col);
                        return;
                    }
                }
            }

            // If no adjacent cells are available, make a random attack
            int randRow, randCol;
            do
            {
                randRow = random.Next(0, 10);
                randCol = random.Next(0, 10);
            } while (playerBoard.HasBeenAttacked(randRow, randCol));

            playerBoard.Attack(randRow, randCol);
        }

        private void HardComputerTurn()
        {
            // Implement advanced AI behavior for Hard mode
            // For demonstration purposes, use a basic strategy of attacking every other cell
            for (int row = 0; row < 10; row += 2)
            {
                for (int col = 0; col < 10; col += 2)
                {
                    if (!playerBoard.HasBeenAttacked(row, col))
                    {
                        playerBoard.Attack(row, col);
                        return;
                    }
                }
            }

            // If all cells have been attacked, make a random attack
            int randRow, randCol;
            do
            {
                randRow = random.Next(0, 10);
                randCol = random.Next(0, 10);
            } while (playerBoard.HasBeenAttacked(randRow, randCol));

            playerBoard.Attack(randRow, randCol);
        }

        private List<Tuple<int, int>> GetAdjacentCells(int row, int col)
        {
            var adjacentCells = new List<Tuple<int, int>>();
            if (row > 0)
                adjacentCells.Add(new Tuple<int, int>(row - 1, col));
            if (row < 9)
                adjacentCells.Add(new Tuple<int, int>(row + 1, col));
            if (col > 0)
                adjacentCells.Add(new Tuple<int, int>(row, col - 1));
            if (col < 9)
                adjacentCells.Add(new Tuple<int, int>(row, col + 1));
            return adjacentCells;
        }
    }
}
