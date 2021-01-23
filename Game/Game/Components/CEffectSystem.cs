using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components.Combat;
using Game.Components.Player;
using Game.Interfaces;
using Game.Tools;

namespace Game.Components
{
    public class CEffectSystem : IComponent, IOnStart
    {
        public GameObject MyGameObject { get; set; } = null;

        public CCombat Combat { get; set; }

        public CPlayerController PlayerController { get; set; }

        private List<Effect> Effects { get; set; } = new List<Effect>();

        public void Start()
        {
            if (MyGameObject.Scene.GameManager.Effects.Count == 0)
            {
                return;
            }

            foreach (Effect effect in MyGameObject.Scene.GameManager.Effects)
            {
                Effects.Add(effect);
                ApplyEffect(effect);
            }
        }

        public void AddEffect(EffectType type, float strength)
        {
            // check if silenced-effect exists in list
            if (type == EffectType.Silenced)
            {
                if (Effects.Any(effect => effect.Type == EffectType.Silenced))
                {
                    return;
                }
            }

            // else add effect
            Effect newEffect = new Effect(type, strength);
            Effects.Add(newEffect);
            MyGameObject.Scene.GameManager.Effects.Add(newEffect);
            ApplyEffect(newEffect);
        }

        public void RemoveEffect(Effect effect)
        {
            switch (effect.Type)
            {
                case EffectType.Fragile:
                    Combat.Armor *= 1f / (1f - effect.Strength);
                    break;
                case EffectType.Slow:
                    PlayerController.PlayerSpeed *= 1f / (1f - effect.Strength);
                    break;
                case EffectType.Silenced:
                    break;
                case EffectType.Weakness:
                    Combat.AttackDamage *= 1f / (1f - effect.Strength);
                    break;
                default:
                    throw new NullReferenceException($"Effect of type {effect.Type} doesn't exist!");
            }
        }

        private void ApplyEffect(Effect effect)
        {
            switch (effect.Type)
            {
                case EffectType.Fragile:
                    Combat.Armor *= 1f - effect.Strength;
                    break;
                case EffectType.Slow:
                    PlayerController.PlayerSpeed *= 1f - effect.Strength;
                    break;
                case EffectType.Silenced:
                    break;
                case EffectType.Weakness:
                    Combat.AttackDamage *= 1f - effect.Strength;
                    break;
                default:
                    throw new NullReferenceException($"Effect of type {effect.Type} doesn't exist!");
            }
        }
    }
}