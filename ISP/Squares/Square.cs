using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public abstract class Square
{
    public PlayerBase? Player { get; set; }
    public new abstract string ToString();
    public abstract Command GetCommand(GameState gameState);
}