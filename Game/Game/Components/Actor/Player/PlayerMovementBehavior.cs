using Game.Components.Actor.Helper;
using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;
using OpenTK;
using OpenTK.Input;

namespace Game.Components.Actor.Player
{
    public class PlayerMovementBehavior : IActorMovementBehavior
    {
        public IActor Actor { get; set; }

        public PlayerState State { get; set; } = PlayerState.Free;

        public ITrigger GroundTrigger { get; set; }

        private bool CanRoll { get; set; } = true;

        private int OnGround { get; set; } = 0;

        private float RollCooldown { get; set; } = 0f;

        private float RollTelegraph { get; set; }

        public ActorState UpdateMovementBehavior(ActorState currentState, float deltaTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            float axisLeftRight = keyboard.IsKeyDown(Key.A) ? -1.0f : keyboard.IsKeyDown(Key.D) ? 1.0f : 0.0f;
            if (State != PlayerState.Free && OnGround > 0)
            {
                axisLeftRight = 0f;
            }

            // run
            if (State == PlayerState.Free)
            {
                Actor.ActorStateBehavior.Running(new Vector2(axisLeftRight * Actor.ActorStats.MoveSpeed, 0f));
            }

            // jump
            if (keyboard.IsKeyDown(Key.Space) && State == PlayerState.Free)
            {
                ((PlayerStateBehavior)Actor.ActorStateBehavior).Jump(OnGround);
            }

            // roll
            if (keyboard.IsKeyDown(Key.ShiftLeft))
            {
                if (RollCooldown <= 0f && Actor.ActorStats.RollEnabled)
                {
                    State = PlayerState.Blocked;
                    RollCooldown = 1.0f;
                    RollTelegraph = 0.2f;
                    CanRoll = false;
                    ((PlayerStateBehavior)Actor.ActorStateBehavior).Roll();
                }
            }

            ControlRollVelocity(deltaTime);

            if (RollCooldown > 0f)
            {
                RollCooldown -= deltaTime;
            }

            // update animations
            ((PlayerStateBehavior)Actor.ActorStateBehavior).Idle();
            ((PlayerStateBehavior)Actor.ActorStateBehavior).Fall(OnGround);

            if (OnGround >= 1 && Actor.ActorStats.JumpCooldown > 0f)
            {
                Actor.ActorStats.JumpCooldown -= deltaTime;
            }

            return ActorState.Idle;
        }

        private void ControlRollVelocity(float deltaTime)
        {
            if (RollTelegraph > 0f)
            {
                RollTelegraph -= deltaTime;
                ((PlayerStateBehavior)Actor.ActorStateBehavior).SetXVelocity(Actor.FacingRight ? 20f : -20f);
            }
            else if (!CanRoll)
            {
                CanRoll = true;
                State = PlayerState.Free;
                ((PlayerStateBehavior)Actor.ActorStateBehavior).SetXVelocity(0f);
            }
        }

        public void ClearTimeInState()
        {
        }

        public void OnGroundTriggerExited(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Wall")
            {
                OnGround--;
            }
        }

        public void OnGroundTriggerEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Wall")
            {
                OnGround++;
                ((PlayerStateBehavior)Actor.ActorStateBehavior).Jumping = false;
            }
        }
    }
}