using Game.GameObjectFactory;
using Game.Interfaces;
using Game.Tools;
using OpenTK;

namespace Game.Components.UI
{
    public class CShopScreen : IComponent
    {
        public GameObject MyGameObject { get; set; }

        public GameObject Player { get; set; }

        public GameObject HealButton { get; set; }

        public int HealPrice { get; } = 15;

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

            DisplayPowerDowns();
        }

        public void BuyHealth(object sender, int i)
        {
            if (MyGameObject.Scene.GameManager.Coins >= HealPrice && MyGameObject.Scene.GameManager.PlayerHealth < 100f)
            {
                MyGameObject.Scene.GameManager.PlayerHealth += 10f;
                MyGameObject.Scene.GameManager.Coins -= 15;

                // Clamp health to max
                if (MyGameObject.Scene.GameManager.PlayerHealth > 100f)
                {
                    MyGameObject.Scene.GameManager.PlayerHealth = 100f;
                }
            }

            if (MyGameObject.Scene.GameManager.Coins < HealPrice || MyGameObject.Scene.GameManager.PlayerHealth > 95f)
            {
                HealButton.GetComponent<CButton>().Active = false;
            }
        }

        private void DisplayPowerDowns()
        {
            int i = 0;
            if (MyGameObject.Scene.GameManager.NumEffectTypeInEffects(EffectType.Fragile) > 0)
            {
                string text = $"{MyGameObject.Scene.GameManager.NumEffectTypeInEffects(EffectType.Fragile)}x {EffectType.Fragile}";
                Vector2 position = new Vector2(0.2f, 0.2f - (0.1f * i));
                GuiFactory.BuildShopPowerDown(MyGameObject.Scene, HealButton.GetComponent<CButton>().Canvas, MyGameObject, position, text);
                i++;
            }

            if (MyGameObject.Scene.GameManager.NumEffectTypeInEffects(EffectType.Silenced) > 0)
            {
                string text = $"{MyGameObject.Scene.GameManager.NumEffectTypeInEffects(EffectType.Silenced)}x {EffectType.Silenced}";
                Vector2 position = new Vector2(0.2f, 0.2f - (0.1f * i));
                GuiFactory.BuildShopPowerDown(MyGameObject.Scene, HealButton.GetComponent<CButton>().Canvas, MyGameObject, position, text);
                i++;
            }

            if (MyGameObject.Scene.GameManager.NumEffectTypeInEffects(EffectType.Slow) > 0)
            {
                string text = $"{MyGameObject.Scene.GameManager.NumEffectTypeInEffects(EffectType.Slow)}x {EffectType.Slow}";
                Vector2 position = new Vector2(0.2f, 0.2f - (0.1f * i));
                GuiFactory.BuildShopPowerDown(MyGameObject.Scene, HealButton.GetComponent<CButton>().Canvas, MyGameObject, position, text);
                i++;
            }

            if (MyGameObject.Scene.GameManager.NumEffectTypeInEffects(EffectType.Weakness) > 0)
            {
                string text = $"{MyGameObject.Scene.GameManager.NumEffectTypeInEffects(EffectType.Weakness)}x {EffectType.Weakness}";
                Vector2 position = new Vector2(0.2f, 0.2f - (0.1f * i));
                GuiFactory.BuildShopPowerDown(MyGameObject.Scene, HealButton.GetComponent<CButton>().Canvas, MyGameObject, position, text);
            }
        }
    }
}