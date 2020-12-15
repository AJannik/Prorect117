using System;
using Game.Components;
using OpenTK;

namespace Game.Interfaces
{
    public interface ICollider
    {
        public event EventHandler<ICollider> TriggerEntered;

        public event EventHandler<ICollider> TriggerExited;

        public bool IsTrigger { get; set; }

        public Vector2 Offset { get; set; }

        public ISimpleGeometry Geometry { get; set; }

        public void DebugDraw();
    }
}