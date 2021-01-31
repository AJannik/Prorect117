using Game.Components.Actor;

namespace Game.Interfaces.ActorInterfaces
{
    public interface IActorCombatBehavior
    {
        public float AttackSpeed { get; set; }

        public IActor Actor { get; set; }

        public float AttackTime { get; set; }

        public float TimeToHit { get; set; }

        public ActorState UpdateCombatBehavior(ActorState currentState, float deltaTime);
    }
}