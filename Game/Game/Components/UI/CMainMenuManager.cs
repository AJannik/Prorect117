using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;

namespace Game.Components.UI
{
    public class CMainMenuManager : IComponent
    {
        public GameObject MyGameObject { get; set; }

        public void OnButtonClick(object sender, int num)
        {
            (sender as CButton).MyGameObject.Transform.Position = new OpenTK.Vector2(0.5f, -0.5f);
        }
    }
}