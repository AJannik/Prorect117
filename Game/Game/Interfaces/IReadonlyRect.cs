using OpenTK;

namespace Game.Interfaces
{
    public interface IReadonlyRect
    {
        Vector2 Center { get; }

        float MaxX { get; }

        float MaxY { get; }

        float MinX { get; }

        float MinY { get; }

        Vector2 Size { get; }
    }
}