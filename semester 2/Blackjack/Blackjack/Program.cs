using System;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            BotFisrt botFirst = new BotFisrt(1000, 0, "John");
            BotSecond botSecond = new BotSecond(1000, 0, "Conor");
            Dealer dealer = new Dealer();
            Deck myDeck = new Deck("Round table");
            myDeck.CreateCards();
            for (int i = 0; i < 400; i++)
            {
                myDeck.Game(botFirst, botSecond, dealer);
            }
            myDeck.GetInfo();
            botFirst.Info();
            botSecond.Info();
            Console.ReadKey();
        }
    }
}
