using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;
/// <summary>
/// Controller is responsible for:
///  - Running the main game loop
///  - Coordinating Game and View
///  - Determining when the game ends
/// </summary>
public class Controller
{
    /// <summary>
    /// Core game state and rules.
    /// </summary>
    private readonly Game game;
    
    /// <summary>
    /// View responsible for all console input/output.
    /// </summary>
    private readonly View.View view;

    /// <summary>
    /// Creates a controller with explicit dependencies.
    /// This allows for change of view and game via dependency injection.
    /// </summary>
    /// <param name="game">Game logic and state</param>
    /// <param name="view">rendering and input</param>
    public Controller(Game game, View.View view)
    {
        this.game = game;
        this.view = view;
}
    /// <summary>
    /// Runs the main game loop.
    /// The loop continues until a player has
    /// finished both of their pieces.
    /// </summary>
    public void Run()
    {
        while (game.Winner() == null)
        {
            Console.Clear();

            view.DrawPrefix();
            view.Draw();
            Console.WriteLine();
            view.DrawPostfix();

            game.PlayTurn();
        }
        
        view.GameOver();
    }
}