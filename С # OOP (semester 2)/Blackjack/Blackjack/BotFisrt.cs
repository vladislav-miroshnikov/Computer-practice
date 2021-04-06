using System.Collections.Generic;
using static Blackjack.Action;

namespace Blackjack
{
    public class BotFisrt : AbstractPlayer
    {

        public BotFisrt(int playerWallet, int gamesCount, string botName)
            :base(playerWallet, gamesCount, botName)
        {
            
        }

      
        public override void Strategy(int dealerValue, List<Cards> cardsList)
        {
            int sum = Sum(List);
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
                    GetOneCard(List, cardsList);                   
                }
                //describe the option "DOUBLE"
                else if (((dealerValue >= 2 && dealerValue <= 11) && (sum == 11))
                    || ((dealerValue >= 2 && dealerValue <= 9) && (sum == 10))
                    || ((dealerValue >= 3 && dealerValue <= 6) && (sum == 9))) 
                {
                    Double();
                    GetOneCard(List, cardsList);
                    break;                   
                }

                sum = Sum(List);
                if (sum > 21)
                {
                    break;                  
                }
            }  
        }
    }
}