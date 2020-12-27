using OpenTK;

namespace Game.Physics
{
    internal static class PhysicConstants
    {
        public static Vector2 Gravity { get; } = new Vector2(0f, -50f);

        public static float FixedUpdate { get; } = 1f / 150f;
    }
}