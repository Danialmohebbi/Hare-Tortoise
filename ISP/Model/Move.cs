using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

public class Move
{
    public int PieceIndex { get; init; }
    public int Delta { get; init; }
    public bool EatCarrots { get; init; }

    public override string ToString()
    {
        if (EatCarrots)
            return $"Piece {PieceIndex}: Eat carrots (stay)";

        return $"Piece {PieceIndex}: Move {Delta} squares";
    }
    public string Describe(Game game, PlayerBase player)
    {
        var piece = player.Pieces[PieceIndex];
        int from = piece.CurrentSquare;
        int to = from + Delta;

        if (EatCarrots)
            return $"Piece {PieceIndex}: Eat carrots (stay on {from})";

        return $"Piece {PieceIndex}: {from} → {to} (cost {(Math.Abs(Delta)*(Math.Abs(Delta)+1))/2})";
    }

}

