using System;
using System.Drawing;
using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;
class Program
{
    static void Main(string[] args)
    {
        int humanPlayers = AskHumanPlayers();

        Game game = new Game(humanPlayers);
        Controller controller = new Controller(game);

        controller.Run();
    }

    static int AskHumanPlayers()
    {
        Console.WriteLine("How many human players? (1–4)");

        while (true)
        {
            string input = Console.ReadLine();

            if (int.TryParse(input, out int count) &&
                count >= 1 && count <= 4)
            {
                return count;
            }

            Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
        }
    }
}
