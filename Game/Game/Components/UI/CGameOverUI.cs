using Game.Components.UI.BaseComponents;
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