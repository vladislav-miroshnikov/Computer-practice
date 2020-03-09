using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blackjack.Tests
{
    [TestClass]
    public class BlackjackUnitTest
    {
        [TestMethod]
        public void BlackjackTest()
        {
            BotFisrt bot1 = new BotFisrt(1000, 0);
            BotSecond bot2 = new BotSecond(1000, 0);
            Deck myDeck = new Deck();
            myDeck.CreateCards();
            myDeck.Game(bot1, bot2);
            bot1.Info();  //вывод в доп сведения
            bot2.Info();
        }
    }
}
