using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Game.Interfaces;
using Game.Tools;

namespace Game.Components
{
    public class CEffectSystem : IComponent, IUpdateable
    {
        private CPlayerController controller;

        private CCombat combat;

        public GameObject MyGameObject { get; set; } = null;

        public CCombat Combat
        {
            get
            {
                return combat;
            }

            set
            {
                combat = value;
                DefaultArmor = combat.Armor;
                DefaultAttackDmg = combat.AttackDamage;
            }
        }

        public CPlayerController PlayerController
        {
            get
            {
                return controller;
            }

            set
            {
                controller = value;
                DefaultMoveSpeed = controller.PlayerSpeed;
            }
        }

        private float DefaultMoveSpeed { get; set; }

        private float DefaultArmor { get; set; }

        private float DefaultAttackDmg { get; set; }

        private List<Effect> Effects { get; set; } = new List<Effect>();

        public void Update(float deltaTime)
        {
            foreach (Effect effect in Effects.ToList())
            {
                if (effect.Update(deltaTime))
                {
                    Effects.Remove(effect);
                    PlayerController.PlayerSpeed = DefaultMoveSpeed;
                    Combat.Armor = DefaultArmor;
                }
                else
                {
                    switch (effect.Type)
                    {
                        case EffectType.Brittle:
                            Combat.Armor = 1f / (effect.Strength + 0.5f) * DefaultArmor;
                            break;
                        case EffectType.Slow:
                            PlayerController.PlayerSpeed = 1f / (effect.Strength + 1f) * DefaultMoveSpeed;
                            break;
                        case EffectType.Silenced:
                            break;
                        case EffectType.Weakness:
                            Combat.AttackDamage = 1f / ((effect.Strength * 0.5f) + 1f) * DefaultAttackDmg;
                            break;
                        default:
                            break;
                    }
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
