using UnityEngine;
using CardMatch.Core.Domain.Card;
using CardMatch.Core.Interfaces;

namespace CardMatch.Presentation.Controllers
{
    public sealed class CardInputController : MonoBehaviour
    {
        private Card _card;
        private IMatchResolver _matchResolver;

        public void Initialize(Card card, IMatchResolver matchResolver)
        {
            _card = card;
            _matchResolver = matchResolver;
        }

        private void OnMouseDown()
        {
            _matchResolver.RequestFlip(_card);
        }
    }
}