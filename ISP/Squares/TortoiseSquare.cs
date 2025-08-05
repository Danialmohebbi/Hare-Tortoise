using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class TortoiseSquare : Square
{
    public int moves { get; set; }
    public override string ToString()
    {
        return $"[{Player} Tortoise Square ]";
    }

    public override Command GetCommand(GameState gameState)
    {
        Command cmd = new TortoiseSquareAction(moves);
        moves = 0;
        cmd.GameState = gameState;
        return cmd;    }

    public void SetMoves(int moves) => this.moves = moves;
    
    private class TortoiseSquareAction : Command
    {
        private int CarrotsRequired = 10;
        private int moves = 0;

        public TortoiseSquareAction(int moves)
        {
            this.moves = moves;
        }

        public GameState GameState { get; set; }

        public void Execute(PlayerBase player)
        {
            player.Carrots += moves * CarrotsRequired;
        }
    }
}