using System;
using Game.Components.Collision;
using Game.Components.Player;
using Game.Interfaces;
using Game.SceneSystem;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CCollectible : IComponent, IOnStart
    {
        public GameObject MyGameObject { get; set; } = null;

        private CBoxTrigger Trigger { get; set; }

        private CPickupDisplay PickupDisplay { get; set; }

        public void Start()
        {
            foreach (GameObject gameObject in MyGameObject.Scene.GetGameObjects())
            {
                if (gameObject.Name == "Player")
                {
                    foreach (GameObject child in gameObject.GetAllChildren())
                    {
                        if (child.GetComponent<CPickupDisplay>() != null)
                        {
                            PickupDisplay = child.GetComponent<CPickupDisplay>();
                            return;
                        }
                    }
                }
            }
        }

        public void SetupTrigger(CBoxTrigger trigger)
        {
            Trigger = trigger;
            Trigger.TriggerEntered += Triggered;
        }

        private void Triggered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                if (MyGameObject.Name == "Coin")
                {
                    e.MyGameObject.Scene.GameManager.Coins++;
                    PickupDisplay.AddCoins(1);
                }
                else if (MyGameObject.Name == "Key")
                {
                    if (!e.MyGameObject.Scene.GameManager.Key)
                    {
                        e.MyGameObject.Scene.GameManager.Key = true;
                        PickupDisplay.AddKey();
                    }

                    e.MyGameObject.Scene.GameManager.Coins += 9;
                    PickupDisplay.AddCoins(9);
                }

                MyGameObject.Scene.RemoveGameObject(MyGameObject);
            }
        }
    }
}