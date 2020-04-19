using System;
using System.Collections.Generic;

namespace Blackjack
{
    class Action
    {
        //создан дополнительный класс, поскольку данные методы общие и для ботов и для дилера, 
        //а дилер больше не наследник AbstractPlayer
        private static Random rand = new Random();

        public static void GetTwoCard(List<Cards> list, List<Cards> cardsList)
        {
            for (int p = 0; p < 2; p++)
            {
                int y = cardsList.Count;
                int i = rand.Next(0, y);
                list.Add(cardsList[i]);
                cardsList.RemoveAt(i);
            }
        }

        public static void GetOneCard(List<Cards> list, List<Cards> cardsList)
        {
            int y = cardsList.Count;
            int i = rand.Next(0, y);
            list.Add(cardsList[i]);
            cardsList.RemoveAt(i);
        }

        public static int Sum(List<Cards> list)
        {
            int sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i].CardValue;
            }
            return sum;
        }
    }
}
