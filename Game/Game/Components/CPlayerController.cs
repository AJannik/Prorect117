using System;
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

        public CRender Render { get; set; }

        public CAnimationSystem AnimationSystem { get; set; }

        public float PlayerSpeed { get; private set; } = 10f;

        public float JumpForce { get; private set; } = 2000f;

        private float JumpCooldown { get; set; }

        private bool Jumping { get; set; } = false;

        private int OnGround { get; set; } = 0;

        private bool Running { get; set; } = false;

        private bool FacingRight { get; set; } = true;

        public void SetUpGroundTrigger(CBoxCollider trigger)
        {
            GroundTrigger = trigger;
            GroundTrigger.TriggerEntered += OnGroundTriggerEntered;
            GroundTrigger.TriggerExited += OnGroundTriggerExited;
        }

        public void Update(float deltaTime)
        {
            if (!MyGameObject.Active)
            {
                return;
            }

            var keyboard = Keyboard.GetState();

            float axisLeftRight = keyboard.IsKeyDown(Key.A) ? -1.0f : keyboard.IsKeyDown(Key.D) ? 1.0f : 0.0f;
            RigidBody.Velocity = new OpenTK.Vector2(axisLeftRight * PlayerSpeed, RigidBody.Velocity.Y);

            if (keyboard.IsKeyDown(Key.Space))
            {
                Jump();
            }

            if (Jumping)
            {
                if (RigidBody.Velocity.Y < 0f)
                {
                    RigidBody.GravityScale = 1f;
                }
                else
                {
                    // falling after jump
                    Jumping = false;
                    RigidBody.GravityScale = 8f;
                }
            }
            else
            {
                RigidBody.GravityScale = 4f;
            }

            // updating facingRight and animations
            if (RigidBody.Velocity.X > 0f)
            {
                FacingRight = true;
                if (Running == false)
                {
                    Running = true;
                    AnimationSystem.PlayAnimation("Run");
                }
            }
            else if (RigidBody.Velocity.X < 0f)
            {
                FacingRight = false;
                if (Running == false)
                {
                    Running = true;
                    AnimationSystem.PlayAnimation("Run");
                }
            }

            if (RigidBody.Velocity.X == 0 && !Jumping)
            {
                Running = false;
                AnimationSystem.PlayAnimation("Idle");
            }

            if (RigidBody.Velocity.Y < -0.1f)
            {
                AnimationSystem.PlayAnimation("Fall");
            }

            // update Render
            if (FacingRight)
            {
                Render.Flipped = false;
            }
            else
            {
                Render.Flipped = true;
            }

            if (OnGround >= 1 && JumpCooldown > 0)
            {
                JumpCooldown -= deltaTime;
            }
        }

        private void Jump()
        {
            if (!Jumping && OnGround >= 1 && JumpCooldown <= 0f)
            {
                // RigidBody.Velocity = new OpenTK.Vector2(RigidBody.Velocity.X, JumpForce);
                RigidBody.AddForce(new OpenTK.Vector2(0f, JumpForce));
                Jumping = true;
                JumpCooldown = 0.1f;
                AnimationSystem.PlayAnimation("Jump");
            }
        }

        private void OnGroundTriggerExited(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Floor")
            {
                OnGround--;
            }
        }

        private void OnGroundTriggerEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Floor")
            {
                OnGround++;
                Jumping = false;
                if (RigidBody.Velocity.X == 0)
                {
                    AnimationSystem.PlayAnimation("Idle");
                }
                else
                {
                    AnimationSystem.PlayAnimation("Run");
                }
            }
        }
    }
}