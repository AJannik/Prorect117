using Game.Interfaces.ActorInterfaces;

namespace Game.Components.Actor.Skeleton
{
    public class SkeletonStats : IActorStats
    {
        private float maxHp = 40f;

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

        public float CurrentHealth { get; set; } = 40f;

        public float AttackSpeed { get; set; } = 1.8f;

        public float TimeToHit { get; set; } = 0.8f;

        public float DyingTime { get; set; } = 0.75f;

        public float AttackTime { get; set; } = 0f;

        public float MoveSpeed { get; set; } = 1.5f;

        public float Armor { get; set; } = 50f;

        public float BleedTime { get; set; } = 0f;

        public float AttackCooldown { get; set; }

        public float AttackDamage { get; set; } = 80f;

        public float InvincibleTime { get; set; } = 0f;

        public float JumpForce { get; set; }

        public float JumpCooldown { get; set; }

        public bool RollEnabled { get; set; }
    }
}