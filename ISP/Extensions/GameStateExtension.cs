using ConsoleApp2.Player;
using ConsoleApp2.Squares;

namespace ConsoleApp2.Extensions
{

    public static class GameStateExtension
    {
        public static void ApplyMove(this GameState gameState, Move move)
        {
            move.gameState = gameState;
            Move(gameState, move.SqureTargetIndex, move.EatCarrots);
        }

        public static void Move(GameState gameState, int squareTargetIndex, bool EatCarrots = false)
        {

            var player = gameState.Players[gameState.TurnIndex];
            bool movingAway = player.CurrentSquare != squareTargetIndex;

            if (player.SkipRound)
            {
                player.SkipRound = false;
                return;
            }

            if (gameState.Board[player.CurrentSquare].GetType() == typeof(NumberSquare) && movingAway)
                player.ExecuteCommand();
            else if (gameState.Board[player.CurrentSquare].GetType() == typeof(LettuceSquare) && !player.RequiredToMove)
            {
                player.ExecuteCommand();
                return;
            }
            else if (gameState.Board[player.CurrentSquare].GetType() == typeof(CarrotSquare) && !movingAway)
            {
                player.TakeCarrots = EatCarrots && player.CurrentSquare >= 6 ? false : true;
                player.ExecuteCommand();
                player.SetCommand(gameState.Board[player.CurrentSquare].GetCommand(gameState));
            }


            if (player.RequiredToMove && !movingAway)
                throw new Exception();

            int move = int.Abs(player.CurrentSquare - squareTargetIndex);
            if (player.CurrentSquare == 0)
                move++;
            int cost = (move * (move + 1)) / 2;
            if (player.Carrots >= cost && movingAway)
            {
                if (gameState.Board[squareTargetIndex].GetType() == typeof(TortoiseSquare)
                    && gameState.Board[squareTargetIndex].Player == null
                    && squareTargetIndex < player.CurrentSquare)
                {
                    (gameState.Board[squareTargetIndex] as TortoiseSquare).SetMoves(move);
                    player.SetCommand(gameState.Board[squareTargetIndex].GetCommand(gameState));
                    gameState.Board[player.CurrentSquare].Player = null;
                    gameState.Board[squareTargetIndex].Player = player;
                    player.CurrentSquare = squareTargetIndex;
                    Console.WriteLine("hey");
                }
                else
                {
                    player.Carrots -= cost;
                    gameState.Board[player.CurrentSquare].Player = null;
                    player.CurrentSquare = squareTargetIndex;
                    gameState.Board[squareTargetIndex].Player = player;
                    player.SetCommand(gameState.Board[squareTargetIndex].GetCommand(gameState));
                    player.RequiredToMove = false;
                    player.CarrotsUsedLastTurn = cost;
                }

                if (gameState.Board[player.CurrentSquare].GetType() == typeof(HareSquare)
                    || gameState.Board[player.CurrentSquare].GetType() == typeof(TortoiseSquare))
                    player.ExecuteCommand();
                return;
            }else if (player.Carrots < cost)
                Console.WriteLine("Yup");

            UpdateRank(gameState);
        }

        public static void UpdateRank(GameState gameState)
        {
            List<PlayerBase> players = new();
            foreach (var square in gameState.Board)
            {
                if (square.Player != null)
                    players.Add(square.Player);
            }

            for (int i = players.Count - 1, j = 0; i >= 0; i--, j++)
            {
                players[j].Rank = i + 1;
            }
        }
    }
}