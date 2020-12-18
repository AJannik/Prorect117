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

        public float PlayerSpeed { get; private set; } = 10f;

        public float JumpForce { get; private set; } = 10f;

        private float JumpCooldown { get; set; }

        private bool Jumping { get; set; } = false;

        public void Update(float deltaTime)
        {
            var keyboard = Keyboard.GetState();

            float axisLeftRight = keyboard.IsKeyDown(Key.A) ? -1.0f : keyboard.IsKeyDown(Key.D) ? 1.0f : 0.0f;
            RigidBody.AddForce(new OpenTK.Vector2(axisLeftRight * PlayerSpeed, 0f));

            // if(onGround && JumpCooldown > 0)
            // JumpCooldown -= deltaTime;
        }

        private void Jump()
        {
            // and not onGround
            if (!Jumping)
            {
                RigidBody.AddForce(new OpenTK.Vector2(0, JumpForce));
                JumpCooldown = 0.1f;
            }
        }
    }
}
