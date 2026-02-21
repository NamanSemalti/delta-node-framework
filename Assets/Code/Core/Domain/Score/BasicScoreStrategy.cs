using System;
using CardMatch.Core.Domain.Match;

namespace CardMatch.Core.Domain.Score
{
    public sealed class BasicScoreStrategy : IScoreStrategy
    {
        public int CalculateScore(MatchResult result, int comboCount)
        {
            if (result == MatchResult.Match)
                return 100 * Math.Max(1, comboCount);

            return -10;
        }
    }
}