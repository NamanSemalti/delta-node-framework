using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using CardMatch.Core.Domain.Card;
using CardMatch.Core.Events;
using CardMatch.Core.Interfaces;
using CardMatch.Configs;

namespace CardMatch.Presentation.Views
{
    public sealed class CardView : MonoBehaviour
    {
        [SerializeField] private Transform cardVisual;
        [SerializeField] private Image visualImage;
        [SerializeField] private float flipDuration = 0.25f;
        [SerializeField] private float mismatchFlipBackDelay = 0.5f;

        private Card _card;
        private IEventBus _eventBus;
        private CardVisualConfigSO _visualConfig;

        // VISUAL-ONLY state
        private bool _isFlippingUp;
        private Coroutine _flipCoroutine;

        public void Initialize(
    Card card,
    IEventBus eventBus,
    CardVisualConfigSO visualConfig)
        {
            _card = card;
            _eventBus = eventBus;
            _visualConfig = visualConfig;

            if (_card.State == CardState.Revealed || _card.State == CardState.Locked)
            {
                // Card was already revealed → show front
                visualImage.sprite = _visualConfig.GetSprite(_card.MatchKey);
                cardVisual.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                // Normal face-down start
                visualImage.sprite = _visualConfig.CardBack;
                cardVisual.localRotation = Quaternion.identity;
            }

            _eventBus.Subscribe<CardFlipStarted>(OnCardFlipStarted);
            _eventBus.Subscribe<MatchResolved>(OnMatchResolved);
        }

        private void OnDestroy()
        {
            if (_eventBus == null) return;

            _eventBus.Unsubscribe<CardFlipStarted>(OnCardFlipStarted);
            _eventBus.Unsubscribe<MatchResolved>(OnMatchResolved);
        }

        // =========================
        // EVENT HANDLERS
        // =========================

        private void OnCardFlipStarted(CardFlipStarted evt)
        {
            if (evt.Card != _card)
                return;

            // Only cancel previous visual animation of THIS card
            if (_flipCoroutine != null)
                StopCoroutine(_flipCoroutine);

            _flipCoroutine = StartCoroutine(FlipUp());
        }

        private void OnMatchResolved(MatchResolved evt)
        {
            if (!evt.Cards.Contains(_card))
                return;

            if (_card.State == CardState.Locked)
                return;

            // DO NOT stop flip-up
            StartCoroutine(HandleMismatchFlipBack());
        }

        // =========================
        // ANIMATION FLOW
        // =========================

        private IEnumerator HandleMismatchFlipBack()
        {
            // Wait until flip-up finishes
            while (_isFlippingUp)
                yield return null;

            // Let player SEE the mismatch
            yield return new WaitForSeconds(mismatchFlipBackDelay);

            // Ensure we don't overlap animations
            if (_flipCoroutine != null)
                StopCoroutine(_flipCoroutine);

            _flipCoroutine = StartCoroutine(FlipDown());
        }

        private IEnumerator FlipUp()
        {
            _isFlippingUp = true;

            // 0 → 90 (back)
            yield return Rotate(0f, 90f);

            // Swap sprite at midpoint
            visualImage.sprite = _visualConfig.GetSprite(_card.MatchKey);

            // 90 → 180 (front)
            yield return Rotate(90f, 180f);

            _isFlippingUp = false;
        }

        private IEnumerator FlipDown()
        {
            // 180 → 90 (front)
            yield return Rotate(180f, 90f);

            // Swap back at midpoint
            visualImage.sprite = _visualConfig.CardBack;

            // 90 → 0 (back)
            yield return Rotate(90f, 0f);
        }

        // =========================
        // ROTATION
        // =========================

        private IEnumerator Rotate(float from, float to)
        {
            float t = 0f;

            while (t < flipDuration)
            {
                t += Time.deltaTime;
                float y = Mathf.Lerp(from, to, t / flipDuration);
                cardVisual.localRotation = Quaternion.Euler(0f, y, 0f);
                yield return null;
            }

            cardVisual.localRotation = Quaternion.Euler(0f, to, 0f);
        }
        public void Preview(float previewDuration)
        {
            StartCoroutine(PreviewRoutine(previewDuration));
        }

        private IEnumerator PreviewRoutine(float duration)
        {
            // Only preview unrevealed cards
            if (_card.State != CardState.FaceDown)
                yield break;

            // Flip up visually (NO events)
            yield return FlipUp();

            // Hold so player can memorize
            yield return new WaitForSeconds(duration);

            // If card is still unrevealed logically, flip back
            if (_card.State == CardState.FaceDown)
                yield return FlipDown();
        }
    }
}