using System;
using System.Collections.Generic;
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

        public CBoxCollider LeftFootTrigger { get; set; }

        public CBoxCollider RightFootTrigger { get; set; }

        public void Update(float deltaTime)
        {
        }

        public void SetupLeftTrigger(CBoxCollider trigger)
        {
            LeftFootTrigger = trigger;
            LeftFootTrigger.TriggerExited += LeftFootExited;
        }

        public void SetupRightTrigger(CBoxCollider trigger)
        {
            RightFootTrigger = trigger;
            RightFootTrigger.TriggerExited += RightFootExited;
        }

        private void RightFootExited(object sender, ICollider e)
        {
        }

        private void LeftFootExited(object sender, ICollider e)
        {
        }
    }
}
