using System;

namespace CardMatch.Core.Domain.Card
{
    public sealed class Card
    {
        public int Id { get; }
        public int MatchKey { get; }
        public CardState State { get; private set; }

        public Card(int id, int matchKey)
        {
            Id = id;
            MatchKey = matchKey;
            State = CardState.FaceDown;
        }

        public bool CanFlip()
        {
            return State == CardState.FaceDown;
        }

        public void StartFlipUp()
        {
            if (!CanFlip())
                throw new InvalidOperationException("Card cannot be flipped up.");

            State = CardState.FlippingUp;
        }

        public void Reveal()
        {
            if (State != CardState.FlippingUp)
                throw new InvalidOperationException("Card must be flipping up to reveal.");

            State = CardState.Revealed;
        }

        public void StartFlipDown()
        {
            if (State != CardState.Revealed)
                throw new InvalidOperationException("Only revealed cards can flip down.");

            State = CardState.FlippingDown;
        }

        public void Hide()
        {
            if (State != CardState.FlippingDown)
                throw new InvalidOperationException("Card must be flipping down to hide.");

            State = CardState.FaceDown;
        }

        public void Lock()
        {
            if (State != CardState.Revealed)
                throw new InvalidOperationException("Only revealed cards can be locked.");

            State = CardState.Locked;
        }
    }
}