using System.Linq;
using CardMatch.Core.Domain.Boards;
using CardMatch.Core.Domain.Match;
using CardMatch.Core.Events;
using CardMatch.Core.Interfaces;

namespace CardMatch.Application.Game
{
    public sealed class GameCompletionService
    {
        private readonly Board _board;
        private readonly IEventBus _eventBus;

        public GameCompletionService(Board board, IEventBus eventBus)
        {
            _board = board;
            _eventBus = eventBus;

            _eventBus.Subscribe<MatchResolved>(OnMatchResolved);
        }

        private void OnMatchResolved(MatchResolved evt)
        {
            // Only check after successful matches
            if (evt.Result != MatchResult.Match)
                return;

            bool allLocked = _board.Cards.All(c => c.State == Core.Domain.Card.CardState.Locked);

            if (allLocked)
                _eventBus.Publish(new GameCompleted());
        }
    }
}