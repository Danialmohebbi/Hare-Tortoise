using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

public class Move
{
    public int PieceIndex { get; set; }
    public int SqureTargetIndex { get; set; }
    public bool EatCarrots { get; set; }
}
