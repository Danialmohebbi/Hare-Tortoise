using ConsoleApp2.Extensions;
using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

class Program
{


    static void Main(string[] args)
    {


        GameState gameState = new GameState(1);
        // gameState.Players[0].Rank = 1;
        // Console.WriteLine("Current Carrots: " + gameState.Players[0].Carrots);
        // gameState.ApplyMove(new Move { SqureTargetIndex = 4  });
        // Console.WriteLine("Current Carrots: " + gameState.Players[0].Carrots);
        // gameState.ApplyMove(new Move { SqureTargetIndex = 4 , EatCarrots = true});
        // Console.WriteLine("Current Carrots: " + gameState.Players[0].Carrots);
        PlayerBase player = gameState.Players[gameState.TurnIndex];
        // gameState.ApplyMove(new Move { SqureTargetIndex = 54 });
        player.CurrentSquare = 12;
        gameState.ApplyMove(new Move { SqureTargetIndex = player.CurrentSquare + 1 });
        Console.WriteLine("Current Carrots: " + gameState.Players[0].Carrots);

        
        // while (!gameState.GameOver())
        // {
        foreach (var cell in gameState.Board)
            Console.WriteLine(cell.ToString());
        //     gameState.Players[0].Move(gameState.Board);
        //     Console.WriteLine("----------------------------");
        // }

    }
    
    
}