using ConsoleApp2.Rules;

namespace ConsoleApp2.Player;

public class RandomPlayer : PlayerBase
{
    Random r = new Random();

    public RandomPlayer(PlayerColor color) : base(color) { }
    
    public override Move move(Game game)
    {
        var moves = MoveGenerator.GetLegalMoves(game, this);
        return moves.Count == 0
            ? null
            : moves[Random.Shared.Next(moves.Count)];
    }

}