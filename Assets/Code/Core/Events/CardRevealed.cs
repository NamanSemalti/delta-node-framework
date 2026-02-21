using CardMatch.Core.Domain.Card;

namespace CardMatch.Core.Events
{
    public sealed class CardRevealed
    {
        public Card Card { get; }

        public CardRevealed(Card card)
        {
            Card = card;
        }
    }
}