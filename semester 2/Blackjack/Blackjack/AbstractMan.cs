using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{
    public abstract class AbstractMan
    {
        protected Random rand = new Random();
        public List<Cards> list { get; set; }
        public int playerWallet { get; set; }
        public int bet;
        public void GetTwoCard()
        {
            list = new List<Cards>();
            for (int p = 0; p < 2; p++)
            {
                int y = Deck.cards.Count();
                int i = rand.Next(0, y);
                list.Add(Deck.cards[i]);
                Deck.cards.RemoveAt(i);
            }
        }

        public void GetOneCard()
        {
            int y = Deck.cards.Count();
            int i = rand.Next(0, y);
            list.Add(Deck.cards[i]);
            Deck.cards.RemoveAt(i);
        }

        public int Sum()
        {
            int sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i].cardValue;
            }
            return sum;
        }
       
    }
}
