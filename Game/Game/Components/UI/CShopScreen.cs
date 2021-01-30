using System.Collections.Generic;
using Game.Components.UI.BaseComponents;
using Game.Entity;
using Game.Interfaces;
using Game.Tools;

namespace Game.Components.UI
{
    public class CShopScreen : IComponent
    {
        public GameObject MyGameObject { get; set; }

        public GameObject Player { get; set; }

        public GameObject HealButton { get; set; }

        public List<GameObject> PowerDownDisplays { get; } = new List<GameObject>();

        private int HealPrice { get; } = 10;

        private int RemovePowerDownPrice { get; } = 15;

        public void OnContinue(object sender, int i)
        {
            MyGameObject.Active = false;
            MyGameObject.Scene.InvokeLoadLevelEvent(1);
        }

        public void Show()
        {
            MyGameObject.Active = true;
            MyGameObject.Scene.RemoveGameObject(Player);

            if (MyGameObject.Scene.GameManager.Coins < HealPrice)
            {
                HealButton.GetComponent<CButton>().Active = false;
            }

            if (MyGameObject.Scene.GameManager.Coins < RemovePowerDownPrice)
            {
                foreach (GameObject gameObject in PowerDownDisplays)
                {
                    gameObject.GetChild(0).GetComponent<CButton>().Active = false;
                }
            }

            DisplayPowerDowns();
            DeactivateButtons();
        }

        public void BuyHealth(object sender, int i)
        {
            if (MyGameObject.Scene.GameManager.Coins >= HealPrice && MyGameObject.Scene.GameManager.PlayerHealth < 95f)
            {
                MyGameObject.Scene.GameManager.PlayerHealth += 10f;
                MyGameObject.Scene.GameManager.Coins -= HealPrice;

                // Clamp health to max
                if (MyGameObject.Scene.GameManager.PlayerHealth > 100f)
                {
                    MyGameObject.Scene.GameManager.PlayerHealth = 100f;
                }
            }

            DeactivateButtons();
        }

        public void RemovePowerDown(object sender, int type)
        {
            if (MyGameObject.Scene.GameManager.Coins >= RemovePowerDownPrice)
            {
                MyGameObject.Scene.GameManager.Coins -= RemovePowerDownPrice;
                MyGameObject.Scene.GameManager.RemoveEffectOfType((EffectType)type);
                DisplayPowerDowns();
            }

            DeactivateButtons();
        }

        private void DisplayPowerDowns()
        {
            for (int i = 0; i < PowerDownDisplays.Count; i++)
            {
                int num = MyGameObject.Scene.GameManager.NumEffectTypeInEffects((EffectType)i);
                PowerDownDisplays[i].GetComponent<CGuiTextRender>().Text = $"{num}x {(EffectType)i}";
                if (num == 0 || MyGameObject.Scene.GameManager.Coins < RemovePowerDownPrice)
                {
                    PowerDownDisplays[i].GetChild(0).GetComponent<CButton>().Active = false;
                }
            }
        }

        private void DeactivateButtons()
        {
            if (MyGameObject.Scene.GameManager.Coins < HealPrice || MyGameObject.Scene.GameManager.PlayerHealth > 95f)
            {
                HealButton.GetComponent<CButton>().Active = false;
            }

            if (MyGameObject.Scene.GameManager.Coins < RemovePowerDownPrice)
            {
                foreach (GameObject gameObject in PowerDownDisplays)
                {
                    gameObject.GetChild(0).GetComponent<CButton>().Active = false;
                }
            }
        }
    }
}