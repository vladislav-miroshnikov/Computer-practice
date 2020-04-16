using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Deck
    {
        public static List<Cards> CardsList { get; private set; }
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
       
        public void Game<T,U>(T botFirst, U botSecond) 
            where T: BotFisrt
            where U: BotSecond
        {
            Dealer dealer = new Dealer();
            for (int a = 0; a < 400; a++) 
            {
                if (CardsList.Count <= 52) //считаем, что когда останется 52 карты и менее-перемешиваем
                {
                    CreateCards();
                }

                if ((botFirst.PlayerWallet > 0) && (botSecond.PlayerWallet > 0)) //случай, когда оба бота в игре
                {
                    botFirst.GamesCount++;
                    botSecond.GamesCount++;                  
                    botFirst.MakeBet();                   
                    botSecond.MakeBet();                   
                    botFirst.GetTwoCard();
                    botSecond.GetTwoCard();                   
                    dealer.GetTwoCard();
                    if (dealer.BlackjackCheck(botFirst) == false)  //если вернется true,значит метод BlackjackCheck выполнился
                    {
                        botFirst.Strategy(dealer.List[0].СardValue);
                        dealer.Strategy();
                        dealer.WinnerCheck(botFirst);
                    }
                    if (dealer.BlackjackCheck(botSecond) == false)
                    {
                        botSecond.Strategy(dealer.List[0].СardValue);
                        dealer.Strategy();                                                                     
                        dealer.WinnerCheck(botSecond);
                    }
                    botFirst.List.RemoveRange(0, botFirst.List.Count);
                    botSecond.List.RemoveRange(0, botSecond.List.Count);
                    dealer.List.RemoveRange(0, dealer.List.Count);

                }
                else if ((botFirst.PlayerWallet > 0) && (botSecond.PlayerWallet <= 0)) //случай, когда только первый бот в игре
                {
                    botFirst.GamesCount++;
                    botFirst.MakeBet();
                    botFirst.GetTwoCard();                   
                    dealer.GetTwoCard();
                    if (dealer.BlackjackCheck(botFirst) == false)
                    {
                        botFirst.Strategy(dealer.List[0].СardValue);
                        dealer.Strategy();
                        dealer.WinnerCheck(botFirst);
                    }
                    botFirst.List.RemoveRange(0, botFirst.List.Count);                  
                    dealer.List.RemoveRange(0, dealer.List.Count);
                }
                else if ((botFirst.PlayerWallet <= 0) && (botSecond.PlayerWallet > 0)) //второй бот в игре
                {
                    botSecond.GamesCount++;
                    botSecond.MakeBet();
                    botSecond.GetTwoCard();
                    dealer.GetTwoCard();
                    if (dealer.BlackjackCheck(botSecond) == false)
                    {
                        botSecond.Strategy(dealer.List[0].СardValue);
                        dealer.Strategy();
                        dealer.WinnerCheck(botSecond);
                    }
                    botSecond.List.RemoveRange(0, botSecond.List.Count);
                    dealer.List.RemoveRange(0, dealer.List.Count);
                }
                else //оба выбыли
                {                   
                    break;
                }             
            }
           
        }
    }
}