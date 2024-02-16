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
                    Console.WriteLine("Are you sure you want to trigger a nuclear strike? This action may have unforeseen consequences! (Y/N)");
                    string confirm = Console.ReadLine().ToUpper();
                    if (confirm == "Y")
                    {
                        // Determine the outcome based on probabilities
                        int outcome = random.Next(1, 101); // Generate a random number between 1 and 100
                        if (outcome <= 22)
                        {
                            // Both players' ships are destroyed
                            Console.Clear();
                            Console.WriteLine("Nuclear strike initiated! Both players' ships have been destroyed!");
                            playerBoard.Nuke();
                            computerBoard.Nuke();
                            Console.WriteLine("Press Enter to continue...");
                            Console.ReadLine();
                        }
                        else if (outcome <= 27)
                        {
                            // Player's ships are destroyed
                            Console.Clear();
                            Console.WriteLine("Nuclear strike initiated! Your ships have been destroyed! Computer wins!");
                            playerBoard.Nuke();
                            Console.WriteLine("Press Enter to continue...");
                            Console.ReadLine();
                        }
                        else
                        {
                            // Only opponent's ships are destroyed
                            Console.Clear();
                            Console.WriteLine("Nuclear strike initiated! All enemy ships have been destroyed!");
                            computerBoard.Nuke();
                            Console.WriteLine("Press Enter to continue...");
                            Console.ReadLine();
                        }
                    }
                    return;

                case "SURRENDER":
                    Console.WriteLine("Are you sure you want to surrender? The computer will be declared the winner! (Y/N)");
                    string confirmSurrender = Console.ReadLine().ToUpper();
                    if (confirmSurrender == "Y")
                    {
                        // Computer wins
                        Console.Clear();
                        Console.WriteLine("You surrendered! The computer is declared the winner!");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        // Player decides not to surrender
                        Console.WriteLine("Surrender cancelled. Keep fighting!");
                    }
                    return;

                case "MISSILE":
                    // Activate Hyper-Sonic-Missile cheat
                    Console.WriteLine("Hyper-Sonic-Missile cheat activated! Select 3 coordinates to attack on the computer's board.");
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine($"Enter coordinate {i + 1}:");
                        string missileInput = Console.ReadLine().ToUpper();
                        if (missileInput.Length == 2 && char.IsLetter(missileInput[0]) && char.IsDigit(missileInput[1]))
                        {
                            int row = missileInput[0] - 'A';
                            int col = missileInput[1] - '1';
                            computerBoard.Attack(row, col); // Attack the computer's board
                        }
                        else
                        {
                            Console.WriteLine("Invalid coordinate format. Please enter coordinates in the format 'A1'.");
                            i--; // Retry input
                        }
                    }
                    return;

                case "SABOTAGE":
                    Console.WriteLine("Sabotage activated! Select a coordinate on the opponent's board to destroy one of their ships. If it exists!");
                    Console.WriteLine("Enter coordinate:");
                    string sabotageInput = Console.ReadLine().ToUpper();
                    if (sabotageInput.Length == 2 && char.IsLetter(sabotageInput[0]) && char.IsDigit(sabotageInput[1]))
                    {
                        int row = sabotageInput[0] - 'A';
                        int col = sabotageInput[1] - '1';
                        if (computerBoard.GetGridValue(row, col) == 'O')
                        {
                            // Ship found at the selected coordinate
                            Console.WriteLine($"Sabotage successful! Revealing the entire ship.");
                            // Find the ship that contains the selected coordinate
                            int shipRow = row, shipCol = col;
                            while (shipRow >= 0 && computerBoard.GetGridValue(shipRow, col) == 'O')
                            {
                                shipRow--;
                            }
                            shipRow++;
                            while (shipRow < 10 && computerBoard.GetGridValue(shipRow, col) == 'O')
                            {
                                shipRow++;
                            }
                            shipRow--;
                            // Reveal the entire ship
                            for (int r = shipRow; r >= 0 && computerBoard.GetGridValue(r, col) == 'O'; r--)
                            {
                                computerBoard.Attack(r, col);
                            }
                            for (int r = shipRow + 1; r < 10 && computerBoard.GetGridValue(r, col) == 'O'; r++)
                            {
                                computerBoard.Attack(r, col);
                            }
                        }
                        else
                        {
                            // No ship found at the selected coordinate
                            Console.WriteLine("No ship found at the selected coordinate.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid coordinate format. Please enter coordinates in the format 'A1'.");
                    }
                    return;

                case "COIN":
                    Console.WriteLine("Flipping a coin...");
                    int coinFlip = random.Next(2); // Generate a random number (0 or 1) to represent heads or tails
                    if (coinFlip == 0)
                    {
                        // Player wins
                        Console.WriteLine("It's heads! You win!");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                    }
                    else
                    {
                        // Computer wins
                        Console.WriteLine("It's tails! Computer wins!");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                    }
                    return;


                case "HELP":
                    Console.WriteLine("Welcome to Battleship!");
                    Console.WriteLine("To play the game, you will take turns with the computer to attack each other's ships on a 10x10 grid.");
                    Console.WriteLine("Your ships are hidden from the opponent, and your objective is to sink all of the opponent's ships before they sink yours.");
                    Console.WriteLine("During your turn, you can either:");
                    Console.WriteLine("- Enter coordinates to attack the opponent's grid (e.g., A1)");
                    Console.WriteLine("- Use cheat commands and turn the tides in your favor with the cheats command");
                    Console.WriteLine("The game will display the results of each attack, indicating whether you've hit or missed the opponent's ships.");
                    Console.WriteLine("Be strategic and try to outsmart the computer to win the game!");
                    Console.WriteLine("Try the ADMIN Command for advanced Options");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    return;



                case "CHEATS":
                    Console.WriteLine("Available cheat commands:");
                    Console.WriteLine("- HELP: Display instructions on how to play the game.");
                    Console.WriteLine("- REVEAL: Reveal the opponent's ships.");
                    Console.WriteLine("- NUKE: Trigger a nuclear strike (use with caution!).");
                    Console.WriteLine("- MISSILE: Activate the Hyper-Sonic-Missile cheat to attack 3 coordinates at once.");
                    Console.WriteLine("- SABOTAGE: Reveal the location of one of the opponent's ships.");
                    Console.WriteLine("- COIN: Flip a coin to determine the winner.");
                    Console.WriteLine("- SURRENDER: Surrender the game and declare the computer the winner.");
                    Console.WriteLine("- ADMIN: Advanced Options and Cheat list.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    return;

                case "ADMIN":
                    Console.WriteLine("Welcome to the admin panel!");
                    Console.WriteLine("You can start a new game or view all available cheats here.");
                    Console.WriteLine("1. Start a new game");
                    Console.WriteLine("2. View all cheats");
                    Console.WriteLine("Enter your choice (1 or 2):");
                    string adminChoice = Console.ReadLine();
                    switch (adminChoice)
                    {
                        case "1":
                            // Start a new game
                            Console.WriteLine("Starting a new game...");
                            Initialize(); // Call the Initialize method to start a new game
                            Play(); // Start the new game
                            break;
                        case "2":
                            // View all cheats
                            Console.WriteLine("Available cheat commands:");
                            Console.WriteLine("- HELP: Display instructions on how to play the game.");
                            Console.WriteLine("- REVEAL: Reveal the opponent's ships.");
                            Console.WriteLine("- NUKE: Trigger a nuclear strike (use with caution!).");
                            Console.WriteLine("- MISSILE: Activate the Hyper-Sonic-Missile cheat to attack 3 coordinates at once.");
                            Console.WriteLine("- SABOTAGE: Reveal the location of one of the opponent's ships.");
                            Console.WriteLine("- COIN: Flip a coin to determine the winner.");
                            Console.WriteLine("- SURRENDER: Surrender the game and declare the computer the winner.");
                            Console.WriteLine("- CHEATS: View all available cheat commands.");
                            Console.WriteLine("Press Enter to continue...");
                            Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                            break;
                    }
                    return;



                default:
                    if ((input.Length == 2 || input.Length == 3) && char.IsLetter(input[0]) && char.IsDigit(input[input.Length - 1]))
                    {
                        int row = input[0] - 'A';
                        int col;
                        if (input.Length == 3 && input[1] == '1' && input[2] == '0')
                        {
                            col = 9; // Convert '10' to column index 9 (for column 10)
                        }
                        else
                        {
                            col = input[input.Length - 1] - '1'; // Convert digit to column index (0-indexed)
                        }

                        if (row >= 0 && row < 10 && col >= 0 && col < 10)
                        {
                            computerBoard.Attack(row, col); // Apply the attack on the computer's board
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Coordinates must be within the range A1 to J10.");
                            Console.ReadLine();
                            PlayerTurn(); // Retry player's turn
                        }
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
