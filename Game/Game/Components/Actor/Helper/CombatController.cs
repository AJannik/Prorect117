using System;
using System.Linq;
using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;

namespace Game.Components.Actor.Helper
{
    public class CombatController
    {
        public IActor Actor { get; set; }

        /// <summary>
        /// Attack in given hitbox. Returns false if attack is still on cooldown.
        /// </summary>
        /// <param name="hitbox">Attack hitbox.</param>
        /// <param name="dmgMultiplier">Multiplies the base damage by this value.</param>
        /// <param name="ignoreArmor">Whether to ignore Armor or not.</param>
        /// <returns>Returns False if attack still on cooldown.</returns>
        public bool Attack(ITrigger hitbox, float dmgMultiplier, bool ignoreArmor)
        {
            return ApplyDamage(hitbox, dmgMultiplier, ignoreArmor, Actor.MyGameObject.Name, Actor.ActorStats.AttackDamage);
        }

        public void MakeInvincible(float time)
        {
            if (Actor.ActorStats.InvincibleTime < time)
            {
                Actor.ActorStats.InvincibleTime = time;
            }
        }

        public float CalculateDamage(float dmgAmount, bool ignoreArmor, float armor)
        {
            float dmg;
            if (ignoreArmor)
            {
                dmg = dmgAmount;
            }
            else
            {
                dmg = dmgAmount * ((100f - armor) / 100f);
            }

            return dmg;
        }

        private bool ApplyDamage(ITrigger hitbox, float dmgMultiplier, bool ignoreArmor, string name, float attackDamage)
        {
            foreach (IComponent hit in hitbox.GetTriggerHits())
            {
                if (hit.MyGameObject.GetAllComponents().Any(component => component is IActor))
                {
                    hit.MyGameObject.GetComponent<IActor>().ActorStateBehavior.TakeDamage(dmgMultiplier * attackDamage, ignoreArmor);
                    return true;
                }
            }

            return false;
        }
    }
}