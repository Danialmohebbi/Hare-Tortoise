using ConsoleApp2;
using ConsoleApp2.Extensions;
using ConsoleApp2.Player;
using ConsoleApp2.Squares;
using Xunit;

namespace ISP.Tests;

public class CarrotSquareTests
{
    [Theory]
    [InlineData(1,1)]
    [InlineData(4,4)]
    [InlineData(12,0)]
    [InlineData(20,0)]
    [InlineData(25,0)]
    [InlineData(32,0)]
    [InlineData(37,0)]
    [InlineData(39,0)]
    [InlineData(48,0)]
    [InlineData(54,0)]
    [InlineData(58,0)]
    public void MoveToCarrotSquare_ReturnsCorrectSquareIndex(int SquareTargetIndex, int ExpectedTargetIndex)
    {
        GameState gameState = new GameState(1);
        PlayerBase player = gameState.Players[gameState.TurnIndex];
        player.Rank = 1;
        
        gameState.ApplyMove(new Move { SqureTargetIndex = SquareTargetIndex });
        
        Assert.Equal(ExpectedTargetIndex, player.CurrentSquare);
    }
    
    [Theory]
    [InlineData(1,1)]
    [InlineData(4,4)]
    [InlineData(12,0)]
    [InlineData(20,0)]
    [InlineData(25,0)]
    [InlineData(32,0)]
    [InlineData(37,0)]
    [InlineData(39,0)]
    [InlineData(48,0)]
    [InlineData(54,0)]
    [InlineData(58,0)]
    public void MoveToCarrotSquare_ReturnsCorrectSquareType(int SquareTargetIndex, int ExpectedTargetIndex)
    {
        GameState gameState = new GameState(1);
        PlayerBase player = gameState.Players[gameState.TurnIndex];
        player.Rank = 1;
        player.Carrots = 1_000_000;
        
        gameState.ApplyMove(new Move { SqureTargetIndex = SquareTargetIndex });
        
        Assert.IsAssignableFrom<CarrotSquare>(gameState.Board[player.CurrentSquare]);
    }

    
    [Theory]
    [InlineData(20,64)]
    [InlineData(25,64)]
    [InlineData(39,64)]
    [InlineData(48,64)]
    [InlineData(54,64)]
    [InlineData(58,64)]
    public void MoveAfterCarrotSquare_ReturnsCorrectCarrotAmount(int BeginningSquareIndex,int TargetCarrotAmount)
    {
        GameState gameState = new GameState(1);
        PlayerBase player = gameState.Players[gameState.TurnIndex];
        player.Rank = 1;
        player.CurrentSquare = BeginningSquareIndex;
        gameState.Board[BeginningSquareIndex].Player = player;
        
        gameState.ApplyMove(new Move { SqureTargetIndex = BeginningSquareIndex + 1 });
        
        Assert.Equal(TargetCarrotAmount, player.Carrots);
    }
}