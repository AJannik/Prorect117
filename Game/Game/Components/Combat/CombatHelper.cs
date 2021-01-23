using Game.Components.Player;
using Game.Interfaces;

namespace Game.Components.Combat
{
    public class CombatHelper
    {
        public float ResetAttackTime(float attackSpeed)
        {
            float nextAttackTime;
            if (attackSpeed >= 0f)
            {
                nextAttackTime = 1f / attackSpeed;
            }
            else
            {
                nextAttackTime = 1f;
            }

            return nextAttackTime;
        }

        public void ApplyDamage(ITrigger hitbox, float dmgMultiplier, bool ignoreArmor, string name, float attackDamage)
        {
            foreach (IComponent hit in hitbox.GetTriggerHits())
            {
                if ((hit.MyGameObject.Name == "Player" || hit.MyGameObject.Name == "Enemy") && name != hit.MyGameObject.Name && hit.MyGameObject.GetComponent<CCombat>() != null)
                {
                    hit.MyGameObject.GetComponent<CCombat>().TakeDamage(dmgMultiplier * attackDamage, ignoreArmor, hit.MyGameObject.GetComponent<CCombat>().HurtAnimationName);
                }
            }
        }

        public CPickupDisplay GetPickupDisplay(GameObject myGameObject)
        {
            foreach (GameObject gameObject in myGameObject.Scene.GetGameObjects())
            {
                if (gameObject.Name == "Player")
                {
                    foreach (GameObject child in gameObject.GetAllChildren())
                    {
                        if (child.GetComponent<CPickupDisplay>() != null)
                        {
                            return child.GetComponent<CPickupDisplay>();
                        }
                    }
                }
            }

            return null;
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
    }
}