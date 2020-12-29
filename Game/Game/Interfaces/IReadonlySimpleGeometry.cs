using OpenTK;

namespace Game.Interfaces
{
    public interface IReadonlySimpleGeometry
    {
        public Vector2 Center { get; }

        public Vector2 Size { get; }

        public Vector2 PhysicOffset { get; }
    }
}