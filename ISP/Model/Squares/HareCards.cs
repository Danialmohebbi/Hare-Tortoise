using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;

public interface HareCard
{
    public PlayerBase _player { get; set; }
    public void Execute(Game state);
    public void SetPlayer(PlayerBase player);
}

public static class HareCardExtensions
{
    public static HareCard Clone(this HareCard card)
    {
        HareCard c = (HareCard)Activator.CreateInstance(card.GetType())!;
        c.SetPlayer(card._player);
        return c;
    }
}


public class Card1 : HareCard
{
    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public PlayerBase _player { get; set; }

    public void Execute(Game state)
    {
        foreach (var player in state.Players)
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
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state)
    {
        int behindPlayer = state.Players.FindAll(p => p.Rank < _player.Rank && p.Rank != 0).Count;
        int afterPlayer = state.Players.FindAll(p => p.Rank > _player.Rank && p.Rank != 0).Count;
        if (behindPlayer < afterPlayer)
            state.TurnIndex = (int)_player.Color;
        else
            _player.SkipRound = true;

    }
}

public class Card3 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state)
    {
        if (_player.Carrots != 0)
            _player.Carrots /= 2;
    }
}

public class Card4 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state)
    {
        _player.Carrots = 65;
    }
}

public class Card5 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state)
    {
        _player.Carrots += _player.CarrotsUsedLastTurn;
        _player.CarrotsUsedLastTurn = 0;
    }
}

public class Card6 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state)
    {
        if (_player.Lettuce > 0)
            _player.Carrots += _player.Lettuce * 10;
        else
            _player.SkipRound = true;
    }
}

public class Card7 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state)
    {
        foreach (PlayerBase player in state.Players)
        {
            _player.Carrots += 2;
            player.Carrots -= 2;
        }
    }
}

public class Card8 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state)
    {
        List<int> squares = state.GetSquareIndexes(new CarrotSquare());
        IEnumerable<int> wantedSquares = from s in squares
            where state.Board[s].Player == null && s > _player.CurrentSquare
                select s;

        if (wantedSquares.Count() > 0)
        {
            state.Board[_player.CurrentSquare].Player = null;
            state.Board[wantedSquares.ElementAt(0)].Player = _player;
            _player.CurrentSquare = wantedSquares.ElementAt(0);
            _player.SetCommand(state.Board[wantedSquares.ElementAt(0)].GetCommand(state));
        }

        
    }
}

