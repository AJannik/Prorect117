using Game.Interfaces.ActorInterfaces;

namespace Game.Components.Actor.Bandit
{
    public class BanditStats : IActorStats
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

        public float CurrentHealth { get; set; } = 100f;

        public float AttackSpeed { get; set; } = 1f;

        public float TimeToHit { get; set; } = 0.33f;

        public float DyingTime { get; set; } = 0.75f;

        public float AttackTime { get; set; } = 0f;

        public float MoveSpeed { get; set; } = 1.5f;

        public float Armor { get; set; } = 90f;

        public float BleedTime { get; set; } = 0f;

        public float AttackCooldown { get; set; } = 0f;

        public float AttackDamage { get; set; } = 10f;

        public float InvincibleTime { get; set; } = 0f;

        public float JumpForce { get; set; } = 0f;

        public float JumpCooldown { get; set; } = 0f;

        public bool RollEnabled { get; set; }
    }
}