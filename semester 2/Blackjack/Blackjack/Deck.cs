using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Deck
    {
        public List<Cards> CardsList { get; private set; }

        private Random rand = new Random();

        public void CreateCards()
        {
            CardsList = new List<Cards>();         
            for (int j = 0; j < 8; j++) 
            {

                foreach (int s in Cards.values)
                {
                    CardsList.Add(new Cards(Cards.Suits.Hearts, s));               
                }

                foreach (int s in Cards.values)
                {
                    CardsList.Add(new Cards(Cards.Suits.Diamonds, s));                   
                }

                foreach (int s in Cards.values)
                {
                    CardsList.Add(new Cards(Cards.Suits.Spades, s));                   
                }

                foreach (int s in Cards.values)
                {
                    CardsList.Add(new Cards(Cards.Suits.Clubs, s));                   
                }
                
            }

            //shuffle
            for (int o = CardsList.Count - 1; o >= 1; o--)
            {
                int j = rand.Next(o + 1);
                Cards tmp = CardsList[j];
                CardsList[j] = CardsList[o];
                CardsList[o] = tmp;
            }

        }

        //public void ShowCards()
        //{
        //    for (int i = 0; i < 416; i++)
        //    {
        //        Console.WriteLine(CardsList[i]);
        //    }
        //    Console.WriteLine("all");
        //}
    }
}