using System;
using System.Collections.Generic;
using OpenTK;

namespace Game.Interfaces
{
    public interface ITrigger : IComponent
    {
        public event EventHandler<IComponent> TriggerEntered;

        public event EventHandler<IComponent> TriggerExited;

        public Vector2 Offset { get; set; }

        public ISimpleGeometry Geometry { get; set; }

        public IReadOnlyList<IComponent> GetTriggerHits();
    }
}