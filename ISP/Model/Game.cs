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

        int baseSquare = player.CurrentSquare;

        // Skip round → only noop
        if (player.SkipRound)
        {
            moves.Add(new Move { SqureTargetIndex = 0, EatCarrots = false });
            return moves;
        }

        // Eat carrots in place
        if (baseSquare >= 0 &&
            Board[baseSquare] is CarrotSquare &&
            player.Carrots < 6)
        {
            moves.Add(new Move { SqureTargetIndex = 0, EatCarrots = true });
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
                SqureTargetIndex = delta,
                EatCarrots = false
            });

            if (targetSquare is CarrotSquare)
            {
                moves.Add(new Move
                {
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
                    SqureTargetIndex = nearestTortoise - baseSquare,
                    EatCarrots = false
                });
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

    public Game Clone()
    {
        Game clone = new Game(Players.Count);
        clone.Board = (from b in Board select b.Clone()).ToList();
        clone.Players = (from p in Players select p.Clone()).ToList();
        clone.TurnIndex = TurnIndex;
        return clone;
    }

    public bool Move(Move _move)
    {
        var player = Players[TurnIndex];
        int squareTargetIndex = _move.SqureTargetIndex + player.CurrentSquare;
        bool EatCarrots = _move.EatCarrots;
        bool movingAway = player.CurrentSquare != squareTargetIndex;

        if (_move.SqureTargetIndex == player.CurrentSquare && _move.EatCarrots)
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
            squareTargetIndex < player.CurrentSquare && typeof(TortoiseSquare) != Board[squareTargetIndex].GetType())
        {
            Console.WriteLine(squareTargetIndex);
            Console.WriteLine("Please enter a valid number of moves to play");
            return false;
        }

        Square currentSquare = player.CurrentSquare == -1 ? null : Board[player.CurrentSquare];
        Square targetSquare = Board[squareTargetIndex];



        if (player.SkipRound)
        {
            player.SkipRound = false;
            return true;
        }

        if (currentSquare != null && currentSquare.GetType() == typeof(NumberSquare) && movingAway)
            player.ExecuteCommand();
        else if (currentSquare != null && currentSquare.GetType() == typeof(LettuceSquare) && !player.RequiredToMove)
        {
            player.ExecuteCommand();
            if (movingAway)
                Console.WriteLine("You are not allowed to move while chewwing on food. it's Dangerous!");
            return true;
        }
        else if (currentSquare != null && currentSquare.GetType() == typeof(CarrotSquare) && !movingAway)
        {
            player.TakeCarrots = EatCarrots && player.CurrentSquare >= 6 ? false : true;
            player.ExecuteCommand();
            player.SetCommand(currentSquare.GetCommand(this));
        }


        int move = int.Abs((player.CurrentSquare == -1 ? 0 : player.CurrentSquare) - squareTargetIndex);
        int cost = (move * (move + 1)) / 2;
        if (player.Carrots >= cost && movingAway)
        {
            if (targetSquare.GetType() == typeof(TortoiseSquare) && player.CurrentSquare < squareTargetIndex)
            {
                Console.WriteLine("The square you have chosen can only be moved to backwardly!");
                return false;
            }

            if (targetSquare.GetType() == typeof(TortoiseSquare) && player.CurrentSquare > squareTargetIndex
                                                                 && targetSquare.Player == null
                                                                 && squareTargetIndex < player.CurrentSquare
                                                                 && GetNearestTortoiseSquare(player.CurrentSquare) ==
                                                                 squareTargetIndex)
            {
                (targetSquare as TortoiseSquare).SetMoves(move);
                player.SetCommand(targetSquare.GetCommand(this));
                currentSquare.Player = null;
                targetSquare.Player = player;
                player.CurrentSquare = squareTargetIndex;
            }
            else
            {
                player.Carrots -= cost;
                if (currentSquare != null)
                {
                    currentSquare.Player = null;
                }

                player.CurrentSquare = squareTargetIndex;
                targetSquare.Player = player;
                player.SetCommand(targetSquare.GetCommand(this));
                player.RequiredToMove = false;
                player.CarrotsUsedLastTurn = cost;
            }

            if ((targetSquare.GetType() == typeof(HareSquare)
                 || targetSquare.GetType() == typeof(TortoiseSquare)
                 || targetSquare.GetType() == typeof(FinalSquare)))
            {
                player.ExecuteCommand();
            }

            UpdateRank();
            return true;
        }
        else if (player.Carrots < cost)
            return false;

        return false;
    }

    public void UpdateRank()
    {
        List<PlayerBase> players = new();
        List<PlayerBase> currentPlayers = new();
        foreach (var square in Board)
            if (square.Player != null)
            {
                players.Add(square.Player);
                currentPlayers.Add(square.Player);
            }

        for (int i = currentPlayers.Count, j = 0; i >= 1; i--, j++)
        {
            currentPlayers[j].Rank = i;
        }

    }


    public int? Winner()
    {
        var winner = Players.FindIndex(p => p.Won);
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

    public void InitializePlayers(int NumberOfPlayers)
    {

        // for (int p = 1; p <= NumberOfPlayers; p++)
        // { Players.Add(new PlayerBase((PlayerColor)p)
        //         {
        //             Carrots = 1000,
        //             Lettuce = 3
        //         }
        //         );
        // }
        Players.Add(new PlayerBase((PlayerColor.R)) { Carrots = 1000, Lettuce = 3, IsAi = false });
        Players.Add(new RandomPlayer((PlayerColor.B)) { Carrots = 65, Lettuce = 3, IsAi = true });
    }

    private bool Occupied(int Square)
    {
        foreach (PlayerBase p in Players)
        {
            if (p.CurrentSquare == Square)
                return true;
        }

        return false;
    }

    public bool HandleStuckPlayer(PlayerBase player)
    {
        if (player.RestartedThisTurn)
            return false;

        Console.WriteLine($"{player.Color} is stuck and must restart!");

        // Remove from board
        if (player.CurrentSquare >= 0)
            Board[player.CurrentSquare].Player = null;

        player.CurrentSquare = -1;
        player.Carrots = 65;
        player.SkipRound = false;
        player.RequiredToMove = false;
        player.CarrotsUsedLastTurn = 0;
        player.RestartedThisTurn = true;

        return true;
    }

    public void PlayTurn()
    {
        PlayerBase player = Players[TurnIndex];
        player.RestartedThisTurn = false;
        Console.WriteLine();
        bool stuck = player.Carrots <= 0 || possibleMoves().Count == 0;
        if (stuck && !player.IsAi)
        {
            stuck = AskHumanToReset(player);
        }
        if (stuck)
        {
            bool restarted = HandleStuckPlayer(player);

            if (!restarted || possibleMoves().Count == 0)
            {
                Console.WriteLine($"{player.Color} still cannot move. Turn skipped.");
                NextTurn();
                return;
            }
        }

        Move move = player.IsAi
            ? player.move(this)
            : ReadHumanMove();

        Move(move);

        NextTurn();
    }

    private Move ReadHumanMove()
    {
        bool eatCarrots = false;
        int curPlayerIndex = Players[TurnIndex].CurrentSquare;
        
        if (curPlayerIndex != -1 && Board[curPlayerIndex].GetType() == typeof(CarrotSquare))
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

    private void NextTurn()
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