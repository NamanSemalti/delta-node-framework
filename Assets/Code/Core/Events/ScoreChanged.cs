namespace CardMatch.Core.Events
{
    public sealed class ScoreChanged
    {
        public int NewScore { get; }

        public ScoreChanged(int newScore)
        {
            NewScore = newScore;
        }
    }
}