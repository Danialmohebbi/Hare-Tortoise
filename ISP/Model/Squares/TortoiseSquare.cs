using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;
/// <summary>
/// Represents a Tortoise square on the board.
///
/// TortoiseSquares reward backward movement.
/// When a piece moves backward onto a TortoiseSquare,
/// the number of squares moved backward is recorded and
/// converted into a carrot reward.
///
/// Forward movement onto a TortoiseSquare is illegal
/// and must be prevented by move generation rules.
/// </summary>
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