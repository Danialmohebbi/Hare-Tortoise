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
        PlayerBase currentPlayer = Players[TurnIndex];
        for (int i = 1; i < Board.Count; i++)
        {
            int move = currentPlayer.CurrentSquare + i;
            int cost = (move * (move + 1)) / 2;
            if (currentPlayer.Carrots >= cost)
            {
                moves.Add(new Move { EatCarrots = true,SqureTargetIndex = move });
                moves.Add(new Move { EatCarrots = false,SqureTargetIndex = move });
            }
        }

        for (int i = currentPlayer.CurrentSquare - 1; i >= 0; i--)
        {
            if (Board[i].GetType() == typeof(TortoiseSquare))
            {
                moves.Add(new Move { EatCarrots = false, SqureTargetIndex = i });
            }
        }
        
        return moves;
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
        if (Occupied(squareTargetIndex))
        {
            Console.WriteLine("The square you have chosen is already occupied!");
            return false;
        }
        


        if (squareTargetIndex >= Board.Count ||
            squareTargetIndex < player.CurrentSquare && typeof(TortoiseSquare) != Board[squareTargetIndex].GetType() )
        {
            Console.WriteLine(squareTargetIndex);
            Console.WriteLine("Please enter a valid number of moves to play");
            return false;
        }
        
        Square currentSquare = player.CurrentSquare == -1 ? null : Board[player.CurrentSquare];
        Square targetSquare = Board[squareTargetIndex];
        bool EatCarrots = _move.EatCarrots;
        bool movingAway = player.CurrentSquare != squareTargetIndex;
        

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
        
    
        int move = int.Abs((player.CurrentSquare == -1 ? 0 : player.CurrentSquare ) - squareTargetIndex);
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
                && GetNearestTortoiseSquare(player.CurrentSquare) == squareTargetIndex)
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
                    currentSquare.Player = null;}
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
        }else if (player.Carrots < cost)
            return false;
        
        return false;
    }
    
    public  void UpdateRank()
    {
        List<PlayerBase> players = new();
        List<PlayerBase> currentPlayers = new();
        foreach (var square in Board)
            if (square.Player != null)
            {
                players.Add(square.Player);
                currentPlayers.Add(square.Player);
            }
        
        for (int i = currentPlayers.Count , j = 0; i >= 1; i--, j++)
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

        for (int p = 1; p <= NumberOfPlayers; p++)
        { Players.Add(new PlayerBase((PlayerColor)p)
                {
                    Carrots = 1000,
                    Lettuce = 3
                }
                );
        }
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