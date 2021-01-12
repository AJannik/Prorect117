using System;
using Game.Components.Collision;
using Game.Interfaces;
using Game.SceneSystem;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CCollectible : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public CBoxTrigger Trigger { get; set; }

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
                }
                else if (MyGameObject.Name == "Key")
                {
                    if (e.MyGameObject.Scene.GameManager.Key)
                    {
                        e.MyGameObject.Scene.GameManager.Coins += 10;
                    }
                    else
                    {
                        e.MyGameObject.Scene.GameManager.Key = true;
                    }
                }

                MyGameObject.Scene.RemoveGameObject(MyGameObject);
            }
        }
    }
}