using UnityEngine;
using CardMatch.Configs;
using CardMatch.Core.Domain.Match;
using CardMatch.Core.Events;
using CardMatch.Core.Interfaces;

namespace CardMatch.Presentation.Audio
{
    public sealed class AudioService
    {
        private readonly AudioSource _audioSource;
        private readonly AudioConfigSO _config;

        public AudioService(
            AudioSource audioSource,
            AudioConfigSO config,
            IEventBus eventBus)
        {
            _audioSource = audioSource;
            _config = config;

            eventBus.Subscribe<CardRevealed>(OnCardRevealed);
            eventBus.Subscribe<MatchResolved>(OnMatchResolved);
        }

        private void OnCardRevealed(CardRevealed evt)
        {
            Play(_config.CardFlip);
        }

        private void OnMatchResolved(MatchResolved evt)
        {
            Play(evt.Result == MatchResult.Match
                ? _config.Match
                : _config.Mismatch);
        }

        private void Play(AudioClip clip)
        {
            if (clip == null) return;
            _audioSource.PlayOneShot(clip);
        }
    }
}