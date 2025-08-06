using System;
using ConsoleApp2.Extensions;
using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

class Program
{


    static void Main(string[] args)
    {


        GameState gameState = new GameState(4);
        // gameState.Players[0].Rank = 1;
        // Console.WriteLine("Current Carrots: " + gameState.Players[0].Carrots);
        // gameState.ApplyMove(new Move { SqureTargetIndex = 4  });
        // Console.WriteLine("Current Carrots: " + gameState.Players[0].Carrots);
        // gameState.ApplyMove(new Move { SqureTargetIndex = 4 , EatCarrots = true});
        // Console.WriteLine("Current Carrots: " + gameState.Players[0].Carrots);
        // // gameState.ApplyMove(new Move { SqureTargetIndex = 54 });
        int[] playerSquares = new int[] { -1, -1, -1, -1 };
        for (int i = 0; i < 4; i++)
            if (playerSquares[i] != -1)
                gameState.Board[playerSquares[i]].Player = gameState.Players[i];
        
        GameStateExtension.UpdateRank(gameState);
        List<int> playersRank = new();
        foreach (PlayerBase p in gameState.Players)
            playersRank.Add(p.Rank);
        
        
        // while (!gameState.GameOver())
        // {
        foreach (var cell in playersRank)
            Console.WriteLine(cell.ToString());
        //     gameState.Players[0].Move(gameState.Board);
        //     Console.WriteLine("----------------------------");
        // }

    }
    
    
}