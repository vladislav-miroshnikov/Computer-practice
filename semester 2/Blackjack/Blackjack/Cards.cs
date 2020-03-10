namespace Blackjack
{
    public class Cards
    {
        public enum Suits
        {
            Hearts,
            Diamonds,
            Spades,
            Clubs
        }

        private Suits suit;
        public int cardValue { get; set; }

        public Cards(Suits _suit, int _cardValue)
        {
            suit = _suit;
            cardValue = _cardValue;
        }
        
        public static int[] values = new int[13] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };

        //public override string ToString()
        //{
        //    return string.Format("{0} of {1}", cardValue, suit);
        //}

    }
}
