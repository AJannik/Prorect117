using Game.Components.Collision;
using Game.Interfaces;

namespace Game.Components
{
    public class CDoor : IComponent
    {
        private bool unlocked = false;

        public GameObject MyGameObject { get; set; } = null;

        public CBoxTrigger Trigger { get; set; }

        public void SetupTrigger(CBoxTrigger trigger)
        {
            Trigger = trigger;
            Trigger.TriggerEntered += Triggered;
            Trigger.TriggerExited += UnTriggered;
        }

        private void Triggered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                if (e.MyGameObject.Scene.GameManager.Key)
                {
                    e.MyGameObject.Scene.GameManager.Key = false;
                    unlocked = true;
                }

                if (unlocked)
                {
                    // TODO: Add button that opens the shop and set visible
                }
            }
        }

        private void UnTriggered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                if (e.MyGameObject.Scene.GameManager.Key)
                {
                    e.MyGameObject.Scene.GameManager.Key = false;
                    unlocked = true;
                }

                if (unlocked)
                {
                    // TODO: Make shop button invisible
                }
            }
        }
    }
}