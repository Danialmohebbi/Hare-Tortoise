namespace ConsoleApp2.Player;
/// <summary>
/// Represents an executable game command triggered by landing on a board square.
/// </summary>
public interface Command
{
    public Game State { get; set; }
    public void Execute(PlayerBase player,Piece piece);
}