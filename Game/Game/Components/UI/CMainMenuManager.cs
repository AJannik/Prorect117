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
            // TODO: Remove
            MyGameObject.Scene.GameManager.Coins++;
        }
    }
}