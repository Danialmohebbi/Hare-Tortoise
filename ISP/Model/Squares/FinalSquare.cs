using System;
using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;
/// <summary>
/// Represents a Final square on the board.
///
/// When a piece lands on a FinalSquare:
/// - The piece is marked as finished
/// - It is removed from the board
/// </summary>
public class FinalSquare : Square
{
    public override string ToString()
    {
        string s = OccupyingPiece == null ? "." : $"{Player.Color}";
        return $"[F,{s}]";
    }

    public override Command GetCommand(Game state)
    {
        return new FinalSquareAction { State = state };
    }

    private class FinalSquareAction : Command
    {
        public Game State { get; set; }

        public void Execute(PlayerBase player, Piece piece)
        {
            if (piece.Finished)
                return;

            Console.WriteLine($"{player.Color} finishes a piece!");

            // Remove piece from board
            if (piece.CurrentSquare >= 0)
                State.Board[piece.CurrentSquare].OccupyingPiece = null;

            piece.CurrentSquare = -1;
            piece.Finished = true;
        }
    }
}