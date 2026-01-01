using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;
/// <summary>
/// Represents a Lettuce square on the board.
///
/// When a piece lands on a LettuceSquare, the player must
/// consume one lettuce and is forced to remain on the square
/// for the duration of the action.
/// 
/// The player is rewarded with carrots based on their current rank,
/// and is temporarily prevented from moving again until the lettuce
/// effect has been resolved.
/// </summary>
public class LettuceSquare : Square
{
    public override string ToString()
    {
        string s = Player == null ? "." : $"{Player.Color}";
        return $"[L,{s}]";
    }

    public override Command GetCommand(Game state)
    {
        Command cmd = new LettuceSquareAction();
        cmd.State = state;
        return cmd;
    }
    private class LettuceSquareAction : Command
    {
        
        private int CarrotsRequired = 10;
        public Game State { get; set; }

        public void Execute(PlayerBase player, Piece piece)
        {
            player.Lettuce--;
            player.Carrots += CarrotsRequired * player.Rank;
            player.RequiredToMove = true;
        }
    }
}