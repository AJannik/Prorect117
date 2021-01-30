using System;
using System.Collections.Generic;
using System.Text;
using Game.Entity;
using Game.Interfaces;

namespace Game.Components.UI
{
    public class CMainMenuManager : IComponent
    {
        public GameObject MyGameObject { get; set; }

        public void OnStartButton(object sender, int num)
        {
            MyGameObject.Scene.InvokeLoadLevelEvent(2);
        }

        public void OnTutorialButton(object sender, int num)
        {
            MyGameObject.Scene.InvokeLoadLevelEvent(1);
        }

        public void OnExitButton(object sender, int num)
        {
            MyGameObject.Scene.ExitGame();
        }
    }
}