using System;
using System.Drawing;
using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;
class Program
{
    static void Main(string[] args)
    {
        int humanPlayers = View.View.AskHumanPlayers();

        Game game = new Game(humanPlayers);
        Controller controller = new Controller(game);

        controller.Run();
    }


}
