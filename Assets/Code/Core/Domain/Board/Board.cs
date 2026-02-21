using System.Collections.Generic;
using System.Linq;
using CardMatch.Core.Domain.Card;
namespace CardMatch.Core.Domain.Board
{
    public sealed class Board
    {
        public int Rows { get; }
        public int Columns { get; }

        private readonly List<Card.Card> _cards;
        public IReadOnlyList<Card.Card> Cards => _cards;

        public Board(int rows, int columns, List<Card.Card> cards)
        {
            Rows = rows;
            Columns = columns;
            _cards = cards;
        }

        public Card.Card GetCardById(int id)
        {
            return _cards.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Card.Card> GetRevealedCards()
        {
            return _cards.Where(c => c.State == CardState.Revealed);
        }
    }
}