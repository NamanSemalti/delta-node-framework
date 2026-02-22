using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using CardMatch.Application.Match;
using CardMatch.Application.Score;
using CardMatch.Application.Game;

using CardMatch.Configs;

using CardMatch.Core.Interfaces;
using CardMatch.Core.Events;
using CardMatch.Core.Domain.Score;

using CardMatch.Infrastructure.EventBus;
using CardMatch.Infrastructure.Persistence;

using CardMatch.Presentation.Audio;
using CardMatch.Presentation.Views;
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

        [Header("Views")]
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private ComboView comboView;

        [Header("Values")]
        [SerializeField] private float initialPreviewDuration = 1f;
        [SerializeField] private float restartDelay = 1f;

        private IEventBus _eventBus;
        private IBoardRepository _boardRepository;

        private void Awake()
        {
            // -------------------------
            // 1. Infrastructure
            // -------------------------
            _eventBus = new SimpleEventBus();
            _boardRepository = new PlayerPrefsBoardRepository();

            // -------------------------
            // 2. Domain / Application
            // -------------------------
            var scoreStrategy = new BasicScoreStrategy();
            var matchResolver = new MatchResolver(_eventBus);
            var boardFactory = new BoardFactory();

            // -------------------------
            // 3. Load or Create Board
            // -------------------------
            if (!_boardRepository.TryLoad(out var board, out var savedScore))
            {
                board = boardFactory.Create(
                    boardConfig.Rows,
                    boardConfig.Columns
                );
                savedScore = 0;
            }

            // -------------------------
            // 4. Services
            // -------------------------
            var scoreService = new ScoreService(
                scoreStrategy,
                _eventBus,
                savedScore
            );

            _ = new GameCompletionService(board, _eventBus);

            // -------------------------
            // 5. Views
            // -------------------------
            boardView.Build(board, matchResolver, _eventBus);
            scoreView.Initialize(_eventBus, scoreService.Score);
            comboView.Initialize(_eventBus);

            // -------------------------
            // 6. Audio
            // -------------------------
            _ = new AudioService(audioSource, audioConfig, _eventBus);

            // -------------------------
            // 7. Persistence
            // -------------------------
            _eventBus.Subscribe<MatchResolved>(_ =>
            {
                _boardRepository.Save(board, scoreService.Score);
            });

            // -------------------------
            // 8. Game Completion → Restart
            // -------------------------
            _eventBus.Subscribe<GameCompleted>(_ =>
            {
                StartCoroutine(RestartGame());
            });

            // -------------------------
            // 9. Initial Preview
            // -------------------------
            StartCoroutine(InitialPreview());
        }

        private IEnumerator InitialPreview()
        {
            // Wait one frame so layout is ready
            yield return null;

            boardView.PreviewAllCards(initialPreviewDuration);
        }

        private IEnumerator RestartGame()
        {
            // Let player see final match
            yield return new WaitForSeconds(restartDelay);

            // Clear save so new game starts fresh
            _boardRepository.Clear();

            // Reload scene
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}