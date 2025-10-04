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
        Controller c = new Controller(new Game(2));
        c.Run();

    }
    
    
}