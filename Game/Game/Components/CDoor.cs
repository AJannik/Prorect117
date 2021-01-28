using Game.Components.Renderer;
using Game.Components.UI;
using Game.Entity;
using Game.Interfaces;
using OpenTK.Input;

namespace Game.Components
{
    public class CDoor : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; } = null;

        public CTextRender TextRender { get; set; }

        public CShopScreen ShopScreen { get; set; } = null;

        public bool LastLevel { get; set; } = false;

        private bool Unlockable { get; set; } = false;

        public void SetupTrigger(ITrigger trigger)
        {
            trigger.TriggerEntered += Triggered;
            trigger.TriggerExited += UnTriggered;
        }

        public void Update(float deltaTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (Unlockable && keyboard.IsKeyDown(Key.E))
            {
                MyGameObject.Scene.GameManager.Key = false;
                if (!LastLevel)
                {
                    ShopScreen.Show();
                    Unlockable = false;
                }
                else
                {
                    MyGameObject.Scene.GameManager.PlayerWon = true;
                    MyGameObject.Scene.GameManager.EndGame();
                }
            }
        }

        private void Triggered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                if (e.MyGameObject.Scene.GameManager.Key)
                {
                    TextRender.Text = "E";
                    Unlockable = true;
                }
                else
                {
                    TextRender.Text = "YOU NEED A KEY!";
                    Unlockable = false;
                }

                TextRender.Visible = true;
            }
        }

        private void UnTriggered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                TextRender.Visible = false;
                Unlockable = false;
            }
        }
    }
}