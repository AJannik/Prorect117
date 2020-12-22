using OpenTK;

namespace Game.Interfaces
{
    public interface ISimpleGeometry
    {
        public Vector2 Center { get; set; }

        public Vector2 Size { get; set; }

        public Vector2 PhysicOffset { get; set; }
    }
}