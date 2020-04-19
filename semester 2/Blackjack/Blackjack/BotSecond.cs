using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Blackjack.Action;

namespace Blackjack
{
    public class BotSecond : AbstractPlayer
    {

        public BotSecond(int playerWallet, int gamesCount, string botName)
            :base(playerWallet, gamesCount, botName)
        {
            
        }

        public override void Strategy(int dealerValue, List<Cards> cardsList)
        {
            int sum = Sum(List);
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
                    Surrender();
                    break;
                }
                //describe the option "HIT"
                else if (((dealerValue >= 6 && dealerValue <= 9) && (sum == 16))
                    || ((dealerValue >= 5 && dealerValue <= 10) && (sum == 15))
                    || ((dealerValue >= 5 && dealerValue <= 11) && (sum >= 13 && sum <= 14))
                    || ((dealerValue >= 2 && dealerValue <= 11) && ((sum == 12) || (sum >= 4 && sum <= 9)))
                    || ((dealerValue >= 10 && dealerValue <= 11) && (sum == 10)))
                {
                    GetOneCard(List, cardsList);
                }
                //describe the option "DOUBLE"
                else if (((dealerValue == 2) && (sum == 15))
                    || ((dealerValue >= 2 && dealerValue <= 4) && (sum >= 13 && sum <= 14))
                    || ((dealerValue >= 2 && dealerValue <= 11) && (sum == 11))
                    || ((dealerValue >= 2 && dealerValue <= 9) && (sum == 10)))
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