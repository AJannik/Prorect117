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

        private float DyingTime { get; set; } = 0.75f;

        public EnemyState UpdateCombatBehavior(EnemyState currentState, float deltaTime)
        {
            if (currentState == EnemyState.Dying)
            {
                if (DyingTime <= 0f)
                {
                    return EnemyState.Dead;
                }

                DyingTime -= deltaTime;
            }
            else if (Bandit.Combat.CurrentHealth <= 0f)
            {
                return EnemyState.Dying;
            }

            if (AttackTime <= 0f)
            {
                if (currentState == EnemyState.Attacking)
                {
                    currentState = EnemyState.Idle;
                }

                foreach (IComponent component in Bandit.LeftTrigger.GetTriggerHits())
                {
                    if (component.MyGameObject.Name == "Player")
                    {
                        Bandit.FacingRight = false;
                        Bandit.BanditMovementBehavior.ClearTimeInState();
                        AttackTime = AttackSpeed;
                        currentState = EnemyState.Attacking;
                    }
                }

                foreach (IComponent component in Bandit.RightTrigger.GetTriggerHits())
                {
                    if (component.MyGameObject.Name == "Player")
                    {
                        Bandit.FacingRight = true;
                        Bandit.BanditMovementBehavior.ClearTimeInState();
                        AttackTime = AttackSpeed;
                        currentState = EnemyState.Attacking;
                    }
                }
            }
            else
            {
                AttackTime -= deltaTime;
            }

            return currentState;
        }
    }
}