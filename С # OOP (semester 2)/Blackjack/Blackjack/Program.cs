using System;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            BotFirst botFirst = new BotFirst(1000, 0, "John");
            BotSecond botSecond = new BotSecond(1000, 0, "Conor");
            Game game = new Game();
            
            for (int i = 0; i < 400; i++)
            {
                game.PlayGame(botFirst, botSecond);
            }
            
            botFirst.Info();
            botSecond.Info();
            Console.ReadKey();
        }
    }
}
