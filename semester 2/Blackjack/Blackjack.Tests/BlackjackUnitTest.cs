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
            BotFisrt botFirst = new BotFisrt(1000, 0);
            BotSecond botSecond = new BotSecond(1000, 0);
            Deck myDeck = new Deck();
            myDeck.CreateCards();
            myDeck.Game(botFirst, botSecond);
            botFirst.Info();
            botSecond.Info();
        }
    }
}
