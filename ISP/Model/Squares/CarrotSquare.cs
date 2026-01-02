using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;
/// <summary>
/// Represents a Carrot square on the board.
///
/// Carrot squares allow a player to either:
/// - Eat carrots (gain carrots and stay on the square)
/// - Move away
/// </summary>
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
                if (player.Carrots > 10)
                {
                    player.Carrots -= CarrotsRequired;
                }
                
            }
                
        }
    }
}