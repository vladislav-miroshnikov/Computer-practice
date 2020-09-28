using System;

namespace Blackjack
{
    public class BotFisrt : AbstractMan
    {

        public BotFisrt(int playerWallet, int gamesCount, Deck deck)
        {
            PlayerWallet = playerWallet;
            GamesCount = gamesCount;
            Deck = deck;
        }

        public int GamesCount { get; set; }

        public void MakeBet()
        {
            for (; ; )
            {
                Bet = rand.Next(1, 50);
                if (Bet <= PlayerWallet)   //проверка, если вдруг сгенерируется ставка, превосходящая имеющиеся деньги на руках
                {
                    PlayerWallet -= Bet;
                    break;
                }

            }

        }

        private void Surrender()
        {
            PlayerWallet += (int)(Bet / 2);
            List.RemoveRange(0, List.Count);
        }

        private void Double()
        {
            PlayerWallet -= Bet;
            Bet *= 2;
        }

        public void Strategy(int dealerValue)
        {
            int sum = Sum();
            for (; ; )  //стратегия завершается, когда достигли break
            {
                //describe the option "STAND"
                if (((dealerValue >= 2 && dealerValue <= 11) && (sum >= 18 && sum <= 21))
                    || ((dealerValue >= 2 && dealerValue <= 10) && (sum == 17))
                    || ((dealerValue >= 2 && dealerValue <= 6) && (sum >= 13 && sum <= 16))
                    || ((dealerValue >= 4 && dealerValue <= 6) && (sum == 12)))
                {
                    break;
                }
                // describe the option "SURRENDER"
                else if ((dealerValue == 11 && sum == 17)
                    || ((dealerValue >= 10 && dealerValue <= 11) && (sum >= 15 && sum <= 16)))
                {
                    Surrender();
                    break;
                }
                //describe the option "HIT"
                else if (((dealerValue >= 7 && dealerValue <= 9) && (sum >= 15 && sum <= 16))
                    || ((dealerValue >= 7 && dealerValue <= 11) && (sum >= 12 && sum <= 14))
                    || ((dealerValue >= 2 && dealerValue <= 3) && (sum == 12))
                    || ((dealerValue >= 10 && dealerValue <= 11) && (sum == 10))
                    || ((dealerValue >= 7 && dealerValue <= 11) && (sum == 9))
                    || ((dealerValue == 2) && (sum == 9))
                    || ((dealerValue >= 2 && dealerValue <= 11) && (sum >= 4 && sum <= 8)))
                {
                    GetOneCard();
                }
                //describe the option "DOUBLE"
                else if (((dealerValue >= 2 && dealerValue <= 11) && (sum == 11))
                    || ((dealerValue >= 2 && dealerValue <= 9) && (sum == 10))
                    || ((dealerValue >= 3 && dealerValue <= 6) && (sum == 9)))
                {
                    Double();
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
            if (PlayerWallet <= 0)
            {
                Console.WriteLine($"bot1 lost on the {GamesCount} game");
            }
            else
            {
                Console.WriteLine($"bot1 bank is {PlayerWallet}");
            }
        }
    }
}
