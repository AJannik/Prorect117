using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Game.Components.Collision;
using Game.Components.Combat;
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

        public float InAttackTime { get; set; } = 1f;

        public float HitTime { get; set; } = 0.33f;

        private float TimeInState { get; set; } = 0f;

        private int LeftOnGround { get; set; } = 0;

        private int RightOnGround { get; set; } = 0;

        private bool AttackDisabled { get; set; } = false;

        private Random Randomizer { get; set; } = new Random();

        private float LastInterrupt { get; set; } = 0f;

        public void Update(float deltaTime)
        {
            switch (State)
            {
                case EnemyState.Idle:
                    AnimationSystem.PlayAnimation("Idle", false, !FacingRight);
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

                    if (AnimationSystem.ActiveAnimation.Name != "Death")
                    {
                        RigidBody.Velocity = new Vector2((FacingRight ? 1 : -1) * MoveSpeed, RigidBody.Velocity.Y);
                    }

                    AnimationSystem.PlayAnimation("Walk", false, !FacingRight);
                    break;
                case EnemyState.Attacking:
                    TelegraphAttack();
                    break;
                default:
                    State = EnemyState.Idle;
                    break;
            }

            if (AnimationSystem.ActiveAnimation.Name == "Hurt" && LastInterrupt <= 0)
            {
                State = EnemyState.Idle;
                LastInterrupt = 1f;
            }

            if (TimeInState <= 0f)
            {
                if (State == EnemyState.Idle)
                {
                    State = EnemyState.Running;
                    TimeInState = Randomizer.Next(3, 15) + (float)Randomizer.NextDouble();
                    foreach (IComponent component in LeftTrigger.GetTriggerHits())
                    {
                        if (component.MyGameObject.Name == "Player" && AnimationSystem.ActiveAnimation.Name != "Death")
                        {
                            FacingRight = false;
                            Attack();
                        }
                    }

                    foreach (IComponent component in RightTrigger.GetTriggerHits())
                    {
                        if (component.MyGameObject.Name == "Player" && AnimationSystem.ActiveAnimation.Name != "Death")
                        {
                            FacingRight = true;
                            Attack();
                        }
                    }
                }
                else if (State == EnemyState.Attacking)
                {
                    AttackDisabled = false;
                    State = EnemyState.Idle;
                    TimeInState = TimeInState = Randomizer.Next(1, 3) + (float)Randomizer.NextDouble();
                    foreach (IComponent component in LeftTrigger.GetTriggerHits())
                    {
                        if (component.MyGameObject.Name == "Player" && AnimationSystem.ActiveAnimation.Name != "Death")
                        {
                            FacingRight = false;
                            Attack();
                        }
                    }

                    foreach (IComponent component in RightTrigger.GetTriggerHits())
                    {
                        if (component.MyGameObject.Name == "Player" && AnimationSystem.ActiveAnimation.Name != "Death")
                        {
                            FacingRight = true;
                            Attack();
                        }
                    }
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
            TimeInState = InAttackTime;
            AnimationSystem.PlayAnimation("Attack", false, !FacingRight);
            RigidBody.Velocity = new Vector2(0, RigidBody.Velocity.Y);
        }

        private void TelegraphAttack()
        {
            if (TimeInState < InAttackTime - HitTime && !AttackDisabled)
            {
                Combat.Attack(FacingRight ? RightTrigger : LeftTrigger, 1, false);
                AttackDisabled = true;
            }
        }

        private void LeftEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player" && State != EnemyState.Attacking && AnimationSystem.ActiveAnimation.Name != "Death")
            {
                FacingRight = false;
                Attack();
            }

            if (e.MyGameObject.Name == "Wall" || (e.MyGameObject.Name == "Enemy" && e.MyGameObject != MyGameObject))
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
            if (e.MyGameObject.Name == "Player" && State != EnemyState.Attacking && AnimationSystem.ActiveAnimation.Name != "Death")
            {
                FacingRight = true;
                Attack();
            }

            if (e.MyGameObject.Name == "Wall" || (e.MyGameObject.Name == "Enemy" && e.MyGameObject != MyGameObject))
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
