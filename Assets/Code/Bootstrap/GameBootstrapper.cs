using UnityEngine;
using CardMatch.Application.Match;
using CardMatch.Application.Score;
using CardMatch.Configs;
using CardMatch.Core.Interfaces;
using CardMatch.Infrastructure.EventBus;
using CardMatch.Infrastructure.Persistence;
using CardMatch.Presentation.Audio;
using CardMatch.Presentation.Views;
using CardMatch.Core.Domain.Score;
using CardMatch.Core.Domain.Boards;

namespace CardMatch.Bootstrap
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private BoardView boardView;
        [SerializeField] private AudioSource audioSource;

        [Header("Configs")]
        [SerializeField] private BoardConfigSO boardConfig;
        [SerializeField] private AudioConfigSO audioConfig;
        [SerializeField] private ScoreConfigSO scoreConfig;

        private void Awake()
        {
            // 1. Core infrastructure
            IEventBus eventBus = new SimpleEventBus();
            IBoardRepository boardRepository = new PlayerPrefsBoardRepository();

            // 2. Domain / Application services
            var scoreStrategy = new BasicScoreStrategy();
            var scoreService = new ScoreService(scoreStrategy, eventBus);

            var matchResolver = new MatchResolver(eventBus);
            var boardFactory = new BoardFactory();

            // 3. Load or create board
            if (!boardRepository.TryLoad(out var board, out var savedScore))
            {
                board = boardFactory.Create(
                    boardConfig.Rows,
                    boardConfig.Columns
                );
            }

            // 4. Build board UI
            boardView.Build(board, matchResolver, eventBus);

            // 5. Audio system
            _ = new AudioService(audioSource, audioConfig, eventBus);

            // 6. (Optional) Auto-save hooks
            eventBus.Subscribe<Core.Events.MatchResolved>(_ =>
            {
                boardRepository.Save(board, scoreService.Score);
            });
        }
    }
}