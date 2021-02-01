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
    }
}