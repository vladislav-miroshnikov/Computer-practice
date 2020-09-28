using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{
    public abstract class AbstractMan
    {
        protected Random rand = new Random();
        public List<Cards> List { get; private set; }

        public Deck Deck;
        public int PlayerWallet { get; set; }
        public int Bet { get; set; }
        public void GetTwoCard()
        {
            List = new List<Cards>();
            for (int p = 0; p < 2; p++)
            {
                int y = Deck.CardsList.Count();
                int i = rand.Next(0, y);
                List.Add(Deck.CardsList[i]);
                Deck.CardsList.RemoveAt(i);
            }
        }

        public void GetOneCard()
        {
            int y = Deck.CardsList.Count();
            int i = rand.Next(0, y);
            List.Add(Deck.CardsList[i]);
            Deck.CardsList.RemoveAt(i);
        }

        public int Sum()
        {
            int sum = 0;
            for (int i = 0; i < List.Count; i++)
            {
                sum += List[i].СardValue;
            }
            return sum;
        }

    }
}