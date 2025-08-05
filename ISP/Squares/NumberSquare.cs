using System.Runtime.CompilerServices;
using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class NumberSquare : Square
{
    private int Number = 0;

    public NumberSquare(int number)
    {
        Number = number;
    }
    public override string ToString()
    {
        return $"[{Player} Number Square ({Number}) ]" ;
    }

    public override Command GetCommand(GameState gameState)
    {
        Command cmd = new NumberSquareAction(Number);
        cmd.GameState = gameState;
        return cmd;
    }
    private class NumberSquareAction : Command
    {
        int _number = 0;
        private int CarrotsRequired = 10;
        public NumberSquareAction(int number) => _number = number;
        public GameState GameState { get; set; }

        public void Execute(PlayerBase player)
        {
           if (player.Rank == _number)
               player.Carrots += CarrotsRequired * player.Rank;
        }
    }
}