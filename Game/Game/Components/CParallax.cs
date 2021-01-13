using System;
using System.Collections.Generic;
using System.Text;
using Game.Components.Renderer;
using Game.Interfaces;

namespace Game.Components
{
    public class CParallax : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; } = null;

        public CRender Render { get; set; }

        public CTransform Target { get; set; }

        public float Depth { get; set; }

        public void Update(float deltaTime)
        {
            float offset = (1f / Depth) * Target.WorldPosition.X * 0.01f;
            Render.SetTexCoords(new SimpleGeometry.Rect(0.0f + offset, 0.0f, 1.0f, 1.0f));
        }
    }
}
