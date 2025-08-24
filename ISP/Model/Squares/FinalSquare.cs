using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class FinalSquare : Square
{
    public override Square Clone()
    {
        FinalSquare clone= new FinalSquare();
        clone.Player = Player;
        return clone;
    }
    public override string ToString()
    {
        return $"[{Player} Final Square ]";
    }

    public override Command GetCommand(Game state)
    {
        return null;
    }
}