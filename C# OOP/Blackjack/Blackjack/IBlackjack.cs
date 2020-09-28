using System.Collections.Generic;

namespace Blackjack
{
    public interface IBlackjack
    {
        void GetTwoCard(List<Cards> list, List<Cards> cardsList);
        void GetOneCard(List<Cards> list, List<Cards> cardsList);
    }
}
