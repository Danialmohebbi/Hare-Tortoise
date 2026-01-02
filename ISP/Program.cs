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
        int humanPlayers = View.View.AskHumanPlayers();
        Game game = new Game(humanPlayers);
        Controller controller = new Controller(game,new View.View{ game = game });
        controller.Run();
    }


}
