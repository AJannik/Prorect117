using System;
using System.Collections.Generic;
using OpenTK;

namespace Game.Interfaces
{
    public interface ICollider
    {
        public event EventHandler<IComponent> TriggerEntered;

        public event EventHandler<IComponent> TriggerExited;

        public bool IsTrigger { get; set; }

        public Vector2 Offset { get; set; }

        public ISimpleGeometry Geometry { get; set; }

        public void DebugDraw();

        public IReadOnlyList<IComponent> GetTriggerHits();
    }
}