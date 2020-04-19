using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blackjack;

namespace Blackjack.Tests
{
    [TestClass]
    public class BlackjackUnitTest
    {
        [TestMethod]
        public void BlackjackTest()
        {
            BotFisrt botFirst = new BotFisrt(1000, 0, "John");
            BotSecond botSecond = new BotSecond(1000, 0, "Conor");
            Dealer dealer = new Dealer();
            Deck myDeck = new Deck("Legendary table");
            myDeck.CreateCards();
            for (int i = 0; i < 400; i++)
            {
                myDeck.Game(botFirst, botSecond, dealer);
            }
            myDeck.GetInfo();
            botFirst.Info();
            botSecond.Info();

            Console.WriteLine();
            BotFisrt botMary = new BotFisrt(1000, 0, "Mary");
            BotSecond botKaty = new BotSecond(1000, 0, "Katy");
            Deck myDecks = new Deck("Champion table");
            myDecks.CreateCards();
            for (int i = 0; i < 400; i++)
            {
                myDecks.Game(botMary, botKaty, dealer);
            }
            myDecks.GetInfo();
            botMary.Info();
            botKaty.Info();
        }
    }
}
