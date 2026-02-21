using UnityEngine;

namespace CardMatch.Configs
{
    [CreateAssetMenu(
        fileName = "AudioConfig",
        menuName = "CardMatch/Audio Config"
    )]
    public sealed class AudioConfigSO : ScriptableObject
    {
        public AudioClip CardFlip;
        public AudioClip Match;
        public AudioClip Mismatch;
        public AudioClip GameOver;
    }
}