using System;
using System.Collections.Generic;
using System.Text;
using Game.Components;

namespace Game.Tools
{
    public enum EffectType { Slow, Brittle, Silenced, Weakness }

    public class Effect
    {
        public Effect(EffectType type, float duration, int strength)
        {
            Type = type;
            Duration = duration;
            Strength = strength;
        }

        public EffectType Type { get; private set; }

        public float Duration { get; set; }

        public int Strength { get; set; }

        /// <summary>
        /// Updates the Effect.
        /// </summary>
        /// <param name="deltaTime">DeltaTime.</param>
        /// <returns>If true the effect ran out.</returns>
        public bool Update(float deltaTime)
        {
            Duration -= deltaTime;
            if (Duration <= 0f)
            {
                return true;
            }

            return false;
        }
    }
}
