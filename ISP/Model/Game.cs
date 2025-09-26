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
        Console.WriteLine(player.Color);
        Square currentSquare = player.CurrentSquare == -1 ? null : Board[player.CurrentSquare-1];
        int squareTargetIndex = _move.SqureTargetIndex;
        Square targetSquare = Board[squareTargetIndex-1];
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
            return true;
        }
        else if (currentSquare != null && currentSquare.GetType() == typeof(CarrotSquare) && !movingAway)
        {
            player.TakeCarrots = EatCarrots && player.CurrentSquare >= 6 ? false : true;
            player.ExecuteCommand();
            player.SetCommand(currentSquare.GetCommand(this));
        }


        if (player.RequiredToMove && !movingAway)
            return false;

        int move = int.Abs((player.CurrentSquare == -1 ? 0 : player.CurrentSquare ) - squareTargetIndex);
        if (player.CurrentSquare == -1)
            move++;
        int cost = (move * (move + 1)) / 2;
        if (player.Carrots >= cost && movingAway)
        {
            if (targetSquare.GetType() == typeof(TortoiseSquare)
                && targetSquare.Player == null
                && squareTargetIndex < player.CurrentSquare)
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
                    Console.WriteLine(currentSquare.ToString());
                    Console.WriteLine("helo"); currentSquare.Player = null;}
                player.CurrentSquare = squareTargetIndex;
                targetSquare.Player = player;
                player.SetCommand(targetSquare.GetCommand(this));
                player.RequiredToMove = false;
                player.CarrotsUsedLastTurn = cost;
            }
            
            if (currentSquare != null && (currentSquare.GetType() == typeof(HareSquare)
                                      || currentSquare.GetType() == typeof(TortoiseSquare)
                                      || currentSquare.GetType() == typeof(FinalSquare)))
                player.ExecuteCommand();
            UpdateRank();
            return true;
        }else if (player.Carrots < cost)
            return false;
        
        return false;
    }
    
    public  void UpdateRank()
    {
        List<PlayerBase> players = new();
        foreach (var square in Board)
            if (square.Player != null)
                players.Add(square.Player);

        for (int i = players.Count - 1, j = 0; i >= 0; i--, j++)
            players[j].Rank = i + 1;

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
                    Carrots = 65
                }
                );
        }
    }
}