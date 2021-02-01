using Game.Components.Actor;

namespace Game.Interfaces.ActorInterfaces
{
    public interface IActorCombatBehavior
    {
        public IActor Actor { get; set; }

        public ActorState UpdateCombatBehavior(ActorState currentState, float deltaTime);
    }
}