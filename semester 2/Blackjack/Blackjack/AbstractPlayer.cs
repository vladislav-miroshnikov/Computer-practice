using System;
using System.Collections.Generic;

namespace Blackjack
{
    public abstract class AbstractPlayer
    {
        protected Random rand = new Random();
        public string BotName { get; private set; }
        public List<Cards> List { get; set; }
        public int PlayerWallet { get; set; }
        public int Bet { get; set; }

        public int GamesCount { get; set; }
      
        public AbstractPlayer(int playerWallet, int gamesCount, string botName)
        {
            List = new List<Cards>();
            PlayerWallet = playerWallet;
            GamesCount = gamesCount;
            BotName = botName;
        }

        public abstract void Strategy(int dealerValue, List<Cards> cardsList);

        public void MakeBet()
        {
            if (PlayerWallet > 0)
            {
                for (; ; )
                {
                    Bet = rand.Next(1, 50);
                    if (Bet <= PlayerWallet)   //проверка, если вдруг сгенерируется ставка, превосходящая имеющиеся деньги на руках
                    {
                        PlayerWallet -= Bet;
                        break;
                    }

                }
            }
            else
            {
                Bet = 0;
            }

        }

        protected void Surrender()
        {
            PlayerWallet += (int)(Bet / 2);
            List.RemoveRange(0, List.Count);
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