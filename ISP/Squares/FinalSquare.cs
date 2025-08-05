using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class FinalSquare : Square
{
    public override string ToString()
    {
        return $"[{Player} Final Square ]";
    }

    public override Command GetCommand(GameState gameState)
    {
        return null;
    }
}