using ConsoleApp2;
using ConsoleApp2.Player;
using Xunit;

namespace ISP.Tests.PlayerRankTests;

public class MovingPlayerRankTest
{
    private static int PlayerNotPlaying = -1;
    private static int PlayerNotPlayingRank = 0;
    public static IEnumerable<object[]> NotAllPlayersRankTestData => new List<object[]>
    {
        new object[] { new int[] { PlayerNotPlaying, PlayerNotPlaying, 10, 3 }, new int[] { PlayerNotPlaying, PlayerNotPlaying, 11, 12 }, new int[] {PlayerNotPlayingRank, PlayerNotPlayingRank, 2, 1} },
        new object[] { new int[] { PlayerNotPlaying, PlayerNotPlaying, PlayerNotPlaying, PlayerNotPlaying },new int[] { PlayerNotPlaying, PlayerNotPlaying,PlayerNotPlaying, 1 } ,new int[] { PlayerNotPlayingRank, PlayerNotPlayingRank, PlayerNotPlayingRank, 1 } },
        new object[] { new int[] { 1, 2, 3, PlayerNotPlaying }, new int[] { 3,PlayerNotPlaying ,4, PlayerNotPlaying } ,new int[] { 2, 3, 1, PlayerNotPlayingRank } },
        new object[]{ new int[] {PlayerNotPlaying, 31,PlayerNotPlaying, PlayerNotPlaying}, new int[] {PlayerNotPlaying, 32, PlayerNotPlaying,PlayerNotPlaying} ,new int[] { PlayerNotPlayingRank, 1 , PlayerNotPlayingRank, PlayerNotPlayingRank } },
    };
    
    [Theory]
    [MemberData(nameof(NotAllPlayersRankTestData))]

    public void NotAllPlayersRank_ShouldReturnCorrectValue(int[] playerStartSquares, int[] playerEndSquares,
        int[] ExpectedRanks)
    {
        Game state = new Game(NumberOfPlayers : 4);
        for (int i = 0; i < 4; i++)
            if (playerStartSquares[i] != PlayerNotPlaying)
                state.Board[playerStartSquares[i]].Player = state.Players[i];

        foreach (PlayerBase p in state.Players)
        {
            p.Carrots = 9999999;
        }
        
        for (int i = 0; i < 4; i++)
        {
            state.TurnIndex = i;
            if (playerEndSquares[i] != PlayerNotPlaying)
            {
                state.Move(new Move { SqureTargetIndex = playerEndSquares[i] });
            }
        }
        
        state.UpdateRank();
        List<int> playersRank = new();
        foreach (PlayerBase p in state.Players)
            playersRank.Add(p.Rank);
        
        Assert.Equal(ExpectedRanks, playersRank.ToArray());
    }
}