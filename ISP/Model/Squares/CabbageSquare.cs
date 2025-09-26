using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class LettuceSquare : Square
{
    public override Square Clone()
    {
        LettuceSquare clone= new LettuceSquare();
        clone.Player = Player;
        return clone;
    }

    public override string ToString()
    {
        string s = Player == null ? "." : $"{Player.Color}";
        return $"[L,{s}]";
    }

    public override Command GetCommand(Game state)
    {
        Command cmd = new LettuceSquareAction();
        cmd.State = state;
        return cmd;
    }
    private class LettuceSquareAction : Command
    {
        
        private int CarrotsRequired = 10;
        public Game State { get; set; }

        public void Execute(PlayerBase player)
        {
            player.Lettuce--;
            player.Carrots += CarrotsRequired * player.Rank;
            player.RequiredToMove = true;
        }
    }
}