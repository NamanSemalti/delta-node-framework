using System.Linq;
using UnityEngine;
using CardMatch.Core.Domain.Boards;
using CardMatch.Core.Domain.Card;
using CardMatch.Core.Interfaces;

namespace CardMatch.Infrastructure.Persistence
{
    public sealed class PlayerPrefsBoardRepository : IBoardRepository
    {
        private const string SaveKey = "CARD_MATCH_SAVE";

        public void Save(Board board, int score)
        {
            var dto = new SaveDataDTO
            {
                Rows = board.Rows,
                Columns = board.Columns,
                Score = score,
                Cards = board.Cards.Select(c => new CardDTO
                {
                    Id = c.Id,
                    MatchKey = c.MatchKey,
                    State = (int)c.State
                }).ToList()
            };

            string json = JsonUtility.ToJson(dto);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public bool TryLoad(out Board board, out int score)
        {
            board = null;
            score = 0;

            if (!PlayerPrefs.HasKey(SaveKey))
                return false;

            var json = PlayerPrefs.GetString(SaveKey);
            var dto = JsonUtility.FromJson<SaveDataDTO>(json);

            var cards = dto.Cards.Select(c =>
            {
                var card = new Card(c.Id, c.MatchKey);
                RestoreState(card, (CardState)c.State);
                return card;
            }).ToList();

            board = new Board(dto.Rows, dto.Columns, cards);
            score = dto.Score;

            return true;
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }

        private static void RestoreState(Card card, CardState state)
        {
            // Restore deterministically — no animations here
            switch (state)
            {
                case CardState.Revealed:
                    card.StartFlipUp();
                    card.Reveal();
                    break;

                case CardState.Locked:
                    card.StartFlipUp();
                    card.Reveal();
                    card.Lock();
                    break;

                default:
                    // FaceDown is default
                    break;
            }
        }
    }
}