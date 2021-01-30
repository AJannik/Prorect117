using OpenTK;

namespace Game.Physics
{
    internal static class PhysicConstants
    {
        public static Vector2 Gravity { get; } = new Vector2(0f, -10f);

        public static float FixedDeltaTime { get; } = 1f / 40f;
    }
}