using System.Collections;
using ConsoleApp2.Squares;

namespace ConsoleApp2.Player;

public class PlayerBase
{
    public PlayerColor Color { get; init; }
    public bool RequiredToMove { get; set; }
    public int Carrots { get; set; }
    public int Rank { get; set; }
    public int Lettuce { get; set; }
    public bool SkipRound { get; set; }
    public bool TakeCarrots { get; set; }
    public int CarrotsUsedLastTurn { get; set; }
    public bool Won { get; set; }

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
    
}