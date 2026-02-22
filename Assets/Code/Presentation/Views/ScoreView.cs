using UnityEngine;
using TMPro;
using CardMatch.Core.Events;
using CardMatch.Core.Interfaces;

namespace CardMatch.Presentation.Views
{
    public sealed class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        private IEventBus _eventBus;

        public void Initialize(IEventBus eventBus, int initialScore)
        {
            _eventBus = eventBus;
            scoreText.text = $"Score: {initialScore}";

            _eventBus.Subscribe<ScoreChanged>(OnScoreChanged);
        }

        private void OnDestroy()
        {
            if (_eventBus != null)
                _eventBus.Unsubscribe<ScoreChanged>(OnScoreChanged);
        }

        private void OnScoreChanged(ScoreChanged evt)
        {
            scoreText.text = $"Score: {evt.NewScore}";
        }
    }
}