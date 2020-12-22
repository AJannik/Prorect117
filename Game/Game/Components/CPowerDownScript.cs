using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;

namespace Game.Components
{
    public class CPowerDownScript : IComponent
    {
        public CPowerDownScript()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public CCircleCollider Trigger { get; set; }

        public void Update(float deltaTime)
        {
            if (!MyGameObject.getActive()) return;
            if (Trigger == null)
            {
                Trigger = MyGameObject.GetComponent<CCircleCollider>();
                Trigger.TriggerEntered += OnTriggerEntered;
            }
        }

        private void OnTriggerEntered(object sender, ICollider e)
        {
            // TODO: implement effect
        }
    }
}
