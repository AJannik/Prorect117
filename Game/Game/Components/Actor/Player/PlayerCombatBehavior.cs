using Game.Components.Actor.Helper;
using Game.Interfaces.ActorInterfaces;
using OpenTK.Input;

namespace Game.Components.Actor.Player
{
    public class PlayerCombatBehavior : IActorCombatBehavior
    {
        public IActor Actor { get; set; }

        private float ComboTime { get; set; } = 0f;

        private int ComboCount { get; set; } = 0;

        private float LockTime { get; set; } = 0f;

        public ActorState UpdateCombatBehavior(ActorState currentState, float deltaTime)
        {
            HandleAttack(deltaTime);

            if (ComboTime >= 0f)
            {
                ComboTime -= deltaTime;
            }
            else
            {
                ComboCount = 0;
            }

            if (Actor.ActorStats.InvincibleTime > 0f)
            {
                Actor.ActorStats.InvincibleTime -= deltaTime;
            }

            if (LockTime > 0f)
            {
                LockTime -= deltaTime;
            }
            else
            {
                ((PlayerMovementBehavior)Actor.ActorMovementBehavior).State = PlayerState.Free;
            }

            return ActorState.Idle;
        }

        private void HandleAttack(float deltaTime)
        {
            MouseState mouse = Mouse.GetState();
            if (Actor.ActorStats.AttackTime <= 0f)
            {
                if (mouse.IsButtonDown(MouseButton.Left))
                {
                    Actor.ActorStats.AttackTime = Actor.ActorStats.AttackSpeed;
                    ComboAttack();
                }
            }
            else
            {
                Actor.ActorStats.AttackTime -= deltaTime;
            }
        }

        private void ComboAttack()
        {
            bool successful;

            switch (ComboCount)
            {
                case 0:
                    successful = Actor.ActorStateBehavior.Attacking("Attack1", 1f);
                    break;
                case 1:
                    successful = Actor.ActorStateBehavior.Attacking("Attack2", 1.5f);
                    break;
                case 2:
                    successful = Actor.ActorStateBehavior.Attacking("Attack3", 2f);
                    break;
                default:
                    ComboCount = 0;
                    successful = Actor.ActorStateBehavior.Attacking("Attack1", 1f);
                    break;
            }

            LockTime = Actor.ActorStats.AttackSpeed * 0.4f;
            ((PlayerMovementBehavior)Actor.ActorMovementBehavior).State = PlayerState.Blocked;
            ((PlayerStateBehavior)Actor.ActorStateBehavior).SetXVelocity(0f);
            if (successful)
            {
                ComboCount++;
                ComboTime = 1.3f;
            }
        }
    }
}