using Game.Components.Actor;

namespace Game.Interfaces.ActorInterfaces
{
    public interface IActorMovementBehavior
    {
        public IActor Actor { get; set; }

        public ActorState UpdateMovementBehavior(ActorState currentState, float deltaTime);

        public void ClearTimeInState();
    }
}