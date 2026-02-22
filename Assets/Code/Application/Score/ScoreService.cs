using CardMatch.Core.Domain.Match;
using CardMatch.Core.Domain.Score;
using CardMatch.Core.Events;
using CardMatch.Core.Interfaces;

namespace CardMatch.Application.Score
{
    public sealed class ScoreService
    {
        private readonly IScoreStrategy _scoreStrategy;
        private readonly IEventBus _eventBus;

        private int _combo;
        public int Score { get; private set; }

        public ScoreService(
            IScoreStrategy scoreStrategy,
            IEventBus eventBus,
            int initialScore = 0)
        {
            _scoreStrategy = scoreStrategy;
            _eventBus = eventBus;
            Score = initialScore;

            _eventBus.Subscribe<MatchResolved>(OnMatchResolved);
        }

        private void OnMatchResolved(MatchResolved evt)
        {
            if (evt.Result == MatchResult.Match)
                _combo++;
            else
                _combo = 0;

            _eventBus.Publish(new ComboChanged(_combo));

            Score += _scoreStrategy.CalculateScore(evt.Result, _combo);
            _eventBus.Publish(new ScoreChanged(Score));
        }
    }
}