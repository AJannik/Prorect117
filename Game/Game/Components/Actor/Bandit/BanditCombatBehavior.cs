using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;

namespace Game.Components.Actor
{
    public class BanditCombatBehavior : IActorCombatBehavior
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

        public IActor Actor { get; set; }

        public float AttackTime { get; set; } = 0f;

        public float TimeToHit { get; set; } = 0.33f;

        private float DyingTime { get; set; } = 0.75f;

        public ActorState UpdateCombatBehavior(ActorState currentState, float deltaTime)
        {
            if (IsDead(currentState, deltaTime))
            {
                return ActorState.Dead;
            }

            if (IsDying())
            {
                return ActorState.Dying;
            }

            if (AttackTime <= 0f)
            {
                if (currentState == ActorState.Attacking)
                {
                    currentState = ActorState.Idle;
                }

                currentState = CheckAttackState(currentState);
            }
            else
            {
                AttackTime -= deltaTime;
            }

            return currentState;
        }

        private ActorState CheckAttackState(ActorState currentState)
        {
            foreach (IComponent component in Actor.LeftTrigger.GetTriggerHits())
            {
                if (component.MyGameObject.Name == "Player")
                {
                    Actor.FacingRight = false;
                    currentState = SetAttackState();
                }
            }

            foreach (IComponent component in Actor.RightTrigger.GetTriggerHits())
            {
                if (component.MyGameObject.Name == "Player")
                {
                    Actor.FacingRight = true;
                    currentState = SetAttackState();
                }
            }

            return currentState;
        }

        private ActorState SetAttackState()
        {
            Actor.ActorMovementBehavior.ClearTimeInState();
            AttackTime = AttackSpeed;
            const ActorState currentState = ActorState.Attacking;
            return currentState;
        }

        private bool IsDying()
        {
            return Actor.Combat.CurrentHealth <= 0f;
        }

        private bool IsDead(ActorState currentState, float deltaTime)
        {
            if (currentState == ActorState.Dying)
            {
                if (DyingTime <= 0f)
                {
                    return true;
                }

                DyingTime -= deltaTime;
            }

            return false;
        }
    }
}