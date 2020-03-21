using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Deck
    {
        public static List<Cards> cards { get; set; }
        private Random rand = new Random();

        public void CreateCards()
        {
            cards = new List<Cards>();
            for (int j = 0; j < 8; j++)
            {

                foreach (int s in Cards.values)
                {
                    cards.Add(new Cards(Cards.Suits.Hearts, s));
                }

                foreach (int s in Cards.values)
                {
                    cards.Add(new Cards(Cards.Suits.Diamonds, s));


                }

                foreach (int s in Cards.values)
                {
                    cards.Add(new Cards(Cards.Suits.Spades, s));


                }

                foreach (int s in Cards.values)
                {
                    cards.Add(new Cards(Cards.Suits.Clubs, s));

                }

            }

            for (int o = cards.Count - 1; o >= 1; o--)
            {
                int j = rand.Next(o + 1);
                Cards tmp = cards[j];
                cards[j] = cards[o];
                cards[o] = tmp;
            }

        }

        //public void Bring()
        //{
        //    for (int i = 0; i < 416; i++)
        //    {
        //        Console.WriteLine(cards[i]);
        //    }
        //    Console.WriteLine("all");
        //}

        public void Game<T, U>(T botFirst, U botSecond)
            where T : BotFisrt
            where U : BotSecond
        {
            Dealer dealer = new Dealer();
            for (int a = 0; a < 400; a++)
            {
                if (cards.Count <= 52) //считаем, что когда останется 52 карты и менее-перемешиваем
                {
                    CreateCards();
                }

                if ((botFirst.playerWallet > 0) && (botSecond.playerWallet > 0)) //случай, когда оба бота в игре
                {
                    botFirst.gamesCount++;
                    botSecond.gamesCount++;
                    botFirst.MakeBet();
                    botSecond.MakeBet();
                    botFirst.GetTwoCard();
                    botSecond.GetTwoCard();
                    dealer.GetTwoCard();
                    if (dealer.BlackjackCheck(botFirst) == false)  //если вернется true,значит метод BlackjackCheck выполнился
                    {
                        botFirst.Strategy(dealer.list[0].cardValue, ref botFirst.bet);
                        dealer.Strategy();
                        dealer.WinnerCheck(botFirst);
                    }
                    if (dealer.BlackjackCheck(botSecond) == false)
                    {
                        botSecond.Strategy(dealer.list[0].cardValue, ref botSecond.bet);
                        dealer.Strategy();
                        dealer.WinnerCheck(botSecond);
                    }
                    botFirst.list.RemoveRange(0, botFirst.list.Count);
                    botSecond.list.RemoveRange(0, botSecond.list.Count);
                    dealer.list.RemoveRange(0, dealer.list.Count);

                }
                else if ((botFirst.playerWallet > 0) && (botSecond.playerWallet <= 0)) //случай, когда только первый бот в игре
                {
                    botFirst.gamesCount++;
                    botFirst.MakeBet();
                    botFirst.GetTwoCard();
                    dealer.GetTwoCard();
                    if (dealer.BlackjackCheck(botFirst) == false)
                    {
                        botFirst.Strategy(dealer.list[0].cardValue, ref botFirst.bet);
                        dealer.Strategy();
                        dealer.WinnerCheck(botFirst);
                    }
                    botFirst.list.RemoveRange(0, botFirst.list.Count);
                    dealer.list.RemoveRange(0, dealer.list.Count);
                }
                else if ((botFirst.playerWallet <= 0) && (botSecond.playerWallet > 0)) //второй бот в игре
                {
                    botSecond.gamesCount++;
                    botSecond.MakeBet();
                    botSecond.GetTwoCard();
                    dealer.GetTwoCard();
                    if (dealer.BlackjackCheck(botSecond) == false)
                    {
                        botSecond.Strategy(dealer.list[0].cardValue, ref botSecond.bet);
                        dealer.Strategy();
                        dealer.WinnerCheck(botSecond);
                    }
                    botSecond.list.RemoveRange(0, botSecond.list.Count);
                    dealer.list.RemoveRange(0, dealer.list.Count);
                }
                else //оба выбыли
                {
                    break;
                }
            }

        }
    }
}