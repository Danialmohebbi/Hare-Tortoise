using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2.Rules;

public static class MoveGenerator
{
    public static List<Move> GetLegalMoves(Game game, PlayerBase player)
    {
        var moves = new List<Move>();

        for (int p = 0; p < player.Pieces.Count; p++)
        {
            var piece = player.Pieces[p];
            if (piece.Finished) continue;

            int baseSquare = piece.CurrentSquare;
            if (baseSquare >= 0 &&
                game.Board[baseSquare] is CarrotSquare)
            {
                moves.Add(new Move {
                    PieceIndex = p,
                    Delta = 0,
                    EatCarrots = true
                });
            }

            GenerateMovementMoves(game, player, piece, p, moves);
        }

        return moves;
    }
    private static void GenerateMovementMoves(
        Game game,
        PlayerBase player,
        Piece piece,
        int pieceIndex,
        List<Move> moves)
    {
        int baseSquare = piece.CurrentSquare;
        for (int delta = 1; ; delta++)
        {
            int target;
            if (baseSquare == -1)
                target = delta - 1;
            else
                target = baseSquare + delta;
            if (target < 0 || target >= game.Board.Count)
                break;
            int cost = (delta * (delta + 1)) / 2;
            if (player.Carrots < cost)
                break;
            if (game.Occupied(target))
                continue;

            Square targetSquare = game.Board[target];
            if (targetSquare is TortoiseSquare)
                continue;
            if (baseSquare >= 0 &&
                game.Board[baseSquare] is LettuceSquare &&
                !player.RequiredToMove)
            {
                continue;
            }
            moves.Add(new Move
            {
                PieceIndex = pieceIndex,
                Delta = delta,
                EatCarrots = false
            });
        }
        if (baseSquare >= 0)
        {
            int nearestTortoise = game.GetNearestTortoiseSquare(baseSquare);

            if (nearestTortoise != -1 && !game.Occupied(nearestTortoise))
            {
                int delta = nearestTortoise - baseSquare;

                moves.Add(new Move
                {
                    PieceIndex = pieceIndex,
                    Delta = delta,
                    EatCarrots = false
                });
            }
        }
    }

}
