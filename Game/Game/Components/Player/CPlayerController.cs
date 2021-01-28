using Game.Components.Collision;
using Game.Components.Renderer;
using Game.Entity;
using Game.Interfaces;
using OpenTK.Input;

namespace Game.Components.Player
{
    public enum PlayerState
    {
        Free,
        Blocked,
    }

    public class CPlayerController : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; } = null;

        public CRigidBody RigidBody { get; set; } = null;

        public CBoxTrigger GroundTrigger { get; set; }

        public CRender Render { get; set; }

        public CAnimationSystem AnimationSystem { get; set; }

        public PlayerState State { get; set; } = PlayerState.Free;

        public float PlayerSpeed { get; set; } = 10f;

        public float JumpForce { get; set; } = 1200f;

        public bool FacingRight { get; private set; } = true;

        private float JumpCooldown { get; set; }

        private bool Jumping { get; set; } = false;

        private int OnGround { get; set; } = 0;

        public void SetUpGroundTrigger(CBoxTrigger trigger)
        {
            GroundTrigger = trigger;
            GroundTrigger.TriggerEntered += OnGroundTriggerEntered;
            GroundTrigger.TriggerExited += OnGroundTriggerExited;
        }

        public void Update(float deltaTime)
        {
            var keyboard = Keyboard.GetState();

            float axisLeftRight = keyboard.IsKeyDown(Key.A) ? -1.0f : keyboard.IsKeyDown(Key.D) ? 1.0f : 0.0f;
            if (State != PlayerState.Free && OnGround > 0)
            {
                axisLeftRight = 0f;
            }

            if (State == PlayerState.Free)
            {
                RigidBody.Velocity = new OpenTK.Vector2(axisLeftRight * PlayerSpeed, RigidBody.Velocity.Y);
            }

            if (keyboard.IsKeyDown(Key.Space) && State == PlayerState.Free)
            {
                Jump();
            }

            if (Jumping && RigidBody.Velocity.Y >= 0f)
            {
                RigidBody.GravityScale = 4f;
                AnimationSystem.UpdateFlipped(!FacingRight);
            }
            else
            {
                RigidBody.GravityScale = 4f;
                Jumping = false;
            }

            // updating facingRight and animations
            if (RigidBody.Velocity.X > 0.1f)
            {
                FacingRight = true;
                if (!Jumping)
                {
                    AnimationSystem.PlayAnimation("Run", false, !FacingRight);
                }
            }
            else if (RigidBody.Velocity.X < -0.1f)
            {
                FacingRight = false;
                if (!Jumping)
                {
                    AnimationSystem.PlayAnimation("Run", false, !FacingRight);
                }
            }

            if (RigidBody.Velocity.X <= 0.001f && RigidBody.Velocity.X >= -0.001f && !Jumping)
            {
                AnimationSystem.PlayAnimation("Idle", false, !FacingRight);
            }

            if (RigidBody.Velocity.Y < 0f && OnGround < 1)
            {
                AnimationSystem.PlayAnimation("Fall", false, !FacingRight);
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
                RigidBody.Velocity = new OpenTK.Vector2(RigidBody.Velocity.X, 0f);
                RigidBody.AddForce(new OpenTK.Vector2(0f, JumpForce));
                Jumping = true;
                JumpCooldown = 0.1f;
                AnimationSystem.PlayAnimation("Jump", false, !FacingRight);
            }
        }

        private void OnGroundTriggerExited(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Wall")
            {
                OnGround--;
            }
        }

        private void OnGroundTriggerEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Wall")
            {
                OnGround++;
                Jumping = false;
                if (RigidBody.Velocity.X >= -0.01f && RigidBody.Velocity.X <= 0.01f)
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