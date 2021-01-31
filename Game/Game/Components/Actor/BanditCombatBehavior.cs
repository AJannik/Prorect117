using Game.Interfaces;

namespace Game.Components.Actor
{
    public class BanditCombatBehavior
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

        public CBandit Bandit { get; set; }

        public float AttackTime { get; private set; } = 0f;

        public float TimeToHit { get; set; } = 0.33f;

        private float DyingTime { get; set; } = 0.75f;

        public EnemyState UpdateCombatBehavior(EnemyState currentState, float deltaTime)
        {
            if (IsDead(currentState, deltaTime))
            {
                return EnemyState.Dead;
            }

            if (IsDying())
            {
                return EnemyState.Dying;
            }

            if (AttackTime <= 0f)
            {
                if (currentState == EnemyState.Attacking)
                {
                    currentState = EnemyState.Idle;
                }

                currentState = CheckAttackState(currentState);
            }
            else
            {
                AttackTime -= deltaTime;
            }

            return currentState;
        }

        private EnemyState CheckAttackState(EnemyState currentState)
        {
            foreach (IComponent component in Bandit.LeftTrigger.GetTriggerHits())
            {
                if (component.MyGameObject.Name == "Player")
                {
                    Bandit.FacingRight = false;
                    currentState = SetAttackState();
                }
            }

            foreach (IComponent component in Bandit.RightTrigger.GetTriggerHits())
            {
                if (component.MyGameObject.Name == "Player")
                {
                    Bandit.FacingRight = true;
                    currentState = SetAttackState();
                }
            }

            return currentState;
        }

        private EnemyState SetAttackState()
        {
            Bandit.BanditMovementBehavior.ClearTimeInState();
            AttackTime = AttackSpeed;
            const EnemyState currentState = EnemyState.Attacking;
            return currentState;
        }

        private bool IsDying()
        {
            return Bandit.Combat.CurrentHealth <= 0f;
        }

        private bool IsDead(EnemyState currentState, float deltaTime)
        {
            if (currentState == EnemyState.Dying)
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