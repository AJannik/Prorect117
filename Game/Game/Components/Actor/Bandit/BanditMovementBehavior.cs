using System;
using Game.Components.Actor.Helper;
using Game.Interfaces.ActorInterfaces;

namespace Game.Components.Actor.Bandit
{
    public class BanditMovementBehavior : IActorMovementBehavior
    {
        public IActor Actor { get; set; }

        private Random Randomizer { get; set; } = new Random();

        private float TimeInState { get; set; } = 0f;

        public ActorState UpdateMovementBehavior(ActorState currentState, float deltaTime)
        {
            if (currentState == ActorState.Attacking)
            {
                return currentState;
            }

            if (TimeInState > 0f)
            {
                TimeInState -= deltaTime;
                return currentState;
            }

            currentState = SetMovementState(currentState);

            return currentState;
        }

        public void ClearTimeInState()
        {
            TimeInState = 0f;
        }

        private ActorState SetMovementState(ActorState currentState)
        {
            if (currentState == ActorState.Idle)
            {
                TimeInState = Randomizer.Next(3, 15) + (float) Randomizer.NextDouble();
                currentState = ActorState.Running;
            }
            else
            {
                TimeInState = Randomizer.Next(1, 7) + (float) Randomizer.NextDouble();
                currentState = ActorState.Idle;
            }

            return currentState;
        }
    }
}