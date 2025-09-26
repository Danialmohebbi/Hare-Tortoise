using System.Runtime.CompilerServices;
using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public class NumberSquare : Square
{
    private int Number = 0;
    public override Square Clone()
    {
        NumberSquare clone= new NumberSquare(Number);
        clone.Player = Player;
        return clone;
    }
    public NumberSquare(int number)
    {
        Number = number;
    }
    public override string ToString()
    {
        string s = Player == null ? "." : $"{Player.Color}";
        return $"[N,{s}]";    }

    public override Command GetCommand(Game state)
    {
        Command cmd = new NumberSquareAction(Number);
        cmd.State = state;
        return cmd;
    }
    private class NumberSquareAction : Command
    {
        int _number = 0;
        private int CarrotsRequired = 10;
        public NumberSquareAction(int number) => _number = number;
        public Game State { get; set; }

        public void Execute(PlayerBase player)
        {
           if (player.Rank == _number)
               player.Carrots += CarrotsRequired * player.Rank;
        }
        

    }
}