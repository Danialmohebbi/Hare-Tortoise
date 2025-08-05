using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

public class GameState
{
    public List<Square> Board { get; private set; }
    public List<PlayerBase> Players { get; private set; } = new();
    public int TurnIndex { get; set; }


    // Keep singleton for game runtime

    // Add this public constructor for testing
    public GameState(int NumberOfPlayers)
    {
        InitializeBoard();
        InitializePlayers(NumberOfPlayers - 1);
        
    }

    

    public void InitializeBoard()
    {
                Board = new List<Square>
        {
            new HareSquare(),
            new CarrotSquare(),
            new HareSquare(),
            new NumberSquare(1),
            new CarrotSquare(),
            new HareSquare(),
            new LettuceSquare(),
            new TortoiseSquare(),
            new NumberSquare(4),
            new NumberSquare(2),
            new TortoiseSquare(),
            new NumberSquare(3),
            new CarrotSquare(),
            new HareSquare(),
            new TortoiseSquare(),
            new FinalSquare(),
            new NumberSquare(2),
            new NumberSquare(4),
            new TortoiseSquare(),
            new NumberSquare(3),
            new CarrotSquare(),
            new LettuceSquare(),
            new NumberSquare(2),
            new TortoiseSquare(),
            new HareSquare(),
            new CarrotSquare(),
            new NumberSquare(4),
            new NumberSquare(3),
            new NumberSquare(2),
            new TortoiseSquare(),
            new HareSquare(),
            new FinalSquare(),
            new CarrotSquare(),
            new HareSquare(),
            new NumberSquare(2),
            new NumberSquare(3),
            new TortoiseSquare(),
            new CarrotSquare(),
            new HareSquare(),
            new CarrotSquare(),
            new NumberSquare(2),
            new LettuceSquare(),
            new TortoiseSquare(),
            new NumberSquare(3),
            new NumberSquare(4),
            new HareSquare(),
            new NumberSquare(2),
            new FinalSquare(),
            new CarrotSquare(),
            new TortoiseSquare(),
            new HareSquare(),
            new NumberSquare(3),
            new NumberSquare(2),
            new NumberSquare(4),
            new CarrotSquare(),
            new TortoiseSquare(),
            new LettuceSquare(),
            new HareSquare(),
            new CarrotSquare(),
            new NumberSquare(2),
            new HareSquare(),
            new LettuceSquare(),
            new HareSquare(),
        };
    }

    public void InitializePlayers(int NumberOfPlayers)
    {
        if (Players.Count > 0)
            return;

        for (int p = 0; p < NumberOfPlayers; p++)
        {
            Players.Add(new PlayerBase((PlayerColor)p)
                {
                    Carrots = 65
                }
                );
        }
        var player = new PlayerBase(PlayerColor.Red)
        {
            Carrots = 65,
            Rank = 1
        };
        Players.Add(player);
    }

    

    public bool GameOver() => Players.Any(p => p.Won);

    public List<int> GetSquareIndexes<T>(T square) where T : Square
    {
        List<int> indexes = new();
        for (int i = 0; i < Board.Count; i++)
        {
            if (Board[i].GetType() == typeof(T))
                indexes.Add(i);
        }
        return indexes;
    }

}