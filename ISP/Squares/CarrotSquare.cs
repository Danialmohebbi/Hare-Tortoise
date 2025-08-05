using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class CarrotSquare : Square
{
    public override string ToString()
    {
        return $"[{Player} Carrot Square ]";
    }

    public override Command GetCommand(GameState gameState)
    {
        Command cmd = new CarrotSquareAction();
        cmd.GameState = gameState;
        return cmd;
    }
    
    private class CarrotSquareAction : Command
    {
        private int CarrotsRequired = 10;
        public GameState GameState { get; set; }

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