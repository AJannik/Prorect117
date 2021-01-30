using Game.Entity;
using Game.Interfaces;

namespace Game.Components.UI
{
    public class CTutorialWindow : IComponent
    {
        public GameObject MyGameObject { get; set; }

        private bool CanOpen { get; set; } = true;

        public void Close(object sender, int i)
        {
            MyGameObject.Active = false;
        }

        public void Open(object sender, IComponent e)
        {
            if (CanOpen && e.MyGameObject.Name == "Player")
            {
                CanOpen = false;
                MyGameObject.Active = true;
            }
        }
    }
}