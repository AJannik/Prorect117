using Game.Components.Collision;
using Game.Components.Player;
using Game.Entity;
using Game.Interfaces;
using Game.Tools;

namespace Game.Components
{
    public class CPowerDownScript : IComponent, IOnStart
    {
        public GameObject MyGameObject { get; set; } = null;

        public CCircleTrigger Trigger { get; set; }

        public EffectType Effect { get; set; } = EffectType.Slow;

        private CPickupDisplay PickupDisplay { get; set; }

        private float EffectStrength { get; } = 0.15f;

        public void SetupTrigger(CCircleTrigger trigger)
        {
            Trigger = trigger;
            Trigger.TriggerEntered += OnTriggerEntered;
        }

        public void Start()
        {
            SetupPickupDisplay();
        }

        private void OnTriggerEntered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name != "Player" || !MyGameObject.Active)
            {
                return;
            }

            if (e.MyGameObject.GetComponent<CEffectSystem>() != null)
            {
                e.MyGameObject.GetComponent<CEffectSystem>().AddEffect(Effect, EffectStrength);
                MyGameObject.Scene.GameManager.Coins += 5;
                PickupDisplay.AddCoins(5);
                PickupDisplay.AddPowerDown(Effect);
            }

            MyGameObject.Scene.RemoveGameObject(MyGameObject);
        }

        private void SetupPickupDisplay()
        {
            foreach (GameObject gameObject in MyGameObject.Scene.GetGameObjects())
            {
                if (gameObject.Name != "Player")
                {
                    continue;
                }

                foreach (GameObject child in gameObject.GetAllChildren())
                {
                    if (child.GetComponent<CPickupDisplay>() == null)
                    {
                        continue;
                    }

                    PickupDisplay = child.GetComponent<CPickupDisplay>();
                    return;
                }
            }
        }
    }
}