using Game.Components.UI.BaseComponents;
using Game.Entity;
using Game.Interfaces;

namespace Game.Components.UI
{
    public class CGameOverUI : IComponent, IOnStart
    {
        public GameObject MyGameObject { get; set; }

        public CGuiTextRender CoinsText { get; set; }

        public CGuiTextRender Title { get; set; }

        public void Start()
        {
            CoinsText.Text = MyGameObject.Scene.GameManager.Coins.ToString();
            if (MyGameObject.Scene.GameManager.PlayerWon)
            {
                Title.Text = "YOU WON!";
            }
            else
            {
                Title.Text = "YOU DIED!";
                CoinsText.MyGameObject.Active = false;
                CoinsText.MyGameObject.GetParent().GetChild(0).GetComponent<CGuiTextRender>().Text = "ALL YOUR COINS HAVE BEEN LOST";
            }
        }

        public void OnBtnExit(object sender, int i)
        {
            MyGameObject.Scene.ExitGame();
        }

        public void OnBtnReturn(object sender, int i)
        {
            MyGameObject.Scene.GameManager.Restart();
        }
    }
}