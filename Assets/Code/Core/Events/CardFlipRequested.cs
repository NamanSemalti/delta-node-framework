using CardMatch.Core.Domain.Card;

namespace CardMatch.Core.Events
{
    public sealed class CardFlipRequested
    {
        public Card Card { get; }

        public CardFlipRequested(Card card)
        {
            Card = card;
        }
    }
}