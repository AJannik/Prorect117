using Game.Components.Collision;
using Game.Components.Player;
using Game.Entity;
using Game.Interfaces;

namespace Game.Components
{
    public class CCollectible : IComponent, IOnStart
    {
        public GameObject MyGameObject { get; set; } = null;

        private CBoxTrigger Trigger { get; set; }

        private CPickupDisplay PickupDisplay { get; set; }

        public void Start()
        {
            SetupPickupDisplay();
        }

        public void SetupTrigger(CBoxTrigger trigger)
        {
            Trigger = trigger;
            Trigger.TriggerEntered += Triggered;
        }

        private void Triggered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name != "Player")
            {
                return;
            }

            switch (MyGameObject.Name)
            {
                case "Coin":
                    AddCoins(e);
                    break;
                case "Key":
                    AddKey(e);
                    break;
            }

            MyGameObject.Scene.RemoveGameObject(MyGameObject);
        }

        private void AddKey(IComponent e)
        {
            if (!e.MyGameObject.Scene.GameManager.Key)
            {
                e.MyGameObject.Scene.GameManager.Key = true;
                PickupDisplay.AddKey();
            }

            e.MyGameObject.Scene.GameManager.Coins += 9;
            PickupDisplay.AddCoins(9);
        }

        private void AddCoins(IComponent e)
        {
            e.MyGameObject.Scene.GameManager.Coins++;
            PickupDisplay.AddCoins(1);
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