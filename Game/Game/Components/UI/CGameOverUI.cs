using Game.Interfaces;

namespace Game.Components.UI
{
    public class CGameOverUI : IComponent, IOnStart
    {
        public GameObject MyGameObject { get; set; }

        public CGuiTextRender CoinsText { get; set; }

        public void Start()
        {
            CoinsText.Text = MyGameObject.Scene.GameManager.Coins.ToString();
        }

        public void OnBtnExit(object sender, int i)
        {
            MyGameObject.Scene.ExitGame();
        }
    }
}