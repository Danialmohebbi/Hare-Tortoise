using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

public struct Move
{
    public int SqureTargetIndex { get; init; }
    public bool EatCarrots { get; init; }
    public GameState gameState { get; set; }

    public Move()
    {
        
    }

}