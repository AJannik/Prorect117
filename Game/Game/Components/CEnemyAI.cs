using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Game.Interfaces;

namespace Game.Components
{
    public class CEnemyAI : IComponent
    {
        public CEnemyAI()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public CRigidBody RigidBody { get; set; }

        public CBoxCollider LeftTrigger { get; set; }

        public CBoxCollider RightTrigger { get; set; }

        public float MoveSpeed { get; private set; } = 5f;

        public void Update(float deltaTime)
        {
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
                RigidBody.Velocity = new OpenTK.Vector2(1 * MoveSpeed, RigidBody.Velocity.Y);
            }
        }

        private void LeftFootExited(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Ground")
            {
                RigidBody.Velocity = new OpenTK.Vector2(1 * MoveSpeed, RigidBody.Velocity.Y);
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
                RigidBody.Velocity = new OpenTK.Vector2(-1 * MoveSpeed, RigidBody.Velocity.Y);
            }
        }

        private void RightFootExited(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Floor" || e.MyGameObject.Name == "Ground")
            {
                RigidBody.Velocity = new OpenTK.Vector2(-1 * MoveSpeed, RigidBody.Velocity.Y);
            }
        }
    }
}
