using OpenTK;

namespace Game.Interfaces
{
    public interface ICollider : IComponent
    {
        public Vector2 Offset { get; set; }

        public ISimpleGeometry Geometry { get; set; }
    }
}