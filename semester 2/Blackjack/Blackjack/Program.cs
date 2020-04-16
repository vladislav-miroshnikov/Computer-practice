using System;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            BotFisrt botFirst = new BotFisrt(1000, 0);
            BotSecond botSecond = new BotSecond(1000, 0);
            Deck myDeck = new Deck();
            myDeck.CreateCards();
            myDeck.Game(botFirst, botSecond);
            botFirst.Info();
            botSecond.Info();
            Console.ReadKey();

        }
    }
}
