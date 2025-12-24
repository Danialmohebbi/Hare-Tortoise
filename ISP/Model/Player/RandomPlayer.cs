namespace ConsoleApp2.Player;

public class RandomPlayer : PlayerBase
{
    Random r = new Random();

    public RandomPlayer(PlayerColor color) : base(color) { }
    
    public override Move move(Game g)
    {
        List<Move> moves = g.possibleMoves();
        if (moves.Count == 0)
        {
            return null;
        }
        return moves[r.Next(moves.Count)];
    }
}