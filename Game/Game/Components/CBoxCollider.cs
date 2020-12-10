using System;
using Game.Interfaces;
using Game.SimpleGeometry;

namespace Game.Components
{
    public class CBoxCollider : IComponent, ICollider
    {
        public GameObject MyGameObject { get; set; } = null;

        public Rect Rect { get; private set; } = new Rect(0f, 0f, 0.2f, 0.2f);

        public void Update(float deltaTime)
        {

        }
    }
}