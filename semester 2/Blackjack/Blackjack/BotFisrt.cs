using System;
using System.Diagnostics;

namespace Blackjack
{
    public class BotFisrt : AbstractMan
    {

        public BotFisrt(int _playerWallet, int _gamesCount)
        {
            playerWallet = _playerWallet;
            gamesCount = _gamesCount;
        }

        public int gamesCount { get; set; }

        public void MakeBet()
        {
            for (; ;)
            {
                bet = rand.Next(1, 50);
                if (bet <= playerWallet)   //проверка, если вдруг сгенерируется ставка, превосходящая имеющиеся деньги на руках
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
            for (; ; )  //стратегия завершается, когда достигли break
            {
                //describe the option "STAND"
                if (((value >= 2 && value <= 11) && (sum >= 18 && sum <= 21))
                    || ((value >= 2 && value <= 10) && (sum == 17))
                    || ((value >= 2 && value <= 6) && (sum >= 13 && sum <= 16))
                    || ((value >= 4 && value <= 6) && (sum == 12)))
                {
                    break;                 
                }
                // describe the option "SURRENDER"
                else if ((value == 11 && sum == 17)
                    || ((value >= 10 && value <= 11) && (sum >= 15 && sum <= 16)))
                {
                    Surrender(bet);
                    break;                   
                }
                //describe the option "HIT"
                else if (((value >= 7 && value <= 9) && (sum >= 15 && sum <= 16))
                    || ((value >= 7 && value <= 11) && (sum >= 12 && sum <= 14))
                    || ((value >= 2 && value <= 3) && (sum == 12))
                    || ((value >= 10 && value <= 11) && (sum == 10))
                    || ((value >= 7 && value <= 11) && (sum == 9))
                    || ((value == 2) && (sum == 9))
                    || ((value >= 2 && value <= 11) && (sum >= 4 && sum <= 8)))
                {
                    GetOneCard();                   
                }
                //describe the option "DOUBLE"
                else if (((value >= 2 && value <= 11) && (sum == 11))
                    || ((value >= 2 && value <= 9) && (sum == 10))
                    || ((value >= 3 && value <= 6) && (sum == 9))) 
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
                Debug.WriteLine($"bot1 lost on the {gamesCount} game");
            }
            else
            {
                Debug.WriteLine($"bot1 bank is {playerWallet}");
            }
        }
    }
}
