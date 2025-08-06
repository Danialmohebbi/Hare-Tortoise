using System.Collections.Generic;
using ConsoleApp2;
using ConsoleApp2.Extensions;
using ConsoleApp2.Player;
using Xunit;

namespace ISP.Tests.PlayerRankTests;

public class StaticBoardRankTest
{
    private static int PlayerNotPlaying = -1;
    private static int PlayerNotPlayingRank = 0;
    public static IEnumerable<object[]> StaticBoardRankTestData => new List<object[]>
    {
        new object[] { new int[] { 0, 1, 2, 3 }, new int[] { 4, 3, 2, 1 } },
        new object[] { new int[] { 5, 1, 2, 10 }, new int[] { 2, 4, 3, 1 } },
        new object[] { new int[] { 32, 1, 23, 31 }, new int[] { 1, 4, 3, 2 } },
        new object[] { new int[] { 12, 11, 1, 2 }, new int[] { 1, 2, 4, 3 } },
        new object[] { new int[] { 4, 11, 9, 33 }, new int[] { 4, 2, 3, 1 } },
        new object[] { new int[] { 33, 34, 22, 1 }, new int[] { 2, 1, 3, 4 } },

    };
    
    [Theory]
    [MemberData(nameof(StaticBoardRankTestData))]
    public void StaticBoardRank_ShouldReturnCorrectValue(int[] playerSquares, int[] ExpectedRanks)
    {
        GameState gameState = new GameState(NumberOfPlayers : 4);
        for (int i = 0; i < 4; i++)
            gameState.Board[playerSquares[i]].Player = gameState.Players[i];
        
        GameStateExtension.UpdateRank(gameState);
        List<int> playersRank = new();
        foreach (PlayerBase p in gameState.Players)
            playersRank.Add(p.Rank);
        
        Assert.Equal(ExpectedRanks, playersRank.ToArray());
    }

    
    public static IEnumerable<object[]> NotAllPlayersRankTestData => new List<object[]>
    {
        new object[] { new int[] { PlayerNotPlaying, PlayerNotPlaying, 10, 3 }, new int[] { PlayerNotPlayingRank, PlayerNotPlayingRank, 1, 2 } },
        new object[] { new int[] { PlayerNotPlaying, PlayerNotPlaying, PlayerNotPlaying, PlayerNotPlaying }, new int[] { PlayerNotPlayingRank, PlayerNotPlayingRank, PlayerNotPlayingRank, PlayerNotPlayingRank } },
        new object[] { new int[] { 2, 1, 3, PlayerNotPlaying }, new int[] { 2, 3, 1, PlayerNotPlayingRank } },
        new object[]{ new int[] {PlayerNotPlaying, 31,PlayerNotPlaying, PlayerNotPlaying}, new int[] { PlayerNotPlayingRank, 1 , PlayerNotPlayingRank, PlayerNotPlayingRank } },
        new object[]{ new int[] {PlayerNotPlaying, 31,21, PlayerNotPlaying}, new int[] { PlayerNotPlayingRank, 1 , 2, PlayerNotPlayingRank } },
        new object[]{ new int[] {21, PlayerNotPlaying,PlayerNotPlaying, 31}, new int[] { 2, PlayerNotPlayingRank , PlayerNotPlayingRank, 1 } }
    };
    [Theory]
    [MemberData(nameof(NotAllPlayersRankTestData))]
    public void NotAllPlayersRank_ShouldReturnCorrectValue(int[] playerSquares, int[] ExpectedRanks)
    {
        GameState gameState = new GameState(NumberOfPlayers : 4);
        for (int i = 0; i < 4; i++)
            if (playerSquares[i] != PlayerNotPlaying)
                gameState.Board[playerSquares[i]].Player = gameState.Players[i];
        
        GameStateExtension.UpdateRank(gameState);
        List<int> playersRank = new();
        foreach (PlayerBase p in gameState.Players)
            playersRank.Add(p.Rank);
        
        Assert.Equal(ExpectedRanks, playersRank.ToArray());
    }
}