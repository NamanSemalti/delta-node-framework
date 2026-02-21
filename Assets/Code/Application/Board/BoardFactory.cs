using System;
using System.Collections.Generic;
using CardMatch.Core.Domain.Card;

namespace CardMatch.Core.Domain.Boards
{
    public sealed class BoardFactory
    {
        private readonly Random _random = new();

        public Board Create(int rows, int columns)
        {
            int totalCards = rows * columns;

            if (totalCards % 2 != 0)
                throw new InvalidOperationException("Total cards must be even.");

            var cards = new List<Card.Card>(totalCards);
            int matchKey = 0;

            for (int i = 0; i < totalCards; i += 2)
            {
                cards.Add(new Card.Card(i, matchKey));
                cards.Add(new Card.Card(i + 1, matchKey));
                matchKey++;
            }

            Shuffle(cards);

            return new Board(rows, columns, cards);
        }

        private void Shuffle(List<Card.Card> cards)
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }
        }
    }
}