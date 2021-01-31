using System;

namespace Game.Components.Actor
{
    public class BanditMovementBehavior
    {
        public float MoveSpeed { get; set; } = 1.5f;

        public CBandit Bandit { get; set; }

        private Random Randomizer { get; set; } = new Random();

        private float TimeInState { get; set; } = 0f;

        public EnemyState UpdateMovementBehavior(EnemyState currentState, float deltaTime)
        {
            if (currentState == EnemyState.Attacking)
            {
                return currentState;
            }

            if (TimeInState > 0f)
            {
                TimeInState -= deltaTime;
                return currentState;
            }

            if (currentState == EnemyState.Idle)
            {
                TimeInState = Randomizer.Next(3, 15) + (float)Randomizer.NextDouble();
                currentState = EnemyState.Running;
            }
            else
            {
                TimeInState = Randomizer.Next(1, 7) + (float)Randomizer.NextDouble();
                currentState = EnemyState.Idle;
            }

            return currentState;
        }

        public void ClearTimeInState()
        {
            TimeInState = 0f;
        }
    }
}