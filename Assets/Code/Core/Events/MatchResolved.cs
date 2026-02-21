using System.Collections.Generic;
using CardMatch.Core.Domain.Card;
using CardMatch.Core.Domain.Match;

namespace CardMatch.Core.Events
{
    public sealed class MatchResolved
    {
        public IReadOnlyList<Card> Cards { get; }
        public MatchResult Result { get; }

        public MatchResolved(IReadOnlyList<Card> cards, MatchResult result)
        {
            Cards = cards;
            Result = result;
        }
    }
}