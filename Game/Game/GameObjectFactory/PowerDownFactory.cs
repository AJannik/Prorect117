using Game.SceneSystem;

namespace Game.GameObjectFactory
{
    public class PowerDownFactory
    {
        public static GameObject Slowness(Scene scene)
        {
            GameObject powerDown = new GameObject(scene, "Slowness");

            return powerDown;
        }

        public static GameObject Vulnerability(Scene scene)
        {
            GameObject powerDown = new GameObject(scene, "Vulnerability");

            return powerDown;
        }
    }
}