using System;

namespace Game.Tools
{
    public enum EffectType
    {
        Slow,
        Fragile,
        Silenced,
        Weakness,
    }

    public class Effect
    {
        public Effect(EffectType type, float strength)
        {
            Type = type;
            Strength = strength;
        }

        public EffectType Type { get; }

        public float Strength { get; }
    }
}