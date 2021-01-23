using Game.Components.Player;
using Game.Components.Renderer;
using Game.Interfaces;
using OpenTK;

namespace Game.Components.Combat
{
    public class CCombat : IComponent, IUpdateable, IOnStart
    {
        private float maxHp = 100f;

        public float MaxHealth
        {
            get => maxHp;

            set
            {
                maxHp = value;
                if (CurrentHealth > maxHp)
                {
                    CurrentHealth = maxHp;
                }
            }
        }

        public GameObject MyGameObject { get; set; } = null;

        public CAnimationSystem AnimationSystem { get; set; } = null;

        public CTextRender HpText { get; set; }

        public CDamageDisplay DamageDisplay { get; set; }

        public CParticleSystem BloodParticles { get; set; }

        public float CurrentHealth { get; set; } = 100f;

        public float Armor { get; set; } = 90f;

        public float InvincibleTime { get; set; } = 0f;

        public float NextAttackTime { get; set; } = 0f;

        public float AttackSpeed { get; set; } = 1.8f;

        public float AttackDamage { get; set; } = 10f;

        public string HurtAnimationName { get; set; } = "Hurt";

        private CPickupDisplay PickupDisplay { get; set; }

        private CombatHelper CombatHelper { get; } = new CombatHelper();

        private float DeathTime { get; set; } = 0f;

        private bool Dying { get; set; } = false;

        private float BleedTime { get; set; } = 0f;

        public void Start()
        {
            if (MyGameObject.Name == "Player")
            {
                CurrentHealth = MyGameObject.Scene.GameManager.PlayerHealth;
            }
            else
            {
                PickupDisplay = CombatHelper.GetPickupDisplay(MyGameObject);
            }
        }

        public void Update(float deltaTime)
        {
            if (NextAttackTime > 0f)
            {
                NextAttackTime -= deltaTime;
            }

            if (HpText != null)
            {
                HpText.Text = $"{(int)CurrentHealth} HP";
            }

            if (InvincibleTime >= 0f)
            {
                InvincibleTime -= deltaTime;
            }

            HandleBloodEffect(deltaTime);

            if (CurrentHealth <= 0f)
            {
                HandleDeath(deltaTime);
            }
        }

        public void TakeDamage(float dmgAmount, bool ignoreArmor, string dmgAnimationName)
        {
            float dmg;
            if (InvincibleTime <= 0f)
            {
                dmg = CombatHelper.CalculateDamage(dmgAmount, ignoreArmor, Armor);
                CurrentHealth -= dmg;

                AnimationSystem?.PlayAnimation(dmgAnimationName, true);
                BleedTime = 0.1f;

                // UI display of damage
                if (MyGameObject.Name == "Player")
                {
                    MyGameObject.Scene.GameManager.PlayerHealth = CurrentHealth;
                    DamageDisplay?.DisplayDamage($"{(int)CurrentHealth}/{MaxHealth}", Color.White);
                }
                else
                {
                    DamageDisplay?.DisplayDamage($"-{(int)dmg}", Color.DarkRed);
                }
            }
        }

        /// <summary>
        /// Attack in given hitbox. Returns false if attack is still on cooldown.
        /// </summary>
        /// <param name="hitbox">Attack hitbox.</param>
        /// <param name="dmgMultiplier">Multiplies the base damage by this value.</param>
        /// <param name="ignoreArmor">Whether to ignore Armor or not.</param>
        /// <returns>Returns False if attack still on cooldown.</returns>
        public bool Attack(ITrigger hitbox, float dmgMultiplier, bool ignoreArmor)
        {
            if (NextAttackTime > 0f)
            {
                return false;
            }

            NextAttackTime = CombatHelper.ResetAttackTime(AttackSpeed);
            CombatHelper.ApplyDamage(hitbox, dmgMultiplier, ignoreArmor, MyGameObject.Name, AttackDamage);

            return true;
        }

        public void MakeInvincible(float time)
        {
            if (InvincibleTime < time)
            {
                InvincibleTime = time;
            }
        }

        private void HandleBloodEffect(float deltaTime)
        {
            if (BleedTime > 0f && BloodParticles != null)
            {
                BleedTime -= deltaTime;
                BloodParticles.Actice = true;
            }
            else if (BloodParticles != null)
            {
                BloodParticles.Actice = false;
            }
        }

        private void HandleDeath(float deltaTime)
        {
            if (!Dying)
            {
                AnimationSystem.PlayAnimation("Death", true);
                DeathTime = 0.75f;
                Dying = true;
                if (MyGameObject.Name != "Player")
                {
                    MyGameObject.Scene.GameManager.Coins += 2;
                    PickupDisplay.AddCoins(2);
                }
            }
            else if (DeathTime <= 0f)
            {
                MyGameObject.Scene.RemoveGameObject(MyGameObject);
                if (MyGameObject.Name == "Player")
                {
                    MyGameObject.Scene.GameManager.EndGame();
                    MyGameObject.Scene.GameManager.PlayerWon = false;
                }
            }
            else
            {
                DeathTime -= deltaTime;
            }
        }
    }
}