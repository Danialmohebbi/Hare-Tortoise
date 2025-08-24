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
        string s = Player != null ? "." : $"{Player.Color}";
        return $"[F,{s}]";    }

    public override Command GetCommand(Game state)
    {
        return new FinalSquareAction();
    }
    private class FinalSquareAction : Command
    {
        public Game State { get; set; }

        public void Execute(PlayerBase player) => player.Won = true;
    }
}