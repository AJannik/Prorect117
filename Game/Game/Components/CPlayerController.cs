using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;
using OpenTK.Input;

namespace Game.Components
{
    public class CPlayerController : IComponent
    {
        public CPlayerController()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public CRigidBody RigidBody { get; set; } = null;

        public CBoxCollider GroundTrigger { get; set; }

        public float PlayerSpeed { get; private set; } = 10f;

        public float JumpForce { get; private set; } = 30f;

        private float JumpCooldown { get; set; }

        private bool Jumping { get; set; } = false;

        private bool OnGround { get; set; } = true;

        public void SetUpGroundTrigger(CBoxCollider trigger)
        {
            GroundTrigger = trigger;
            GroundTrigger.TriggerEntered += OnTriggerEntered;
            GroundTrigger.TriggerExited += OnTriggerExited;
        }

        public void Update(float deltaTime)
        {
            var keyboard = Keyboard.GetState();

            float axisLeftRight = keyboard.IsKeyDown(Key.A) ? -1.0f : keyboard.IsKeyDown(Key.D) ? 1.0f : 0.0f;
            RigidBody.Velocity = new OpenTK.Vector2(axisLeftRight * PlayerSpeed, 0f);

            if (keyboard.IsKeyDown(Key.Space))
            {
                Jump();
            }

            if (Jumping && RigidBody.Velocity.Y > 0)
            {
                RigidBody.GravityScale = 0.4f;
            }
            else
            {
                RigidBody.GravityScale = 1f;
            }

            if (OnGround && JumpCooldown > 0)
            {
                JumpCooldown -= deltaTime;
            }
        }

        private void Jump()
        {
            if (!Jumping && !OnGround && JumpCooldown <= 0f)
            {
                RigidBody.AddForce(new OpenTK.Vector2(0, JumpForce));
                Jumping = true;
                JumpCooldown = 0.1f;
            }
        }

        private void OnTriggerExited(object sender, ICollider e)
        {
            OnGround = false;
        }

        private void OnTriggerEntered(object sender, ICollider e)
        {
            OnGround = true;
            Jumping = false;
        }
    }
}
