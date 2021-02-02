using Game.Interfaces.ActorInterfaces;

namespace Game.Components.Actor.Player
{
    public class PlayerStats : IActorStats
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

        public float AttackSpeed { get; set; } = 0.6f;

        public float TimeToHit { get; set; } = 0.33f;

        public float DyingTime { get; set; } = 0.75f;

        public float AttackTime { get; set; } = 0f;

        public float MoveSpeed { get; set; } = 10f;

        public float Armor { get; set; } = 90f;

        public float BleedTime { get; set; }

        public float AttackCooldown { get; set; }

        public float AttackDamage { get; set; } = 10f;

        public float InvincibleTime { get; set; }

        public float JumpForce { get; set; } = 1200f * 0.66f;

        public float JumpCooldown { get; set; }

        public bool RollEnabled { get; set; } = true;
    }
}