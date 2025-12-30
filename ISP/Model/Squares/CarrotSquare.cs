using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class CarrotSquare : Square
{
    
    public override string ToString()
    {
        string s = Player == null ? "." : $"{Player.Color}";
        return $"[C,{s}]";    }

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

        public void Execute(PlayerBase player, Piece piece)
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