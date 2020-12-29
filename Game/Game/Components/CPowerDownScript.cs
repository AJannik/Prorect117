using Game.Interfaces;
using Game.Tools;

namespace Game.Components
{
    public class CPowerDownScript : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public CCircleTrigger Trigger { get; set; }

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
                    e.MyGameObject.GetComponent<CEffectSystem>().AddEffect(EffectType.Slow);
                }
            }
        }
    }
}
