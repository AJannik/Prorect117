using Game.Components.Collision;
using Game.Interfaces;
using Game.Tools;

namespace Game.Components
{
    public class CPowerDownScript : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public CCircleTrigger Trigger { get; set; }

        public EffectType Effect { get; set; } = EffectType.Slow;

        private float EffectStrength { get; } = 0.15f;

        public void SetupTrigger(CCircleTrigger trigger)
        {
            Trigger = trigger;
            Trigger.TriggerEntered += OnTriggerEntered;
        }

        private void OnTriggerEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player" && MyGameObject.Active)
            {
                if (e.MyGameObject.GetComponent<CEffectSystem>() != null)
                {
                    e.MyGameObject.GetComponent<CEffectSystem>().AddEffect(Effect, EffectStrength);
                }

                MyGameObject.Active = false;
            }
        }
    }
}
