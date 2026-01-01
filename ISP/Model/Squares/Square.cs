using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;
/// <summary>
/// Abstract base class representing a single square on the game board.
/// </summary>
public abstract class Square
{
    public Piece OccupyingPiece { get; set; }
    public PlayerBase? Player { get; set; }
    public new abstract string ToString();
    public abstract Command GetCommand(Game state);
}