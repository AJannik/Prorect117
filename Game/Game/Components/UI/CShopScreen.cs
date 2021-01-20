using Game.Interfaces;

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

            if (MyGameObject.Scene.GameManager.Coins < HealPrice)
            {
                HealButton.GetComponent<CButton>().Active = false;
            }
        }
    }
}