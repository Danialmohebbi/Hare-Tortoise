using System.Collections;
using ConsoleApp2.Squares;

namespace ConsoleApp2.Player;
/// <summary>
/// Represents a single playing piece belonging to a player.
/// </summary>
public class Piece
{
    public int PieceIndex { get; set; }
    public int CurrentSquare { get; set; } = -1;
    public bool Finished { get; set; } = false;

    public Piece(int index)
    {
        PieceIndex = index;
    }
}
/// <summary>
/// Base class representing a player in the game.
/// </summary>
public class PlayerBase
{
    public PlayerColor Color { get; init; }

    public bool IsAi { get; set; }
    public bool RequiredToMove { get; set; }
    public int Carrots { get; set; }
    public int Rank { get; set; } = 0;
    public int Lettuce { get; set; }
    public bool SkipRound { get; set; }
    public bool TakeCarrots { get; set; }
    public int CarrotsUsedLastTurn { get; set; }

    public virtual Move move(Game game)
    {
        throw new NotImplementedException();
    }

    public List<Piece> Pieces { get; } = new()
    {
        new Piece(0),
        new Piece(1)
    };

    public int FinishCount => Pieces.Count(p => p.Finished);
    
    private Command? command = null;

    public void ExecuteCommand(Piece piece)
    {
        if (command == null)
            return;
        command.Execute(this,piece);
        command = null;
    }

    public void SetCommand(Command action)
    {
        command = action;
    }
    public PlayerBase(PlayerColor color)
    {
        this.Color = color;
    }

    public override string ToString()
    {
        return $"{Color} Player" ;
    }


    
}