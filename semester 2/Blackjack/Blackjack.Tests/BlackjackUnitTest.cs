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
            BotFirst botJohn = new BotFirst(1000, 0, "John");
            BotSecond botConor = new BotSecond(1000, 0, "Conor");
            Dealer dealer = new Dealer();
            Game gameFirst = new Game();
            for (int i = 0; i < 400; i++)
            {
                gameFirst.PlayGame(botJohn, botConor);
            }

            botJohn.Info();
            botConor.Info();

            Console.WriteLine();
            BotFirst botMary = new BotFirst(1000, 0, "Mary");
            BotSecond botKaty = new BotSecond(1000, 0, "Katy");
            Game gameSecond = new Game();
            for (int i = 0; i < 400; i++)
            {
                gameSecond.PlayGame(botMary, botKaty);
            }
          
            botMary.Info();
            botKaty.Info();
        }
    }
}
