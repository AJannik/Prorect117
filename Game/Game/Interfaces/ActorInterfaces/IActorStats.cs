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
    }
}