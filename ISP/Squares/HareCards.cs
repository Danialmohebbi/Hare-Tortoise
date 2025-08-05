using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public interface HareCard
{
    public void Execute(GameState gameState);
    public void SetPlayer(PlayerBase player);
}

public class Card1 : HareCard
{
    PlayerBase _player;
    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }
    public void Execute(GameState gameState)
    {
        Console.WriteLine("Card1");
        foreach (var player in gameState.Players)
        {
            if (player.Rank == 0)
                continue;
            if (player.Rank < _player.Rank)
                player.Carrots += 10;
        }
    }
}


public class Card2 : HareCard
{
    PlayerBase _player;

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(GameState gameState)
    {
        Console.WriteLine("Card2");
        int behindPlayer = gameState.Players.FindAll(p => p.Rank < _player.Rank && p.Rank != 0).Count;
        int afterPlayer = gameState.Players.FindAll(p => p.Rank > _player.Rank && p.Rank != 0).Count;
        if (behindPlayer < afterPlayer)
            gameState.TurnIndex = (int)_player.Color;
        else
            _player.SkipRound = true;

    }
}

public class Card3 : HareCard
{
    PlayerBase _player;

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(GameState gameState)
    {
        Console.WriteLine("Card3");
        if (_player.Carrots != 0)
            _player.Carrots /= 2;
    }
}

public class Card4 : HareCard
{
    PlayerBase _player;

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(GameState gameState)
    {
        Console.WriteLine("Card4");
        _player.Carrots = 65;
    }
}

public class Card5 : HareCard
{
    PlayerBase _player;

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(GameState gameState)
    {
        Console.WriteLine("Card5");
        _player.Carrots += _player.CarrotsUsedLastTurn;
        _player.CarrotsUsedLastTurn = 0;
    }
}

public class Card6 : HareCard
{
    PlayerBase _player;

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(GameState gameState)
    {
        Console.WriteLine("Card6");
        if (_player.Lettuce > 0)
            _player.Carrots += _player.Lettuce * 10;
        else
            _player.SkipRound = true;
    }
}

public class Card7 : HareCard
{
    PlayerBase _player;

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(GameState gameState)
    {
        Console.WriteLine("Card7");
        foreach (PlayerBase player in gameState.Players)
        {
            _player.Carrots += 2;
            player.Carrots -= 2;
        }
    }
}

public class Card8 : HareCard
{
    PlayerBase _player;

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(GameState gameState)
    {
        Console.WriteLine("Card8");
        List<int> squares = gameState.GetSquareIndexes(new CarrotSquare());
        IEnumerable<int> wantedSquares = from s in squares
            where gameState.Board[s].Player == null && s > _player.CurrentSquare
                select s;

        if (wantedSquares.Count() > 0)
        {
            gameState.Board[_player.CurrentSquare].Player = null;
            gameState.Board[wantedSquares.ElementAt(0)].Player = _player;
            _player.CurrentSquare = wantedSquares.ElementAt(0);
            _player.SetCommand(gameState.Board[wantedSquares.ElementAt(0)].GetCommand(gameState));
        }

        
    }
}

