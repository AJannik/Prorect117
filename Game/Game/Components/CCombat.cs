using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;

namespace Game.Components
{
    public class CCombat : IComponent
    {
        public CCombat()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public float MaxHealth { get; set; } = 100f;

        public float CurrentHealth { get; set; } = 100f;

        public float Resistance { get; set; } = 10f;

        public float AttackSpeed { get; set; } = 0.5f;

        public float NextAttackTime { get; set; } = 0f;

        public float AttackDamage { get; set; } = 1f;

        public void Update(float deltaTime)
        {
        }

        public void Attack(CCombat targetCCombat, float dmgMultiplier, CBoxCollider hitbox)
        {
            if (NextAttackTime > 0f)
            {
                return;
            }
        }

        public void TakeDamage(float amount, bool ignoreResistance)
        {

        }
    }
}
