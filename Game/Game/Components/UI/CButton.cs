using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;
using Game.Tools;
using OpenTK;
using OpenTK.Input;

namespace Game.Components.UI
{
    public class CButton : IComponent, IMouseListener, IGuiElement
    {
        public GameObject MyGameObject { get; set; } = null;

        public CCanvas Canvas { get; set; }

        public void MouseEvent(MouseButtonEventArgs args)
        {
            Vector2 pos = new Vector2(args.X, args.Y);
            pos = Transformation.Transform(pos, Canvas.CanvasMouseMatrix);
            Console.WriteLine(pos);

            // TODO: Check IsPointInCollider() for coords
            // TODO: if true then throw ButtonClicked event
        }
    }
}