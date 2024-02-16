using System;

namespace BattleshipGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the game
            Game game = new Game();
            game.Initialize();

            // Play the game
            game.Play();

            Console.ReadLine(); // Prevent console from closing immediately
        }
    }
}
