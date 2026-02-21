using UnityEngine;
using UnityEngine.EventSystems;
using CardMatch.Core.Domain.Card;
using CardMatch.Core.Interfaces;

namespace CardMatch.Presentation.Controllers
{
    public sealed class CardInputController
        : MonoBehaviour, IPointerClickHandler
    {
        private Card _card;
        private IMatchResolver _matchResolver;

        public void Initialize(Card card, IMatchResolver matchResolver)
        {
            _card = card;
            _matchResolver = matchResolver;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_card == null || _matchResolver == null)
            {
                Debug.LogError("CardInputController not initialized properly");
                return;
            }

            _matchResolver.RequestFlip(_card);
        }
    }
}