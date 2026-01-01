using ConsoleApp2.Player;
using ConsoleApp2.Rules;
using ConsoleApp2.Squares;

namespace ConsoleApp2;

public class Game
{
    public List<Square> Board { get; private set; }
    public List<PlayerBase> Players { get; private set; } = new();
    public int TurnIndex { get; set; }


    public Game(int NumberOfPlayers)
    {
        InitializePlayers(NumberOfPlayers);
        InitializeBoard();
    }



    private bool AskHumanToReset(PlayerBase player)
    {
        Console.WriteLine($"{player.Color} is stuck!");
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1 - Restart race (reset to Start with 65 carrots)");
        Console.WriteLine("2 - Do not move (end turn)");

        while (true)
        {
            string input = Console.ReadLine()!;
            if (input == "1")
                return true;
            if (input == "2")
                return false;

            Console.WriteLine("Please enter 1 or 2.");
        }
    }


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
    

    private void ApplyMove(Move move)
    {
        var player = Players[TurnIndex];
        var piece = player.Pieces[move.PieceIndex];

        int from = piece.CurrentSquare;
        int to = from + move.Delta;
        if (move.Delta == 0 && move.EatCarrots)
        {
            player.Carrots += 10;
            return;
        }

        Square currentSquare = from == -1 ? null : Board[from];
        Square targetSquare = Board[to];

        int cost = (Math.Abs(move.Delta) * (Math.Abs(move.Delta) + 1)) / 2;
        player.Carrots -= cost;

        if (currentSquare != null)
        {
            currentSquare.Player = null;
            currentSquare.OccupyingPiece = null;
        }

        piece.CurrentSquare = to;
        targetSquare.Player = player;
        targetSquare.OccupyingPiece = piece;

        player.SetCommand(targetSquare.GetCommand(this));
        player.ExecuteCommand(piece);

        UpdateRank();
    }


    public void UpdateRank()
    {
        var ranked = Players
            .OrderByDescending(p => p.FinishCount)
            .ThenByDescending(p => p.Pieces.Max(pc => pc.CurrentSquare))
            .ToList();

        for (int i = 0; i < ranked.Count; i++)
        {
            ranked[i].Rank = i + 1;
        }
    }



    public int? Winner()
    {
        var winner = Players.FindIndex(p => p.FinishCount == 2);
        return winner >= 0 ? winner : (int?)null;
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

    public void InitializePlayers(int humanCount)
    {
        int totalPlayers = 4;
        int aiCount = totalPlayers - humanCount;
        for (int i = 0; i < humanCount; i++)
        {
            Players.Add(new PlayerBase((PlayerColor)(i + 1))
            {
                Carrots = 65,
                Lettuce = 3,
                IsAi = false
            });
        }
        for (int i = 0; i < aiCount; i++)
        {
            Players.Add(new RandomPlayer((PlayerColor)(humanCount + i + 1))
            {
                Carrots = 65,
                Lettuce = 3,
                IsAi = true
            });
        }
    }


    public bool Occupied(int Square)
    {
        foreach (PlayerBase p in Players)
        {
            foreach (Piece piece in p.Pieces)
            if (piece.CurrentSquare == Square)
                return true;
        }

        return false;
    }

    public void HandleStuckPlayer(PlayerBase player)
    {
        foreach (var piece in player.Pieces)
        {
            if (piece.CurrentSquare >= 0)
                Board[piece.CurrentSquare].OccupyingPiece = null;

            piece.CurrentSquare = -1;
            piece.Finished = false;
        }

        player.Carrots = 65;
        player.SkipRound = false;
    }


    public void PlayTurn()
    {
        var player = Players[TurnIndex];

        if (player.FinishCount == 2)
        {
            NextTurn();
            return;
        }

        var legalMoves = MoveGenerator.GetLegalMoves(this, player);

        if (legalMoves.Count == 0)
        {
            HandleStuckPlayer(player);
            NextTurn();
            return;
        }

        Move move;
        do
        {
            move = player.IsAi ? player.move(this) : ReadHumanMove();
        }
        while (move == null || !TryApplyMove(move));

        NextTurn();
    }


    private Move ReadHumanMove()
    {
        var player = Players[TurnIndex];
        var legalMoves = MoveGenerator.GetLegalMoves(this, player);

        Console.WriteLine("Choose a move:");
        for (int i = 0; i < legalMoves.Count; i++)
        {
            Console.WriteLine($"{i} - {legalMoves[i].Describe(this, player)}");
        }

        if (!int.TryParse(Console.ReadLine(), out int choice))
            return null;

        if (choice < 0 || choice >= legalMoves.Count)
            return null;

        return legalMoves[choice];
    }



    public void NextTurn()
    {
        TurnIndex = (TurnIndex + 1) % Players.Count;
    }

    public bool TryApplyMove(Move move)
    {
        var legal = MoveGenerator.GetLegalMoves(this, Players[TurnIndex]);

        if (!legal.Any(m => move.PieceIndex == m.PieceIndex
                            &&   move.EatCarrots == m.EatCarrots
                            &&   move.Delta == m.Delta))   
            return false;

        ApplyMove(move);
        return true;
    }


    public int GetNearestTortoiseSquare(int Square)
    {
        int index = Board
            .Select((b, i) => new { b, i })
            .Where(x => x.b.GetType() == typeof(TortoiseSquare) && x.b.Player == null && x.i < Square)
            .Select(x => x.i)
            .OrderByDescending(x => x).FirstOrDefault(-1);
        return index;
    }
}