using System.Diagnostics;

namespace Blackjack
{
    public class BotSecond : AbstractMan
    {

        public BotSecond(int _sum, int _gamesCount)
        {
            playerWallet = _sum;
            gamesCount = _gamesCount;
        }

        public int gamesCount { get; set; }

        public void MakeBet()
        {
            for (; ; )
            {
                bet = rand.Next(1, 50);
                if (bet <= playerWallet)  //проверка, если вдруг сгенерируется ставка, превосходящая имеющиеся деньги на руках
                {
                    playerWallet -= bet;
                    break;
                }
               
            }

        }

        private void Surrender(int bet)
        {
            playerWallet += (int)(bet / 2);
            list.RemoveRange(0, list.Count);
        }

        private void Double(ref int bet)
        {
            //используем ref, чтобы ставка перезаписалась
            playerWallet -= bet;
            bet *= 2;
        }

        public void Strategy(int value, ref int bet)
        {
            int sum = Sum();
            for (; ; ) //стратегия завершается, когда достигли break, более "агрессивная стратегия"
            {
                //describe the option "STAND"
                if (((value >= 2 && value <= 11) && (sum >= 17 && sum <= 21))
                    || ((value >= 2 && value <= 5) && (sum == 16))
                    || ((value >= 3 && value <= 4) && (sum >= 13 && sum <= 16))
                    || ((value >= 3 && value <= 4) && (sum == 15)))
                {
                    break;
                }
                // describe the option "SURRENDER"
                else if ((value == 11 && sum == 15)
                    || ((value >= 10 && value <= 11) && (sum == 16)))
                {
                    Surrender(bet);
                    break;
                }
                //describe the option "HIT"
                else if (((value >= 6 && value <= 9) && (sum == 16))
                    || ((value >= 5 && value <= 10) && (sum == 15))
                    || ((value >= 5 && value <= 11) && (sum >= 13 && sum <= 14))
                    || ((value >= 2 && value <= 11) && ((sum == 12) || (sum >= 4 && sum <= 9)))
                    || ((value >= 10 && value <= 11) && (sum == 10)))
                {
                    GetOneCard();
                }
                //describe the option "DOUBLE"
                else if (((value == 2) && (sum == 15))
                    || ((value >= 2 && value <= 4) && (sum >= 13 && sum <= 14))
                    || ((value >= 2 && value <= 11) && (sum == 11))
                    || ((value >= 2 && value <= 9) && (sum == 10)))
                {
                    Double(ref bet);
                    GetOneCard();
                    break;
                }

                sum = Sum();
                if (sum > 21)
                {
                    break;
                }
            }
        }
        public void Info()
        {
            if (playerWallet <= 0) 
            {
                Debug.WriteLine($"bot2 lost on the {gamesCount} game");
            }
            else
            {
                Debug.WriteLine($"bot2 bank is {playerWallet}");
            }
        }
    }
}
