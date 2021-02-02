using Game.Components.Actor;
using Game.Components.Actor.Helper;

namespace Game.Interfaces.ActorInterfaces
{
    public interface IActorCombatBehavior
    {
        public IActor Actor { get; set; }

        public ActorState UpdateCombatBehavior(ActorState currentState, float deltaTime);
    }
}