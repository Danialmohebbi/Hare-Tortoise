using System;
using System.Linq;
using ConsoleApp2.Squares;

namespace ConsoleApp2.View;
/// <summary>
/// Responsible for all console input and output.
/// 
/// The View class:
/// - Renders the game board in a fixed ASCII layout
/// - Displays player status information (HUD)
/// - Handles user input for initial configuration
/// - Displays end-of-game results
/// 
/// </summary>
public class View
{
    /// <summary>
    /// Reference to the active game instance.
    /// This is required so the View can read board and player state.
    /// </summary>
    public required Game game { get; init; }
    /// <summary>
    /// Draws the game board to the console.
    /// 
    /// The board layout is hard-coded.
    /// Each row is manually aligned using spaces to create the correct shape.
    /// </summary>
    public void Draw()
    { 
        for (int i = 2; i <= 8; i++)
            Console.Write(game.Board[i].ToString());
        for (int _ = 0; _ <= 7; _++)
            Console.Write("     ");
        for (int i = 58; i <= 62; i++)
            Console.Write(game.Board[i].ToString());
        Console.WriteLine();
        //SecondRow
        Console.Write(game.Board[1].ToString());
        for (int i = 0; i <= 24; i++)
            Console.Write(" ");
        Console.Write(game.Board[9].ToString());
        for (int _ = 0; _ <= 7; _++)
            Console.Write("     ");
        Console.WriteLine(game.Board[57].ToString());
        //Third Row
        
        Console.Write(game.Board[0].ToString());
        for (int i = 0; i <= 19; i++)
            Console.Write(" ");
        Console.Write(game.Board[11].ToString());
        Console.Write(game.Board[10].ToString());
        for (int _ = 0; _ <= 7; _++)
            Console.Write("     ");
        Console.Write(game.Board[56].ToString());
        Console.WriteLine(game.Board[55].ToString());
        //Fourth Row
        for (int i = 0; i <= 19; i++)
            Console.Write(" ");
        Console.Write(game.Board[13].ToString());
        Console.Write(game.Board[12].ToString());
        for (int _ = 0; _ <= 9; _++)
            Console.Write("     ");
        Console.Write(game.Board[54].ToString());
        Console.WriteLine(game.Board[53].ToString());
        //Fifth Row
        for (int i = 0; i <= 14; i++)
            Console.Write(" ");
        Console.Write(game.Board[15].ToString());
        Console.Write(game.Board[14].ToString());
        for (int _ = 0; _ <= 11; _++)
            Console.Write("     ");
        Console.Write(game.Board[52].ToString());
        Console.WriteLine(game.Board[51].ToString());

        //Sixth Row
        for (int i = 0; i <= 9; i++)
            Console.Write(" ");
        Console.Write(game.Board[17].ToString());
        Console.Write(game.Board[16].ToString());
        for (int _ = 0; _ <= 13; _++)
            Console.Write("     ");
        Console.Write(game.Board[49].ToString());
        Console.WriteLine(game.Board[48].ToString());
        //Seventh Row
        Console.Write(game.Board[20].ToString());
        Console.Write(game.Board[19].ToString());
        Console.Write(game.Board[18].ToString());
        for (int _ = 0; _ <= 15; _++)
            Console.Write("     ");
        Console.Write(game.Board[47].ToString());
        Console.Write(game.Board[46].ToString());
        Console.WriteLine(game.Board[45].ToString());
        
        //Eigth Row
        Console.Write(game.Board[21].ToString());
        for (int _ = 0; _ <= 19; _++)
            Console.Write("     ");
        Console.WriteLine(game.Board[44].ToString());
        
        //Ninth Row
        for (int i =22; i <= 43; i++)
            Console.Write(game.Board[i].ToString());
    }
    /// <summary>
    /// Displays the current player information before a turn begins.
    /// Acts as a simple HUD.
    /// </summary>
    public void DrawPrefix()
    {
        Console.WriteLine($"Current Player: {game.Players[game.TurnIndex]} - Carrots: " +
                          $"{game.Players[game.TurnIndex].Carrots}" + 
                          $" - Lettuce: {game.Players[game.TurnIndex].Lettuce}");
    }
    /// <summary>
    /// Draws a visual separator after the board.
    /// Improves readability between turns.
    /// </summary>
    public void DrawPostfix()
    {
        Console.WriteLine("---------------------------------------------------");
    }
    /// <summary>
    /// Asks the user how many human players will participate.
    /// Valid input range: 1–4.
    /// Remaining players will be controlled by AI.
    /// </summary>
    /// <returns>The number of human players</returns>
    public static int AskHumanPlayers()
    {
        Console.WriteLine("How many human players? (1–4)");
        while (true)
        {
            string input = Console.ReadLine();

            if (int.TryParse(input, out int count) &&
                count >= 1 && count <= 4) {
                return count;
            }

            Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
        }
    }
    /// <summary>
    /// Displays the final results of the game.
    /// Players who finished both pieces are shown as winners,
    /// remaining players are listed separately.
    /// </summary>
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

        Console.WriteLine("🏆 Winner:");
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