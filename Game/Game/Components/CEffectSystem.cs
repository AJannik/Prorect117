using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Game.Interfaces;
using Game.Tools;

namespace Game.Components
{
    public class CEffectSystem : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        private List<Effect> Effects { get; set; } = new List<Effect>();

        public void Update(float deltaTime)
        {
            foreach (Effect effect in Effects.ToList())
            {
                if (effect.Update(deltaTime))
                {
                    Effects.Remove(effect);
                }
            }
        }

        public void AddEffect(EffectType type)
        {
            AddEffect(type, 10, 1);
        }

        public void AddEffect(EffectType type, float duration, int strength)
        {
            // check if effect exists in list
            foreach (Effect effect in Effects)
            {
                if (effect.Type == type && effect.Strength < strength)
                {
                    effect.Strength = strength;
                    effect.Duration = duration;
                    return;
                }
                else if (effect.Type == type && effect.Duration < duration)
                {
                    effect.Duration = duration;
                    return;
                }
            }

            // else add effect
            Effects.Add(new Effect(type, duration, strength));
        }
    }
}
