﻿using System;
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
            if (!MyGameObject.Active)
            {
                return;
            }
        }

        public void SetupTrigger(CCircleCollider trigger)
        {
            Trigger = trigger;
            Trigger.IsTrigger = true;
            Trigger.TriggerEntered += OnTriggerEntered;
        }

        private void OnTriggerEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                if (e.MyGameObject.GetComponent<CEffectSystem>() != null)
                {
                    e.MyGameObject.GetComponent<CEffectSystem>().AddEffect(EffectType.Slow);
                }
            }
        }
    }
}
