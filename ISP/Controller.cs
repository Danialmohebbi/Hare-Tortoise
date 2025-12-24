using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

public class Controller
{
    Game game;
    View.View view;

    public Controller(Game game)
    {
        this.game = game;
        view = new View.View{ g = game };
}
    public void Run()
    {
        while (game.Winner() == null)
        {
            Console.WriteLine($"Red Player: {game.Players[0].CurrentSquare}");
            view.DrawPrefix();
            game.PlayTurn();
            view.Draw();Console.WriteLine();
            view.DrawPostfix();
        }
        
        GameOver();
    }
    
    public void GameOver()
    {
        int? winnerIndex = game.Winner();
        if (winnerIndex == null)
            return;
        PlayerBase winner = game.Players[winnerIndex.Value];
        Console.WriteLine();
        Console.WriteLine("🏁 GAME OVER 🏁");
        Console.WriteLine($"Winner: {winner.Color}");
        Console.WriteLine();

        game.UpdateRank();

        foreach (var player in game.Players.OrderBy(p => p.Rank))
        {
            Console.WriteLine(
                $"{player.Rank}. {player.Color} " +
                $"(Square: {player.CurrentSquare}, Carrots: {player.Carrots})");
        }
    }
    
}