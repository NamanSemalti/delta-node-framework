namespace CardMatch.Core.Events
{
    public sealed class CardFlipStarted
    {
        public CardMatch.Core.Domain.Card.Card Card { get; }

        public CardFlipStarted(CardMatch.Core.Domain.Card.Card card)
        {
            Card = card;
        }
    }
}