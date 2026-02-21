using CardMatch.Core.Domain.Boards;

namespace CardMatch.Core.Interfaces
{
    public interface IBoardRepository
    {
        void Save(Board board, int score);
        bool TryLoad(out Board board, out int score);
        void Clear();
    }
}