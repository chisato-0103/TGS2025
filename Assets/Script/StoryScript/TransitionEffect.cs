using UnityEngine;

namespace StoryScript
{
    public enum TransitionEffectType
    {
        None,
        Fade
    }

    [System.Serializable]
    public class TransitionSettings
    {
        [Header("Effect Settings")]
        public TransitionEffectType effectType = TransitionEffectType.Fade;
        
        [Header("Timing")]
        public float duration = 0.5f;
    }
}