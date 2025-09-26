using System;
using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

class Program
{

    static void PrintHelp(int x, int y, Game g)
    {
        for (int i = 0; i <= 19; i++)
            Console.Write(" ");
        Console.Write(g.Board[x].ToString());
        for (int i = 0; i <= 6; i++)
            Console.Write("\t");
        Console.WriteLine(g.Board[y].ToString());
        
    }

    static void Main(string[] args)
    {
        Game game = new Game(4);
        game.Move(new Move { SqureTargetIndex = 10 });
        game.Move(new Move { SqureTargetIndex = 2 });
        View.View v = new View.View(){g = game };
        v.Draw();


    }
    
    
}