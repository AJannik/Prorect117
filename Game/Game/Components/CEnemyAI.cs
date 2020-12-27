using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
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

    public class CEnemyAI : IComponent
    {
        public CEnemyAI()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public CAnimationSystem AnimationSystem { get; set; } = null;

        public CRigidBody RigidBody { get; set; }

        public CBoxCollider LeftTrigger { get; set; }

        public CBoxCollider RightTrigger { get; set; }

        public float MoveSpeed { get; private set; } = 5f;

        public EnemyState State { get; private set; } = EnemyState.Idle;

        public bool FacingRight { get; set; } = false;

        private float TimeInState { get; set; } = 0f;

        private int LeftOnGround { get; set; } = 0;

        private int RightOnGround { get; set; } = 0;

        public void Update(float deltaTime)
        {
            if (!MyGameObject.Active)
            {
                return;
            }
            switch (State)
            {
                case EnemyState.Idle:
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
                    break;
                case EnemyState.Attacking:
                    break;
                default:
                    State = EnemyState.Idle;
                    break;
            }

            if (TimeInState <= 0f)
            {
                if (State == EnemyState.Idle)
                {
                    State = EnemyState.Running;
                    TimeInState = 5f;
                }
                else
                {
                    TimeInState = 5f;
                    State = EnemyState.Idle;
                }
            }
        }

        public void SetupLeftTrigger(CBoxCollider trigger)
        {
            LeftTrigger = trigger;
            LeftTrigger.TriggerExited += LeftFootExited;
            LeftTrigger.TriggerEntered += LeftEntered;
        }

        public void SetupRightTrigger(CBoxCollider trigger)
        {
            RightTrigger = trigger;
            RightTrigger.TriggerExited += RightFootExited;
            RightTrigger.TriggerEntered += RightEntered;
        }

        private void LeftEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                // attack left
            }

            if (e.MyGameObject.Name == "Wall")
            {
                FacingRight = true;
                Console.WriteLine("Entered Wall");
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
            if (e.MyGameObject.Name == "Player")
            {
                // attack right
            }

            if (e.MyGameObject.Name == "Wall")
            {
                FacingRight = false;
                Console.WriteLine("Entered Wall");
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
