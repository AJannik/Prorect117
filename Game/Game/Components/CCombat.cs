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

        public float Armor { get; set; } = 10f;

        public float AttackSpeed { get; set; } = 0.5f;

        public float NextAttackTime { get; set; } = 0f;

        public float AttackDamage { get; set; } = 1f;

        public void Update(float deltaTime)
        {
        }

        public void Attack(CBoxCollider hitbox, float dmgMultiplier, bool ignoreArmor)
        {
            if (NextAttackTime > 0f)
            {
                return;
            }

            foreach (IComponent hit in hitbox.GetTriggerHits())
            {
                if ((hit.MyGameObject.Name == "Player" || hit.MyGameObject.Name == "Enemy") && MyGameObject.Name != hit.MyGameObject.Name)
                {
                    hit.MyGameObject.GetComponent<CCombat>().TakeDamage(dmgMultiplier * AttackDamage, ignoreArmor);
                }
            }
        }

        public void TakeDamage(float dmgAmount, bool ignoreArmor)
        {
            if (ignoreArmor)
            {
                CurrentHealth -= dmgAmount;
            }
            else if (Armor >= 0f)
            {
                CurrentHealth -= dmgAmount * (100 / (100 + Armor));
            }
            else
            {
                CurrentHealth -= dmgAmount * (2 - (100 / (100 - Armor)));
            }
        }
    }
}
