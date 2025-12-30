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
            view.DrawPrefix();
            game.PlayTurn();
            view.Draw();Console.WriteLine();
            view.DrawPostfix();
        }
        
        GameOver();
    }
    
    public void GameOver()
    {
        Console.WriteLine();
        Console.WriteLine("🏁 GAME OVER 🏁");
        Console.WriteLine();
        var winners = game.Players
            .Where(p => p.FinishCount == 2)
            .OrderBy(p => p.Rank)
            .ToList();
        var remaining = game.Players
            .Where(p => p.FinishCount < 2)
            .OrderBy(p => p.Rank)
            .ToList();

        Console.WriteLine("🏆 Podium:");
        foreach (var player in winners)
        {
            Console.WriteLine(
                $"{player.Rank}. {player.Color} " +
                $"(Finished pieces: {player.FinishCount}, Carrots: {player.Carrots})");
        }

        if (remaining.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("❌ Did Not Finish:");
            foreach (var player in remaining)
            {
                Console.WriteLine(
                    $"{player.Rank}. {player.Color} " +
                    $"(Finished pieces: {player.FinishCount}, Carrots: {player.Carrots})");
            }
        }
    }

    
}