using ConsoleApp2;
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
        Game state = new Game(1);
        PlayerBase player = state.Players[state.TurnIndex];
        player.Rank = 1;
        
        state.Move(new Move { SqureTargetIndex = SquareTargetIndex });
        
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
        Game state = new Game(1);
        PlayerBase player = state.Players[state.TurnIndex];
        player.Rank = 1;
        player.Carrots = 1_000_000;
        
        state.Move(new Move { SqureTargetIndex = SquareTargetIndex });
        
        Assert.IsAssignableFrom<CarrotSquare>(state.Board[player.CurrentSquare]);
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
        Game state = new Game(1);
        PlayerBase player = state.Players[state.TurnIndex];
        player.Rank = 1;
        player.CurrentSquare = BeginningSquareIndex;
        state.Board[BeginningSquareIndex].Player = player;
        
        state.Move(new Move { SqureTargetIndex = BeginningSquareIndex + 1 });
        
        Assert.Equal(TargetCarrotAmount, player.Carrots);
    }
}