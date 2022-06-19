namespace DotNetDesignPatternDemos.Behavioral.TemplateMethod
{
    using static System.Console;
    
    // Template without inheritance using void delegates and functions
    public static class GameTemplate
    {
        public static void Run(
          Action start,
          Action takeTurn,
          Func<bool> haveWinner,
          Func<int> winningPlayer)
        {
            start();
            while (!haveWinner())
                takeTurn();
            WriteLine($"Player {winningPlayer()} wins.");
        }
    }

    public class Demo2
    {
        static void Main(string[] args)
        {
            // No classes declarations
            var numberOfPlayers = 2;
            int currentPlayer = 0;
            int turn = 1, maxTurns = 10;

            // Functions that we will run in the template
            void Start()
            {
                WriteLine($"Starting a game of chess with {numberOfPlayers} players.");
            }

            bool HaveWinner()
            {
                return turn == maxTurns;
            }

            void TakeTurn()
            {
                WriteLine($"Turn {turn++} taken by player {currentPlayer}.");
                currentPlayer = (currentPlayer + 1) % numberOfPlayers;
            }

            int WinningPlayer()
            {
                return currentPlayer;
            }

            // Run the template
            GameTemplate.Run(Start, TakeTurn, HaveWinner, WinningPlayer);
        }
    }
}