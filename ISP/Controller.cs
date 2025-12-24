using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

public class Controller
{
    Game game;
    View.View view;

    public Controller(Game game)
    {
        this.game = game;
        view = new View.View{ g = game };
}
    public void Run()
    {
        while (game.Winner() == null)
        {
            Console.WriteLine($"Red Player: {game.Players[0].CurrentSquare}");
            view.DrawPrefix();
            Move move = GetMove();
            while (move == null || !game.Move(move))
            {
                
                move = GetMove();
            }

            if (game.TurnIndex == 1)
            {
                Console.WriteLine(move.SqureTargetIndex);
            }
            game.TurnIndex = (game.TurnIndex + 1) % game.Players.Count;
            view.Draw();Console.WriteLine();
            view.DrawPostfix();
        }
        
        GameOver();
    }

    public Move? GetMove()
    {
        if (game.Players[game.TurnIndex].IsAi)
        {
            Move? move = game.Players[game.TurnIndex].move(game);
            if (move == null)
                game.Players[game.TurnIndex] = new RandomPlayer(game.Players[game.TurnIndex].Color) 
                {Carrots = game.Players[game.TurnIndex].Carrots + 65, Lettuce = game.Players[game.TurnIndex].Lettuce, IsAi = true};
            return game.Players[game.TurnIndex].move(game);
        }
        bool eatCarrots = false;
        int curPlayerIndex = game.Players[game.TurnIndex].CurrentSquare;
        
        if (curPlayerIndex != -1 && game.Board[curPlayerIndex].GetType() == typeof(CarrotSquare))
        {
            Console.WriteLine("Do you wish to move?[YES/NO]");
            string inputEatCarrots = Console.ReadLine()!;
            if (inputEatCarrots.Equals("YES"))
            {
                eatCarrots = false;
            }else if (inputEatCarrots.Equals("NO"))
            {
                eatCarrots = true;
            }
            else
            {
                return null;
            }
            if (eatCarrots)
            {
                return new Move { SqureTargetIndex = curPlayerIndex, EatCarrots = eatCarrots };
            }
        }
        Console.WriteLine("Please enter the box you wish to move to: ");
        int inputedBoxIndex = int.Parse(Console.ReadLine());
        
        return new Move {SqureTargetIndex = inputedBoxIndex, EatCarrots = eatCarrots };
    }
    public void GameOver() { }
    
}