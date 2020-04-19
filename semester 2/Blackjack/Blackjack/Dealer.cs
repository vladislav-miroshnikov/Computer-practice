using System;
using System.Collections.Generic;
using static Blackjack.Action;

namespace Blackjack
{
    public class Dealer 
    {
        public List<Cards> List { get; set; }

        public Dealer()
        {
            List = new List<Cards>();
        }

        public void Strategy(List<Cards> cardsList)
        {
            int sum = Sum(List);
            while(sum < 17)
            {
                GetOneCard(List, cardsList);
                sum = Sum(List);
            }
        }

        public bool BlackjackCheck(AbstractPlayer bot) 
        {
            bool p = false;
            if (List[0].CardValue == 10 || (List[0].CardValue == 11))
            {
                if ((Sum(List) == 21) && (Sum(bot.List) != 21))
                {
                    p = true;
                }
                else if ((Sum(List) == 21) && (Sum(bot.List) == 21))
                {
                    bot.PlayerWallet += bot.Bet;
                    p = true;
                }
            }
            if (Sum(bot.List) == 21)
            {                
                bot.PlayerWallet += (int)(bot.Bet + bot.Bet * 3 / 2);
                p = true;
            }
            return p;
        }

        public void WinnerCheck(AbstractPlayer bot) 
        {            
            if ((Sum(List) > 21 && Sum(bot.List) <= 21) ||
               ((Sum(bot.List) <= 21 && Sum(List) <= 21) && (Sum(bot.List) > Sum(List))))
            {
               
                bot.PlayerWallet += bot.Bet * 2;

            }
        }
    }
}