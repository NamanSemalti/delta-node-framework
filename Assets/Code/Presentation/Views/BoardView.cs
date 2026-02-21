using UnityEngine;
using CardMatch.Core.Domain.Boards;
using CardMatch.Core.Interfaces;
using CardMatch.Presentation.Controllers;
using CardMatch.Presentation.Views;

namespace CardMatch.Presentation.Views
{
    public sealed class BoardView : MonoBehaviour
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private CardView cardPrefab;

        public void Build(Board board, IMatchResolver resolver, IEventBus eventBus)
        {
            int index = 0;
            float cardWidth = container.rect.width / board.Columns;
            float cardHeight = container.rect.height / board.Rows;

            foreach (var card in board.Cards)
            {
                var cardView = Instantiate(cardPrefab, container);
                var rect = cardView.GetComponent<RectTransform>();

                int row = index / board.Columns;
                int col = index % board.Columns;

                rect.sizeDelta = new Vector2(cardWidth, cardHeight);
                rect.anchoredPosition = new Vector2(
                    col * cardWidth,
                    -row * cardHeight
                );

                cardView.Initialize(card, eventBus);

                var input = cardView.gameObject.AddComponent<CardInputController>();
                input.Initialize(card, resolver);

                index++;
            }
        }
    }
}