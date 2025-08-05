using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class LettuceSquare : Square
{
    public override string ToString()
    {
        return $"[{Player} Lettuce Square ]";
    }

    public override Command GetCommand(GameState gameState)
    {
        Command cmd = new LettuceSquareAction();
        cmd.GameState = gameState;
        return cmd;
    }
    private class LettuceSquareAction : Command
    {
        
        private int CarrotsRequired = 10;
        public GameState GameState { get; set; }

        public void Execute(PlayerBase player)
        {
            player.Lettuce--;
            player.Carrots += CarrotsRequired * player.Rank;
            player.RequiredToMove = true;
        }
    }
}