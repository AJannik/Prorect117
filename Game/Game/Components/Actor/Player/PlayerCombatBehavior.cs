using System;
using Game.Interfaces;
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
                    //ComboAttack(true);
                    Actor.ActorStateBehavior.Attacking(Actor.FacingRight ? Actor.RightTrigger : Actor.LeftTrigger);
                    Actor.ActorStats.AttackTime = Actor.ActorStats.AttackSpeed;
                }
            }
            else
            {
                Actor.ActorStats.AttackTime -= deltaTime;
            }
        }
/*
        private void ComboAttack(bool leftSide)
        {
            bool successful;
            ITrigger trigger = Actor.RightTrigger;
            if (leftSide)
            {
                trigger = Actor.LeftTrigger;
            }

            switch (ComboCount)
            {
                case 0:
                    successful = Attack1(trigger, leftSide);
                    break;
                case 1:
                    successful = Attack2(trigger, leftSide);
                    break;
                case 2:
                    successful = Attack3(trigger, leftSide);
                    break;
                default:
                    ComboCount = 0;
                    successful = Attack1(trigger, leftSide);
                    break;
            }

            if (successful)
            {
                ComboCount++;
                ComboTime = 2f;
                ((PlayerMovementBehavior)Actor.ActorMovementBehavior).State = PlayerState.Blocked;
                ((PlayerStateBehavior)Actor.ActorStateBehavior).SetXVelocity(0f);
                LockTime = 0.5f / Actor.ActorStats.AttackSpeed;
            }
        }
        private bool Attack1(ITrigger hitbox, bool leftSide)
        {
            if (Combat.Attack(hitbox, 1f, false))
            {
                AnimationSystem.PlayAnimation("Attack1", true, leftSide);
                return true;
            }

            return false;
        }

        private bool Attack2(ITrigger hitbox, bool leftSide)
        {
            if (Combat.Attack(hitbox, 1.2f, false))
            {
                AnimationSystem.PlayAnimation("Attack2", true, leftSide);
                return true;
            }

            return false;
        }

        private bool Attack3(ITrigger hitbox, bool leftSide)
        {
            if (Combat.Attack(hitbox, 1.8f, false))
            {
                AnimationSystem.PlayAnimation("Attack3", true, leftSide);
                return true;
            }

            return false;
        }*/
    }
}