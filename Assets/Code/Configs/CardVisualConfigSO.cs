using UnityEngine;

namespace CardMatch.Configs
{
    [CreateAssetMenu(
        fileName = "CardVisualConfig",
        menuName = "CardMatch/Card Visual Config"
    )]
    public sealed class CardVisualConfigSO : ScriptableObject
    {
        public Sprite CardBack;

        [System.Serializable]
        public struct CardVisual
        {
            public int MatchKey;
            public Sprite Sprite;
        }

        public CardVisual[] Visuals;

        public Sprite GetSprite(int matchKey)
        {
            foreach (var v in Visuals)
            {
                if (v.MatchKey == matchKey)
                    return v.Sprite;
            }

            return null;
        }
    }
}