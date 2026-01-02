using System.Collections.Generic;
using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2.Rules;
/// <summary>
/// Responsible for generating all LEGAL moves for a player
/// based on the current game state.
///
/// This class:
/// • Does NOT modify game state
/// • Does NOT execute moves
/// • Purely applies rule logic to enumerate possible actions
///
/// All move validation is centralized here to prevent
/// duplicated rule logic across Game, AI, and UI.
/// </summary>
public static class MoveGenerator
{
    /// <summary>
    /// Generates all legal moves for the given player.
    /// Includes:
    /// • Movement moves
    /// • Eating carrots (stay-in-place) moves
    /// 
    /// Finished pieces are ignored.
    /// </summary>
    /// <param name="game">Current game state</param>
    /// <param name="player">Player whose moves are being generated</param>
    /// <returns>List of legal moves</returns>
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
    /// <summary>
    /// Generates all legal movement-based moves for a specific piece.
    /// 
    /// Handles:
    /// • Forward movement with carrot cost
    /// • Start square movement
    /// • Tortoise backward jumps
    /// • Lettuce square movement restrictions
    /// • Occupied square blocking
    /// </summary>
    /// <param name="game">Current game state</param>
    /// <param name="player">Owning player</param>
    /// <param name="piece">Piece being evaluated</param>
    /// <param name="pieceIndex">Index of the piece</param>
    /// <param name="moves">List to append valid moves to</param>
    private static void GenerateMovementMoves(Game game, PlayerBase player, Piece piece, int pieceIndex, List<Move> moves) {
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
