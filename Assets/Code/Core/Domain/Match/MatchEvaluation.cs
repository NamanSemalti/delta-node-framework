using System.Collections.Generic;
using System.Linq;
using CardMatch.Core.Domain.Card;

namespace CardMatch.Core.Domain.Match
{
    public sealed class MatchEvaluation
    {
        public IReadOnlyList<Card.Card> Cards { get; }
        public MatchResult Result { get; }

        public MatchEvaluation(IReadOnlyList<Card.Card> cards)
        {
            Cards = cards;

            Result = cards.All(c => c.MatchKey == cards[0].MatchKey)
                ? MatchResult.Match
                : MatchResult.Mismatch;
        }
    }
}