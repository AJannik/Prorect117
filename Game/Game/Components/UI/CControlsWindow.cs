using Game.Entity;
using Game.Interfaces;

namespace Game.Components.UI
{
    public class CControlsWindow : IComponent
    {
        public GameObject MyGameObject { get; set; }

        public void OnBtnClick(object sender, int i)
        {
            MyGameObject.Active = false;
        }
    }
}