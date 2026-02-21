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
            float spacing = 10f;

            float totalWidth =
                container.rect.width - spacing * (board.Columns - 1);
            float totalHeight =
                container.rect.height - spacing * (board.Rows - 1);

            float cardWidth = totalWidth / board.Columns;
            float cardHeight = totalHeight / board.Rows;

            int index = 0;

            foreach (var card in board.Cards)
            {
                var cardView = Instantiate(cardPrefab, container);
                var rect = cardView.GetComponent<RectTransform>();

                int row = index / board.Columns;
                int col = index % board.Columns;

                rect.anchorMin = rect.anchorMax = new Vector2(0, 1);
                rect.pivot = new Vector2(0, 1);

                rect.sizeDelta = new Vector2(cardWidth, cardHeight);

                float x = col * (cardWidth + spacing);
                float y = -row * (cardHeight + spacing);

                rect.anchoredPosition = new Vector2(x, y);

                cardView.Initialize(card, eventBus);

                var input = cardView.gameObject.AddComponent<CardInputController>();
                input.Initialize(card, resolver);

                index++;
            }
        }
    }
}