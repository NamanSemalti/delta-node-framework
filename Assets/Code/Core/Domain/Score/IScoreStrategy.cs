using CardMatch.Core.Domain.Match;

namespace CardMatch.Core.Domain.Score
{
    public interface IScoreStrategy
    {
        int CalculateScore(MatchResult result, int comboCount);
    }
}