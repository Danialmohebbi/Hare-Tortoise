using System.Collections;
using ConsoleApp2.Squares;

namespace ConsoleApp2.Player;

public class PlayerBase
{
    public PlayerColor Color { get; init; }
    public bool RequiredToMove { get; set; }
    public int Carrots { get; set; }
    public int Rank { get; set; } = 0;
    public int Lettuce { get; set; }
    public bool SkipRound { get; set; }
    public bool TakeCarrots { get; set; }
    public int CarrotsUsedLastTurn { get; set; }
    public bool Won { get; set; }

    public virtual Move move(Game game)
    {
        throw new NotImplementedException();
    }

    public int CurrentSquare { get; set; } = 0;
    
    private Command command = null;

    public void ExecuteCommand()
    {
        if (command == null)
            return;
        command.Execute(this);
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

    public PlayerBase Clone()
    {
        PlayerBase clone = new PlayerBase(Color);
        clone.RequiredToMove = RequiredToMove;
        clone.Carrots = Carrots;
        clone.Rank = Rank;
        clone.Lettuce = Lettuce;
        clone.TakeCarrots = TakeCarrots;
        clone.SkipRound = SkipRound;
        clone.Won = Won;
        clone.CurrentSquare = CurrentSquare;
        clone.command = command;
        clone.CarrotsUsedLastTurn = CarrotsUsedLastTurn;
        return clone;
    }
    
}