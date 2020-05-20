using System.Linq;

namespace Blackjack
{
    public class Game
    {
        private Deck deck;

        private Dealer dealer;

        public Game()
        {
            deck = new Deck();
            dealer = new Dealer();
            deck.CreateCards();
        }

        public void PlayGame(AbstractPlayer playerFirst, AbstractPlayer playerSecond)
        {   //принимает AbstractPlayer, чтобы избавиться от специфичных параметров, и в метод мог приходить любого вида бот
            
            if (deck.CardsList.Count <= 52) //считаем, что когда останется 52 карты и менее-перемешиваем
            {
                deck.CreateCards();
            }

            playerFirst.MakeBet();
            playerSecond.MakeBet();
            if (playerFirst.Bet != 0 || playerSecond.Bet != 0)
            {
                dealer.GetTwoCard(dealer.DealerList, deck.CardsList);
                if (playerFirst.Bet != 0)
                {
                    playerFirst.GamesCount++;
                    playerFirst.GetTwoCard(playerFirst.PlayerList, deck.CardsList);
                    if (BlackjackCheck(playerFirst) == false)  //если вернется true,значит метод BlackjackCheck выполнился
                    {
                        playerFirst.ApplyStrategy(dealer.DealerList[0].CardValue, deck.CardsList);
                        dealer.ApplyStrategy(deck.CardsList);
                        WinnerCheck(playerFirst);
                    }
                    playerFirst.PlayerList.RemoveRange(0, playerFirst.PlayerList.Count);
                }
                if (playerSecond.Bet != 0)
                {
                    playerSecond.GamesCount++;
                    playerSecond.GetTwoCard(playerSecond.PlayerList, deck.CardsList);
                    if (BlackjackCheck(playerSecond) == false)
                    {
                        playerSecond.ApplyStrategy(dealer.DealerList[0].CardValue, deck.CardsList);
                        dealer.ApplyStrategy(deck.CardsList);
                        WinnerCheck(playerSecond);
                    }
                    playerSecond.PlayerList.RemoveRange(0, playerSecond.PlayerList.Count);
                }
                dealer.DealerList.Clear();
            }
        }

        private bool BlackjackCheck(AbstractPlayer bot)
        {
            bool p = false;
            if (dealer.DealerList[0].CardValue == 10 || (dealer.DealerList[0].CardValue == 11))
            {
                if ((dealer.DealerList.Sum(x => x.CardValue) == 21) && (bot.PlayerList.Sum(x => x.CardValue) != 21))
                {
                    p = true;
                }
                else if ((dealer.DealerList.Sum(x => x.CardValue) == 21) && (bot.PlayerList.Sum(x => x.CardValue) == 21))
                {
                    bot.PlayerWallet += bot.Bet;
                    p = true;
                }
            }
            if (bot.PlayerList.Sum(x => x.CardValue) == 21)
            {
                bot.PlayerWallet += (int)(bot.Bet + bot.Bet * 3 / 2);
                p = true;
            }
            return p;
    
        }

        private void WinnerCheck(AbstractPlayer bot)
        {
            if ((dealer.DealerList.Sum(x => x.CardValue) > 21 && bot.PlayerList.Sum(x => x.CardValue) <= 21) ||
               ((bot.PlayerList.Sum(x => x.CardValue) <= 21 && dealer.DealerList.Sum(x => x.CardValue) <= 21) && 
               (bot.PlayerList.Sum(x => x.CardValue) > dealer.DealerList.Sum(x => x.CardValue)))) 
            {
                bot.PlayerWallet += bot.Bet * 2;
            }
        }
    }
}
