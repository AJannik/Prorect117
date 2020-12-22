using OpenTK;

namespace Game.Interfaces
{
    public interface IReadonlyCircle
    {
        Vector2 Center { get; }

        float Radius { get; }
    }
}