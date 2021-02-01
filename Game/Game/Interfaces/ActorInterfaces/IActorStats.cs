namespace Game.Interfaces.ActorInterfaces
{
    public interface IActorStats
    {
        public float MaxHealth { get; set; }

        public float CurrentHealth { get; set; }

        public float AttackSpeed { get; set; }

        public float TimeToHit { get; set; }

        public float DyingTime { get; set; }

        public float AttackTime { get; set; }

        public float MoveSpeed { get; set; }

        public float Armor { get; set; }

        public float BleedTime { get; set; }

        public float AttackCooldown { get; set; }

        public float AttackDamage { get; set; }

        public float InvincibleTime { get; set; }

        public float JumpForce { get; set; }

        public float JumpCooldown { get; set; }

        public bool RollEnabled { get; set; }
    }
}