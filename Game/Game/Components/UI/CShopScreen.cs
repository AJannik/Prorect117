using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;

namespace Game.Components.UI
{
    public class CShopScreen : IComponent
    {
        public GameObject MyGameObject { get; set; }

        public void OnContinue(object sender, int i)
        {
            MyGameObject.Active = false;
            MyGameObject.Scene.LoadLevelEvent(1);
        }

        public void Show(object sender, int i)
        {
            MyGameObject.Active = true;
        }

        public void BuyHealth(object sender, int i)
        {
            if (MyGameObject.Scene.GameManager.Coins > 14 && MyGameObject.Scene.GameManager.PlayerHealth < 100f)
            {
                MyGameObject.Scene.GameManager.PlayerHealth += 10f;

                // Clamp health to max
                if (MyGameObject.Scene.GameManager.PlayerHealth > 100f)
                {
                    MyGameObject.Scene.GameManager.PlayerHealth = 100f;
                }
            }
        }
    }
}