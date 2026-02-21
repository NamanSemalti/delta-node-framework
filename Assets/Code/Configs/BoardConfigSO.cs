using UnityEngine;

namespace CardMatch.Configs
{
    [CreateAssetMenu(
        fileName = "BoardConfig",
        menuName = "CardMatch/Board Config"
    )]
    public sealed class BoardConfigSO : ScriptableObject
    {
        public int Rows = 4;
        public int Columns = 4;
    }
}