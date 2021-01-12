﻿using Game.Components.Collision;
using Game.Interfaces;
using Game.Tools;

namespace Game.Components
{
    public class CPowerDownScript : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public CCircleTrigger Trigger { get; set; }

        public COpenDoor OpenDoor { get; set; } = null;

        public EffectType Effect { get; set; } = EffectType.Slow;

        public float EffectDuration { get; set; } = 10f;

        public int EffectStrength { get; set; } = 1;

        public void SetupTrigger(CCircleTrigger trigger)
        {
            Trigger = trigger;
            Trigger.TriggerEntered += OnTriggerEntered;
        }

        private void OnTriggerEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                if (e.MyGameObject.GetComponent<CEffectSystem>() != null)
                {
                    e.MyGameObject.GetComponent<CEffectSystem>().AddEffect(Effect, EffectDuration, EffectStrength);
                }

                if (OpenDoor != null)
                {
                    OpenDoor.IsOpen = true;
                }
            }
        }
    }
}
