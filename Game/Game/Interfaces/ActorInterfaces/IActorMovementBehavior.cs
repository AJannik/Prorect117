using Game.Components.Actor;
using Game.Components.Actor.Helper;

namespace Game.Interfaces.ActorInterfaces
{
    public interface IActorMovementBehavior
    {
        public IActor Actor { get; set; }

        public ActorState UpdateMovementBehavior(ActorState currentState, float deltaTime);

        public void ClearTimeInState();
    }
}