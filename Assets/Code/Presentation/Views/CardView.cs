using UnityEngine;
using CardMatch.Core.Domain.Card;
using CardMatch.Core.Events;
using CardMatch.Core.Interfaces;
using System.Linq;

namespace CardMatch.Presentation.Views
{
    public sealed class CardView : MonoBehaviour
    {
        [SerializeField] private Transform cardVisual;
        [SerializeField] private float flipDuration = 0.25f;

        private Card _card;
        private IEventBus _eventBus;

        public void Initialize(Card card, IEventBus eventBus)
        {
            _card = card;
            _eventBus = eventBus;

            _eventBus.Subscribe<CardFlipStarted>(OnCardFlipStarted);
            _eventBus.Subscribe<MatchResolved>(OnMatchResolved);
        }

        private void OnDestroy()
        {
            if (_eventBus == null) return;

            _eventBus.Unsubscribe<CardFlipStarted>(OnCardFlipStarted);
            _eventBus.Unsubscribe<MatchResolved>(OnMatchResolved);
        }

        private void OnCardRevealed(CardRevealed evt)
        {
            if (evt.Card != _card) return;

            StopAllCoroutines();
            StartCoroutine(FlipUp());
        }
        private void OnCardFlipStarted(CardFlipStarted evt)
        {
            if (evt.Card != _card) return;

            StopAllCoroutines();
            StartCoroutine(FlipUp());
        }
        private void OnMatchResolved(MatchResolved evt)
        {
            if (!evt.Cards.Contains(_card)) return;

            if (_card.State == CardState.FlippingDown || _card.State == CardState.FaceDown)
                return;

            StartCoroutine(FlipDown());
        }

        private System.Collections.IEnumerator FlipUp()
        {
            yield return Rotate(0, 180);
        }

        private System.Collections.IEnumerator FlipDown()
        {
            yield return Rotate(180, 0);
        }

        private System.Collections.IEnumerator Rotate(float from, float to)
        {
            float t = 0f;

            while (t < flipDuration)
            {
                t += Time.deltaTime;
                float y = Mathf.Lerp(from, to, t / flipDuration);
                cardVisual.localRotation = Quaternion.Euler(0, y, 0);
                yield return null;
            }

            cardVisual.localRotation = Quaternion.Euler(0, to, 0);
        }
    }
}