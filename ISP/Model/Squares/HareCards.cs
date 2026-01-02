using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp2.Player;

namespace ConsoleApp2.Squares;
/// <summary>
/// Represents a Hare card that can be drawn when a piece lands on a HareSquare.
///
/// Hare cards introduce random events.
/// </summary>
public interface HareCard
{
    public PlayerBase _player { get; set; }
    public void Execute(Game state,Piece piece);
    public void SetPlayer(PlayerBase player);
}
//Aid the Tortoise
public class Card1 : HareCard
{
    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public PlayerBase _player { get; set; }

    public void Execute(Game state,Piece piece)
    {
        foreach (var player in state.Players)
        {
            if (player.Rank == 0)
                continue;
            if (player.Rank < _player.Rank)
                player.Carrots += 10;
            Console.WriteLine(player.Carrots);

        }
    }
    
}
//Change of Pace
public class Card2 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state,Piece piece)
    {
        int behindPlayer = state.Players.FindAll(p => p.Rank < _player.Rank && p.Rank != 0).Count;
        int afterPlayer = state.Players.FindAll(p => p.Rank > _player.Rank && p.Rank != 0).Count;
        if (behindPlayer < afterPlayer)
            state.TurnIndex = (int)_player.Color;
        else
            _player.SkipRound = true;

    }
}
//Overconfidence
public class Card3 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state,Piece piece)
    {
        if (_player.Carrots != 0)
            _player.Carrots /= 2;
    }
}
//Back to Basics
public class Card4 : HareCard
{

    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state,Piece piece)
    {
        _player.Carrots = 65;
    }
}
//Refund
public class Card5 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state,Piece piece)
    {
        _player.Carrots += _player.CarrotsUsedLastTurn;
        _player.CarrotsUsedLastTurn = 0;
    }
}
//Salad or Starve
public class Card6 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state,Piece piece)
    {
        if (_player.Lettuce > 0)
            _player.Carrots += _player.Lettuce * 10;
        else
            _player.SkipRound = true;
    }
}
//Redistribution
public class Card7 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state,Piece piece)
    {
        foreach (PlayerBase player in state.Players)
        {
            _player.Carrots += 2;
            player.Carrots -= 2;
        }
    }
}
//Leap Ahead
public class Card8 : HareCard
{
    public PlayerBase _player { get; set; }

    public void SetPlayer(PlayerBase player)
    {
        _player = player;
    }

    public void Execute(Game state,Piece piece)
    {
        List<int> wantedSquares = new();
        for (int i = 0; i < state.Board.Count; i++)
        {
            var s = state.Board[i];
            if (s.Player == null && i > piece.CurrentSquare && s.GetType() == typeof(CarrotSquare))
            {
                wantedSquares.Add(i);
            }
        }

        if (wantedSquares.Count() > 0)
        {
            state.TryApplyMove(new Move { PieceIndex = piece.PieceIndex,Delta = wantedSquares.OrderBy(x => x).First() - piece.CurrentSquare, EatCarrots = false });
        }

        
    }
}

