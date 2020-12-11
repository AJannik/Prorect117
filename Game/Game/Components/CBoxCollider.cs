using System;
using Game.Interfaces;
using Game.SimpleGeometry;

namespace Game.Components
{
    public class CBoxCollider : IComponent, ICollider
    {
        public GameObject MyGameObject { get; set; } = null;

        public ISimpleGeometry Geometry { get; set; } = new Rect(0f, 0f, 0.2f, 0.2f);

        public bool IsTrigger { get; set; } = false;

        public void Update(float deltaTime)
        {
            Geometry.Center = MyGameObject.Transform.WorldPosition;
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }
    }
}