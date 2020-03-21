namespace Blackjack
{
    class Dealer : AbstractMan
    {
        public void Strategy()
        {
            int sum = Sum();
            while (sum < 17)
            {
                GetOneCard();
                sum = Sum();
            }
        }

        public bool BlackjackCheck<T>(T bot) where T : AbstractMan
        {
            bool p = false;
            if (list[0].cardValue == 10 || (list[0].cardValue == 11))
            {
                if ((Sum() == 21) && (bot.Sum() != 21))
                {
                    p = true;
                }
                else if ((Sum() == 21) && (bot.Sum() == 21))
                {
                    bot.playerWallet += bot.bet;
                    p = true;
                }
            }
            if (bot.Sum() == 21)
            {
                bot.playerWallet += (int)(bot.bet + bot.bet * 3 / 2);
                p = true;
            }
            return p;
        }

        public void WinnerCheck<T>(T bot) where T : AbstractMan
        {
            if ((Sum() > 21 && bot.Sum() <= 21) ||
               ((bot.Sum() <= 21 && Sum() <= 21) && (bot.Sum() > Sum())))
            {
                bot.playerWallet += bot.bet * 2;
            }
        }
    }
}