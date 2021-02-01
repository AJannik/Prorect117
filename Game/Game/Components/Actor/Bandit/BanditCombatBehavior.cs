using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;

namespace Game.Components.Actor.Bandit
{
    public class BanditCombatBehavior : IActorCombatBehavior
    {
        public IActor Actor { get; set; }

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

            if (Actor.ActorStats.AttackTime <= 0f)
            {
                if (currentState == ActorState.Attacking)
                {
                    currentState = ActorState.Idle;
                }

                currentState = CheckAttackState(currentState);
            }
            else
            {
                Actor.ActorStats.AttackTime -= deltaTime;
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
            Actor.ActorStats.AttackTime = Actor.ActorStats.AttackSpeed;
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
                if (Actor.ActorStats.DyingTime <= 0f)
                {
                    return true;
                }

                Actor.ActorStats.DyingTime -= deltaTime;
            }

            return false;
        }
    }
}