using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Game.Components.Collision;
using Game.Components.Renderer;
using Game.Interfaces;
using OpenTK;

namespace Game.Components
{
    public enum EnemyState
    {
        Idle,
        Running,
        Attacking,
    }

    public class CEnemyAI : IComponent, IUpdateable
    {
        public CEnemyAI()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public CAnimationSystem AnimationSystem { get; set; } = null;

        public CRigidBody RigidBody { get; set; }

        public CBoxTrigger LeftTrigger { get; set; }

        public CBoxTrigger RightTrigger { get; set; }

        public CCombat Combat { get; set; }

        public float MoveSpeed { get; private set; } = 1.5f;

        public EnemyState State { get; private set; } = EnemyState.Idle;

        public bool FacingRight { get; set; } = false;

        private float TimeInState { get; set; } = 0f;

        private int LeftOnGround { get; set; } = 0;

        private int RightOnGround { get; set; } = 0;

        private bool AttackDisabled { get; set; } = false;

        private Random Randomizer { get; set; } = new Random();

        public void Update(float deltaTime)
        {
            switch (State)
            {
                case EnemyState.Idle:
                    AnimationSystem.PlayAnimation("Idle", false, FacingRight);
                    RigidBody.Velocity = new Vector2(0, RigidBody.Velocity.Y);
                    break;
                case EnemyState.Running:
                    if (RightOnGround < 1)
                    {
                        FacingRight = false;
                    }

                    if (LeftOnGround < 1)
                    {
                        FacingRight = true;
                    }

                    RigidBody.Velocity = new Vector2((FacingRight ? 1 : -1) * MoveSpeed, RigidBody.Velocity.Y);
                    AnimationSystem.PlayAnimation("Walk", false, FacingRight);
                    break;
                case EnemyState.Attacking:
                    TelegraphAttack();
                    break;
                default:
                    State = EnemyState.Idle;
                    break;
            }

            if (AnimationSystem.ActiveAnimation.Name == "Hurt")
            {
                State = EnemyState.Idle;
            }

            if (TimeInState <= 0f)
            {
                if (State == EnemyState.Idle)
                {
                    State = EnemyState.Running;
                    TimeInState = Randomizer.Next(3, 15) + (float)Randomizer.NextDouble();
                }
                else if (State == EnemyState.Attacking)
                {
                    AttackDisabled = false;
                    State = EnemyState.Idle;
                    TimeInState = TimeInState = Randomizer.Next(1, 3) + (float)Randomizer.NextDouble();
                }
                else
                {
                    TimeInState = Randomizer.Next(1, 7) + (float)Randomizer.NextDouble();
                    State = EnemyState.Idle;
                }
            }

            if (TimeInState > 0f)
            {
                TimeInState -= deltaTime;
            }
        }

        public void SetupLeftTrigger(CBoxTrigger trigger)
        {
            LeftTrigger = trigger;
            LeftTrigger.TriggerExited += LeftFootExited;
            LeftTrigger.TriggerEntered += LeftEntered;
        }

        public void SetupRightTrigger(CBoxTrigger trigger)
        {
            RightTrigger = trigger;
            RightTrigger.TriggerExited += RightFootExited;
            RightTrigger.TriggerEntered += RightEntered;
        }

        private void Attack()
        {
            State = EnemyState.Attacking;
            TimeInState = 1f;
            AnimationSystem.PlayAnimation("Attack", true, FacingRight);
            RigidBody.Velocity = new Vector2(0, RigidBody.Velocity.Y);
        }

        private void TelegraphAttack()
        {
            if (TimeInState < 0.67 && !AttackDisabled)
            {
                Combat.Attack(FacingRight ? RightTrigger : LeftTrigger, 1, false);
                AttackDisabled = true;
            }
        }

        private void LeftEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player" && State != EnemyState.Attacking)
            {
                FacingRight = false;
                Attack();
            }

            if (e.MyGameObject.Name == "Wall")
            {
                FacingRight = true;
            }

            if (e.MyGameObject.Name == "Floor")
            {
                LeftOnGround++;
            }
        }

        private void LeftFootExited(object sender, IComponent e)
        {
            if ((e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Ground") && LeftOnGround > 0)
            {
                LeftOnGround--;
            }
        }

        private void RightEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player" && State != EnemyState.Attacking)
            {
                FacingRight = true;
                Attack();
            }

            if (e.MyGameObject.Name == "Wall")
            {
                FacingRight = false;
            }

            if (e.MyGameObject.Name == "Floor")
            {
                RightOnGround++;
            }
        }

        private void RightFootExited(object sender, IComponent e)
        {
            if ((e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Ground") && RightOnGround > 0)
            {
                RightOnGround--;
            }
        }
    }
}
