using CardMatch.Core.Domain.Card;

namespace CardMatch.Core.Interfaces
{
    public interface IMatchResolver
    {
        void RequestFlip(Card card);
    }
}