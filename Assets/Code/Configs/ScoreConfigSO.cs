using UnityEngine;

namespace CardMatch.Configs
{
    [CreateAssetMenu(
        fileName = "ScoreConfig",
        menuName = "CardMatch/Score Config"
    )]
    public sealed class ScoreConfigSO : ScriptableObject
    {
        public int MatchScore = 100;
        public int MismatchPenalty = -10;
    }
}