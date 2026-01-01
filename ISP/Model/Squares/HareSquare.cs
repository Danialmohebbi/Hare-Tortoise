using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

/// <summary>
/// Represents a Hare square on the board.
///
/// When a piece lands on a HareSquare, a HareCard is drawn
/// and executed immediately. Hare cards introduce random
/// events that often disadvantage leading players and
/// benefit players who are behind.
/// </summary>
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
        string s = Player == null ? "." : $"{Player.Color}";
        return $"[H,{s}]";    }

    public override Command GetCommand(Game state)
    {
        Command cmd = new HareSquareAction();
        cmd.State = state;
        return cmd;
    }
    
    public static HareCard GetCard()
    {
        Random rnd = new();
        int randomized;
        do
        {
            if (Cards.All(kv => kv.Count == 0))
            {
                return null;
            }
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

        public Game State { get; set; }

        public  void Execute(PlayerBase player, Piece piece)
        {
            if (_card != null)
            {
                _card.SetPlayer(player);
                _card.Execute(State,piece);
            }
        }
    }
}