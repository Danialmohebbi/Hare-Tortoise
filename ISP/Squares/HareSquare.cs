using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;


public class HareSquare : Square
{
    private static List<(HareCard card, int Count)> Cards { get; set; } = new();
    static HareSquare()
    {
        Cards.Add((new Card1(), 2));
        Cards.Add((new Card2(), 2));
        Cards.Add((new Card3(), 2));
        Cards.Add((new Card4(), 2));
        Cards.Add((new Card5(), 2));
        Cards.Add((new Card6(), 2));
        Cards.Add((new Card7(), 2));
        Cards.Add((new Card8(), 2));
    }
    public override string ToString()
    {
        return $"[{Player} Hare Square ]";
    }

    public override Command GetCommand(GameState gameState)
    {
        Command cmd = new HareSquareAction();
        cmd.GameState = gameState;
        return cmd;
    }
    
    public static HareCard GetCard()
    {
        Random rnd = new();
        int randomized;
        do
        {
            randomized = rnd.Next(0, Cards.Count);
        } while (Cards[randomized].Count == 0);

        var selectedCard = Cards[randomized];
        selectedCard.Count--;
        Cards[randomized] = selectedCard;

        return selectedCard.card;
    }

    private class HareSquareAction : Command
    {
        HareCard _card;
        public HareSquareAction()
        {
            _card = HareSquare.GetCard();
        }

        public GameState GameState { get; set; }

        public  void Execute(PlayerBase player)
        {
            _card.SetPlayer(player);
            _card.Execute(GameState);
        }
    }
}