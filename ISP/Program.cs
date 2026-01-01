using System;

namespace ConsoleApp2;
/// <summary>
/// Application entry point.
/// Responsible only for:
///  - Asking initial configuration questions
///  - Creating core game objects
///  - Starting the controller loop
/// 
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        // Ask user how many human players will participate (1–4).
        // The remaining players will be AI.
        int humanPlayers = View.View.AskHumanPlayers();
        // Create the game state using the selected player configuration
        Game game = new Game(humanPlayers);
        // Create the controller which drives the game loop
        Controller controller = new Controller(game,new View.View{ game = game });
        // Start the game
        controller.Run();
    }


}
