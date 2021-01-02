using Game.Interfaces;

namespace Game.Components.UI
{
    public class CCoinUIUpdater : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; }

        public CGuiTextRender TextRender { get; set; }

        public void Update(float deltaTime)
        {
            TextRender.Text = MyGameObject.Scene.GameManager.Coins.ToString();
        }
    }
}
