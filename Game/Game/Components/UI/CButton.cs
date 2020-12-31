using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;
using OpenTK.Input;

namespace Game.Components.UI
{
    public class CButton : IComponent, IMouseListener
    {
        public GameObject MyGameObject { get; set; } = null;

        public void MouseEvent(MouseButtonEventArgs args)
        {
            // TODO: transform coords to Canvas coords
            // TODO: Check IsPointInCollider() for coords
            // TODO: if true then throw ButtonClicked event
        }
    }
}