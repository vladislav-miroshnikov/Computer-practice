using System;
using System.Collections.Generic;

namespace Blackjack
{
    public abstract class AbstractPlayer : IBlackjack
    {
        protected Random rand = new Random();
        public string BotName { get; private set; }
        public List<Cards> PlayerList { get; set; }
        public int PlayerWallet { get; set; }
        public int Bet { get; set; }

        public int GamesCount { get; set; }
      
        public AbstractPlayer(int playerWallet, int gamesCount, string botName)
        {
            PlayerList = new List<Cards>();
            PlayerWallet = playerWallet;
            GamesCount = gamesCount;
            BotName = botName;
        }

        public abstract void ApplyStrategy(int dealerValue, List<Cards> cardsList);

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

        public void MakeBet()
        {
            if (PlayerWallet > 0)
            {
                Bet = rand.Next(1, (int)(0.05 * PlayerWallet));
                PlayerWallet -= Bet;
                //ставку, больше чем 5% от имеющейся суммы, сделать нельзя
            }
            else
            {
                Bet = 0;
            }

        }

        protected void Surrender()
        {
            PlayerWallet += (int)(Bet / 2);
            PlayerList.RemoveRange(0, PlayerList.Count);
        }

        protected void Double()
        {
            PlayerWallet -= Bet;
            Bet *= 2;
        }

        public void Info()
        {
            if (PlayerWallet <= 0)
            {
                Console.WriteLine($"{BotName} lost on the {GamesCount} game");
            }
            else
            {
                Console.WriteLine($"{BotName}'s bank is {PlayerWallet}");
            }
        }

    }
}