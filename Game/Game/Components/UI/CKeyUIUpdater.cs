using System;
using System.Collections.Generic;
using System.Text;
using Game.Components.UI.BaseComponents;
using Game.Entity;
using Game.Interfaces;

namespace Game.Components.UI
{
    public class CKeyUIUpdater : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; }

        public CImageRender KeyActive { get; set; }

        public CImageRender KeyInactive { get; set; }

        public void Update(float deltaTime)
        {
            if (MyGameObject.Scene.GameManager.Key)
            {
                KeyActive.Visible = true;
                KeyInactive.Visible = false;
            }
            else
            {
                KeyActive.Visible = false;
                KeyInactive.Visible = true;
            }
        }
    }
}