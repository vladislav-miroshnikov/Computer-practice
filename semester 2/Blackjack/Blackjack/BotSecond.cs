using System.Diagnostics;

namespace Blackjack
{
    public class BotSecond : AbstractMan
    {

        public BotSecond(int _playerWallet, int _gamesCount)
        {
            playerWallet = _playerWallet;
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
            //использую ref, чтобы ставка перезаписалась
            playerWallet -= bet;
            bet *= 2;
        }

        public void Strategy(int dealerValue, ref int bet)
        {
            int sum = Sum();
            for (; ; ) //стратегия завершается, когда достигли break, более "агрессивная стратегия"
            {
                //describe the option "STAND"
                if (((dealerValue >= 2 && dealerValue <= 11) && (sum >= 17 && sum <= 21))
                    || ((dealerValue >= 2 && dealerValue <= 5) && (sum == 16))
                    || ((dealerValue >= 3 && dealerValue <= 4) && (sum >= 13 && sum <= 16))
                    || ((dealerValue >= 3 && dealerValue <= 4) && (sum == 15)))
                {
                    break;
                }
                // describe the option "SURRENDER"
                else if ((dealerValue == 11 && sum == 15)
                    || ((dealerValue >= 10 && dealerValue <= 11) && (sum == 16)))
                {
                    Surrender(bet);
                    break;
                }
                //describe the option "HIT"
                else if (((dealerValue >= 6 && dealerValue <= 9) && (sum == 16))
                    || ((dealerValue >= 5 && dealerValue <= 10) && (sum == 15))
                    || ((dealerValue >= 5 && dealerValue <= 11) && (sum >= 13 && sum <= 14))
                    || ((dealerValue >= 2 && dealerValue <= 11) && ((sum == 12) || (sum >= 4 && sum <= 9)))
                    || ((dealerValue >= 10 && dealerValue <= 11) && (sum == 10)))
                {
                    GetOneCard();
                }
                //describe the option "DOUBLE"
                else if (((dealerValue == 2) && (sum == 15))
                    || ((dealerValue >= 2 && dealerValue <= 4) && (sum >= 13 && sum <= 14))
                    || ((dealerValue >= 2 && dealerValue <= 11) && (sum == 11))
                    || ((dealerValue >= 2 && dealerValue <= 9) && (sum == 10)))
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
