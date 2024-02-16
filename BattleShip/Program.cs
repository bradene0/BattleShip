using System;

namespace BattleshipGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Battleship!");

            // Choose difficulty level
            DifficultyLevel difficulty = ChooseDifficulty();

            // Start the game
            Game game = new Game(difficulty);
            game.Initialize();
            game.Play();

            Console.WriteLine("Thank you for playing Battleship!");
        }

        static DifficultyLevel ChooseDifficulty()
        {
            Console.WriteLine("Choose difficulty level:");
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");

            while (true)
            {
                Console.Write("Enter your choice (1-3): ");
                string input = Console.ReadLine();

                if (input == "1")
                    return DifficultyLevel.Easy;
                else if (input == "2")
                    return DifficultyLevel.Medium;
                else if (input == "3")
                    return DifficultyLevel.Hard;
                else
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
            }
        }
    }
}
