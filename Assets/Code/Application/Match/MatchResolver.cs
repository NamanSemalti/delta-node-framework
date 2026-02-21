using System.Collections.Generic;
using System.Linq;
using CardMatch.Core.Domain.Card;
using CardMatch.Core.Domain.Match;
using CardMatch.Core.Events;
using CardMatch.Core.Interfaces;

namespace CardMatch.Application.Match
{
    public sealed class MatchResolver : IMatchResolver
    {
        private readonly IEventBus _eventBus;
        private readonly List<Card> _revealedCards = new();

        private readonly int _cardsPerMatch;

        public MatchResolver(IEventBus eventBus, int cardsPerMatch = 2)
        {
            _eventBus = eventBus;
            _cardsPerMatch = cardsPerMatch;
        }

        public void RequestFlip(Card card)
        {
            if (!card.CanFlip())
                return;

            card.StartFlipUp();
            _eventBus.Publish(new CardRevealed(card));

            card.Reveal();
            _revealedCards.Add(card);

            if (_revealedCards.Count < _cardsPerMatch)
                return;

            ResolveMatch();
        }

        private void ResolveMatch()
        {
            var cardsToEvaluate = _revealedCards
                .Take(_cardsPerMatch)
                .ToList();

            _revealedCards.RemoveRange(0, _cardsPerMatch);

            var evaluation = new MatchEvaluation(cardsToEvaluate);

            if (evaluation.Result == MatchResult.Match)
            {
                foreach (var card in cardsToEvaluate)
                    card.Lock();
            }
            else
            {
                foreach (var card in cardsToEvaluate)
                {
                    card.StartFlipDown();
                    card.Hide();
                }
            }

            _eventBus.Publish(new MatchResolved(cardsToEvaluate, evaluation.Result));
        }
    }
}