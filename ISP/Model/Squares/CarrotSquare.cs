using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class CarrotSquare : Square
{
    
    public override Square Clone()
    {
        CarrotSquare clone= new CarrotSquare();
        clone.Player = Player;
        return clone;
    }
    public override string ToString()
    {
        return $"[{Player} Carrot Square ]";
    }

    public override Command GetCommand(Game state)
    {
        Command cmd = new CarrotSquareAction();
        cmd.State = state;
        return cmd;
    }
    
    private class CarrotSquareAction : Command
    {
        private int CarrotsRequired = 10;
        public Game State { get; set; }

        public void Execute(PlayerBase player)
        {
            if (player.TakeCarrots)
            {
                player.Carrots += CarrotsRequired;
            }
            else
            {
                if (player.Carrots < 10)
                {
                    throw new Exception();
                }
                player.Carrots -= CarrotsRequired;
            }
                
        }
    }
}