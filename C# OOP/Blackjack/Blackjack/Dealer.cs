using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{
    public class Dealer : IBlackjack
    {
        private Random rand = new Random();
        public List<Cards> DealerList { get; set; }

        public Dealer()
        {
            DealerList = new List<Cards>();
        }

        public void ApplyStrategy(List<Cards> cardsList)
        {
            int sum = DealerList.Sum(x => x.CardValue);
            while (sum < 17)
            {
                GetOneCard(DealerList, cardsList);
                sum = DealerList.Sum(x => x.CardValue);
            }
        }

        public void GetTwoCard(List<Cards> list, List<Cards> cardsList)
        {
            for (int p = 0; p < 2; p++)
            {
                int y = cardsList.Count;
                int i = rand.Next(0, y);
                list.Add(cardsList[i]);
                cardsList.RemoveAt(i);
            }
        }

        public void GetOneCard(List<Cards> list, List<Cards> cardsList)
        {
            int y = cardsList.Count;
            int i = rand.Next(0, y);
            list.Add(cardsList[i]);
            cardsList.RemoveAt(i);
        }

    }
}