using System;
using System.Collections.Generic;
using static Blackjack.Action;

namespace Blackjack
{
    public class Deck
    {
        public List<Cards> CardsList { get; private set; }

        private Random rand = new Random();
        public string DeckName { get; private set; }

        public Deck(string deckName)
        {
            DeckName = deckName;
        }

        public void GetInfo()
        {
            Console.WriteLine($"Name of deck: {DeckName}");
        }

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

            for (int o = CardsList.Count - 1; o >= 1; o--)
            {
                int j = rand.Next(o + 1);
                Cards tmp = CardsList[j];
                CardsList[j] = CardsList[o];
                CardsList[o] = tmp;
            }

        }

        //public void Bring()
        //{
        //    for (int i = 0; i < 416; i++)
        //    {
        //        Console.WriteLine(CardsList[i]);
        //    }
        //    Console.WriteLine("all");
        //}

  
        public void Game(AbstractPlayer playerFirst, AbstractPlayer playerSecond, Dealer dealer) 
        {   //принимает AbstractPlayer, чтобы избавиться от специфичных параметров, и в метод мог приходить любой бот
            if (CardsList.Count <= 52) //считаем, что когда останется 52 карты и менее-перемешиваем
            {
                CreateCards();
            }

            playerFirst.MakeBet();
            playerSecond.MakeBet();
            if (playerFirst.Bet != 0 || playerSecond.Bet != 0)
            {
                GetTwoCard(dealer.List, CardsList);
                if (playerFirst.Bet != 0)
                {
                    playerFirst.GamesCount++;
                    GetTwoCard(playerFirst.List, CardsList);
                    if (dealer.BlackjackCheck(playerFirst) == false)  //если вернется true,значит метод BlackjackCheck выполнился
                    {
                        playerFirst.Strategy(dealer.List[0].CardValue, CardsList);
                        dealer.Strategy(CardsList);
                        dealer.WinnerCheck(playerFirst);
                    }
                    playerFirst.List.RemoveRange(0, playerFirst.List.Count);
                }
                if (playerSecond.Bet != 0)
                {
                    playerSecond.GamesCount++;
                    GetTwoCard(playerSecond.List, CardsList);
                    if (dealer.BlackjackCheck(playerSecond) == false)
                    {
                        playerSecond.Strategy(dealer.List[0].CardValue, CardsList);
                        dealer.Strategy(CardsList);
                        dealer.WinnerCheck(playerSecond);
                    }
                    playerSecond.List.RemoveRange(0, playerSecond.List.Count);
                }
                dealer.List.Clear();
            }
        }
    }
}