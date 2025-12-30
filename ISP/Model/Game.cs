using ConsoleApp2.Player;
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



    public List<Move> possibleMoves()
    {
        List<Move> moves = new();
        PlayerBase player = Players[TurnIndex];

        for (int i = 0; i < player.Pieces.Count; i++)
        {
            Piece piece = player.Pieces[i];
            if (piece.Finished) continue;
            int baseSquare = piece.CurrentSquare;
            if (player.SkipRound)
            {
                moves.Add(new Move { PieceIndex = i,SqureTargetIndex = 0, EatCarrots = false });
                return moves;
            }
            if (baseSquare >= 0 &&
                Board[baseSquare] is CarrotSquare)
            {
                moves.Add(new Move { PieceIndex = i,SqureTargetIndex = 0, EatCarrots = true });
            }

            for (int delta = 1;; delta++)
            {
                int target = baseSquare + delta;
                if (baseSquare == -1)
                    target = delta - 1;

                if (target < 0 || target >= Board.Count)
                    break;

                int cost = (delta * (delta + 1)) / 2;
                if (player.Carrots < cost)
                    break;

                if (Occupied(target))
                    continue;

                Square targetSquare = Board[target];
                if (targetSquare is TortoiseSquare)
                    continue;
                if (baseSquare >= 0 &&
                    Board[baseSquare] is LettuceSquare &&
                    !player.RequiredToMove)
                {
                    continue;
                }

                moves.Add(new Move
                {
                    PieceIndex = i,
                    SqureTargetIndex = delta,
                    EatCarrots = false
                });

                if (targetSquare is CarrotSquare)
                {
                    moves.Add(new Move
                    {
                        PieceIndex = i,
                        SqureTargetIndex = delta,
                        EatCarrots = true
                    });
                }
            }
            if (baseSquare >= 0)
            {
                int nearestTortoise = GetNearestTortoiseSquare(baseSquare);
                if (nearestTortoise != -1 && !Occupied(nearestTortoise))
                {
                    moves.Add(new Move
                    {
                        PieceIndex = i,
                        SqureTargetIndex = nearestTortoise - baseSquare,
                        EatCarrots = false
                    });
                }
            }
        }
        return moves;
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
    

    public bool Move(Move _move)
    {
        var player = Players[TurnIndex];
        var piece = player.Pieces[_move.PieceIndex];
        int squareTargetIndex = _move.SqureTargetIndex + piece.CurrentSquare;
        bool EatCarrots = _move.EatCarrots;
        bool movingAway = piece.CurrentSquare != squareTargetIndex;

        if (_move.SqureTargetIndex == 0 && squareTargetIndex == piece.CurrentSquare && _move.EatCarrots)
        {
            player.Carrots += 10;
            return true;
        }

        if (Occupied(squareTargetIndex))
        {
            Console.WriteLine("The square you have chosen is already occupied!");
            return false;
        }



        if (squareTargetIndex >= Board.Count ||
            squareTargetIndex < piece.CurrentSquare && typeof(TortoiseSquare) != Board[squareTargetIndex].GetType())
        {
            Console.WriteLine(squareTargetIndex);
            Console.WriteLine("Please enter a valid number of moves to play");
            return false;
        }

        Square currentSquare = piece.CurrentSquare == -1 ? null : Board[piece.CurrentSquare];
        Square targetSquare = Board[squareTargetIndex];



        if (player.SkipRound)
        {
            Console.WriteLine("HEY NUMBER 1");
            player.SkipRound = false;
            return true;
        }

        if (currentSquare != null && currentSquare.GetType() == typeof(NumberSquare) && movingAway)
            player.ExecuteCommand(piece);
        else if (currentSquare != null && currentSquare.GetType() == typeof(LettuceSquare) && !player.RequiredToMove)
        {
            Console.WriteLine("HEY NUMBER 2");
            player.ExecuteCommand(piece);
            if (movingAway)
                Console.WriteLine("You are not allowed to move while chewwing on food. it's Dangerous!");
            return true;
        }
        else if (currentSquare != null && currentSquare.GetType() == typeof(CarrotSquare) && !movingAway)
        {
            player.TakeCarrots = EatCarrots;
            player.ExecuteCommand(piece);
            player.SetCommand(currentSquare.GetCommand(this));
        }


        int move = int.Abs((piece.CurrentSquare == -1 ? 0 : piece.CurrentSquare) - squareTargetIndex);
        int cost = (move * (move + 1)) / 2;
        if (player.Carrots >= cost && movingAway)
        {
            if (targetSquare.GetType() == typeof(TortoiseSquare) && piece.CurrentSquare < squareTargetIndex)
            {
                Console.WriteLine("The square you have chosen can only be moved to backwardly!");
                return false;
            }

            if (targetSquare.GetType() == typeof(TortoiseSquare) && piece.CurrentSquare > squareTargetIndex
                                                                 && targetSquare.Player == null
                                                                 && squareTargetIndex < piece.CurrentSquare
                                                                 && GetNearestTortoiseSquare(piece.CurrentSquare) ==
                                                                 squareTargetIndex)
            {
                (targetSquare as TortoiseSquare).SetMoves(move);
                player.SetCommand(targetSquare.GetCommand(this));
                currentSquare.Player = null;
                currentSquare.OccupyingPiece = null;
                targetSquare.Player = player;
                targetSquare.OccupyingPiece = piece;
                piece.CurrentSquare = squareTargetIndex;
            }
            else
            {
                player.Carrots -= cost;
                if (currentSquare != null) currentSquare.Player = null;

                piece.CurrentSquare = squareTargetIndex;
                targetSquare.Player = player;
                targetSquare.OccupyingPiece = piece;
                player.SetCommand(targetSquare.GetCommand(this));
                player.RequiredToMove = false;
                player.CarrotsUsedLastTurn = cost;
            }

            if ((targetSquare.GetType() == typeof(HareSquare)
                 || targetSquare.GetType() == typeof(TortoiseSquare)
                 || targetSquare.GetType() == typeof(FinalSquare)))
            {
                player.ExecuteCommand(piece);
            }

            UpdateRank();
            Console.WriteLine("HEY NUMBER 3");
            return true;
        }
        else if (player.Carrots < cost)
        {
            player.RequiredToMove = false;
            player.SkipRound = true;  
            return false;
        }

        return false;
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


    private bool Occupied(int Square)
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
        PlayerBase player = Players[TurnIndex];

        if (player.FinishCount == 2)
        {
            NextTurn();
            return;
        }

        player.RestartedThisTurn = false;
        Console.WriteLine();

        bool stuck = player.Carrots <= 0 || possibleMoves().Count == 0;

        if (stuck && !player.IsAi)
        {
            bool wantsReset = AskHumanToReset(player);

            if (!wantsReset)
            {
                Console.WriteLine($"{player.Color} chooses not to move.");
                NextTurn();
                return;
            }
        }
        if (stuck)
        {
            HandleStuckPlayer(player);

            if (possibleMoves().Count == 0)
            {
                Console.WriteLine($"{player.Color} still cannot move. Turn skipped.");
                NextTurn();
                return;
            }
        }
        
        List<Move> legalMoves = possibleMoves();

        while (true)
        {
            if (legalMoves.Count == 0)
            {
                Console.WriteLine($"{player.Color} has no legal moves.");
                break;
            }

            Move move;

            if (player.IsAi)
            {
                move = player.move(this);

                if (!legalMoves.Any(m =>
                        m.PieceIndex == move.PieceIndex &&
                        m.SqureTargetIndex == move.SqureTargetIndex &&
                        m.EatCarrots == move.EatCarrots))
                {
                    legalMoves.RemoveAll(m =>
                        m.PieceIndex == move.PieceIndex &&
                        m.SqureTargetIndex == move.SqureTargetIndex &&
                        m.EatCarrots == move.EatCarrots);

                    continue;
                }
            }
            else
            {
                move = ReadHumanMove();

                if (move == null)
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }
                
            }
            if (Move(move))
            {
                break;
            }
            legalMoves.RemoveAll(m =>
                m.PieceIndex == move.PieceIndex &&
                m.SqureTargetIndex == move.SqureTargetIndex &&
                m.EatCarrots == move.EatCarrots);
        }

        NextTurn();
    }


private Move? ReadHumanMove()
{
    PlayerBase player = Players[TurnIndex];
    Console.WriteLine("Choose piece:");
    for (int i = 0; i < player.Pieces.Count; i++)
    {
        var pc = player.Pieces[i];
        string pos = pc.CurrentSquare == -1 ? "Start" : pc.CurrentSquare.ToString();
        Console.WriteLine($"{i} - Piece at {pos}");
    }

    if (!int.TryParse(Console.ReadLine(), out int pieceIndex) ||
        pieceIndex < 0 || pieceIndex >= player.Pieces.Count)
    {
        return null;
    }

    Piece piece = player.Pieces[pieceIndex];
    int baseSquare = piece.CurrentSquare;
    bool eatCarrots = false;
    if (baseSquare >= 0 && Board[baseSquare] is CarrotSquare)
    {
        Console.WriteLine("Do you want to move? [YES/NO]");
        string input = Console.ReadLine()!.Trim().ToUpper();
        
        if (input == "YES")
        {
            eatCarrots = false;
        }
        else if (input == "NO")
        {
            eatCarrots = true;
            return new Move() { EatCarrots = eatCarrots, SqureTargetIndex = 0 ,PieceIndex = pieceIndex};
        }
        else
        {
            return null;
        }
    }

    Console.WriteLine("Please enter the box you wish to move to:");

    if (!int.TryParse(Console.ReadLine(), out int targetSquare))
    {
        return null;
    }

    int delta;

    if (baseSquare == -1)
    {
        delta = targetSquare + 1;
    }
    else
    {
        delta = targetSquare;
    }

    return new Move
    {
        PieceIndex = pieceIndex,
        SqureTargetIndex = delta,
        EatCarrots = eatCarrots
    };
}

    public void NextTurn()
    {
        TurnIndex = (TurnIndex + 1) % Players.Count;
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