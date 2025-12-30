using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class TortoiseSquare : Square
{
    public int moves { get; set; }
    public override string ToString()
    {
        string s = Player == null ? "." : $"{Player.Color}";
        return $"[T,{s}]";    }

    public override Command GetCommand(Game state)
    {
        Command cmd = new TortoiseSquareAction(moves);
        moves = 0;
        cmd.State = state;
        return cmd;    }

    public void SetMoves(int moves) => this.moves = moves;
    
    private class TortoiseSquareAction : Command
    {
        private int CarrotsRequired = 10;
        private int moves = 0;

        public TortoiseSquareAction(int moves)
        {
            this.moves = moves;
        }

        public Game State { get; set; }

        public void Execute(PlayerBase player, Piece piece)
        {
            player.Carrots += moves * CarrotsRequired;
        }
    }
}