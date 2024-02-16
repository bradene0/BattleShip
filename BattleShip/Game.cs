using System;
using System.Collections.Generic;

namespace BattleshipGame
{
    class Game
    {
        private Board playerBoard;
        private Board computerBoard;
        private Random random;

        private bool computerInTargetingMode;
        private List<Tuple<int, int>> computerTargetQueue;

        public Game()
        {
            random = new Random();
            computerInTargetingMode = false;
            computerTargetQueue = new List<Tuple<int, int>>();
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
                    Console.Clear();
                    Console.WriteLine("Player triggered a nuclear strike! All enemy ships are destroyed!");
                    computerBoard.Nuke(); // Destroy all enemy ships
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
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
            if (!computerInTargetingMode)
            {
                // Hunt mode
                int row = random.Next(0, 10);
                int col = random.Next(0, 10);
                while (playerBoard.GetGridValue(row, col) == 'X' || playerBoard.GetGridValue(row, col) == '.')
                {
                    row = random.Next(0, 10);
                    col = random.Next(0, 10);
                }
                playerBoard.Attack(row, col);
                if (playerBoard.GetGridValue(row, col) == 'O')
                {
                    computerInTargetingMode = true;
                    AddNeighborsToQueue(row, col);
                }
            }
            else
            {
                // Target mode
                if (computerTargetQueue.Count == 0)
                {
                    computerInTargetingMode = false;
                    return;
                }

                Tuple<int, int> target = computerTargetQueue[0];
                computerTargetQueue.RemoveAt(0);
                int row = target.Item1;
                int col = target.Item2;
                playerBoard.Attack(row, col);
                if (playerBoard.GetGridValue(row, col) == 'O')
                {
                    AddNeighborsToQueue(row, col);
                }
            }
        }


        private void AddNeighborsToQueue(int row, int col)
        {
            if (row > 0 && !playerBoard.HasBeenAttacked(row - 1, col))
                computerTargetQueue.Add(new Tuple<int, int>(row - 1, col));
            if (row < 9 && !playerBoard.HasBeenAttacked(row + 1, col))
                computerTargetQueue.Add(new Tuple<int, int>(row + 1, col));
            if (col > 0 && !playerBoard.HasBeenAttacked(row, col - 1))
                computerTargetQueue.Add(new Tuple<int, int>(row, col - 1));
            if (col < 9 && !playerBoard.HasBeenAttacked(row, col + 1))
                computerTargetQueue.Add(new Tuple<int, int>(row, col + 1));
        }

    }
}
