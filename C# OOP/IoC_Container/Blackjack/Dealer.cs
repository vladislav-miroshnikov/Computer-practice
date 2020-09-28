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
            if (List[0].СardValue == 10 || (List[0].СardValue == 11))
            {
                if ((Sum() == 21) && (bot.Sum() != 21))
                {
                    p = true;
                }
                else if ((Sum() == 21) && (bot.Sum() == 21))
                {
                    bot.PlayerWallet += bot.Bet;
                    p = true;
                }
            }
            if (bot.Sum() == 21)
            {
                bot.PlayerWallet += (int)(bot.Bet + bot.Bet * 3 / 2);
                p = true;
            }
            return p;
        }

        public void WinnerCheck<T>(T bot) where T : AbstractMan
        {
            if ((Sum() > 21 && bot.Sum() <= 21) ||
               ((bot.Sum() <= 21 && Sum() <= 21) && (bot.Sum() > Sum())))
            {
                bot.PlayerWallet += bot.Bet * 2;
            }
        }
    }
}