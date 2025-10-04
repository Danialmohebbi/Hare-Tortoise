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
            view.DrawPrefix();
            Move move = GetMove();
            game.Move(move);
            game.TurnIndex = (game.TurnIndex + 1) % game.Players.Count;
            view.Draw();Console.WriteLine();
            view.DrawPostfix();
        }
        
        GameOver();
    }

    public Move GetMove()
    {
        Console.WriteLine("Please enter the box you wish to move to: ");
        int inputedBoxIndex = int.Parse(Console.ReadLine());
        if (game.Players[game.TurnIndex].GetType() == typeof(CarrotSquare))
        {
            Console.WriteLine("Please enter if you wish to stay and eat some yummy carrots: Y/N ");
            string inputEatCarrots = Console.ReadLine();
            bool eatCarrots = inputEatCarrots == null ? false : (inputEatCarrots.Equals("Y") ? true : false);
            return new Move {EatCarrots = eatCarrots, SqureTargetIndex = inputedBoxIndex};
        }
        
        return new Move {SqureTargetIndex = inputedBoxIndex};
       
    }
    public void GameOver() { }
    
}