using Game.Entity;
using Game.SceneSystem;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace UnitTests.DummyClasses
{
    [ExcludeFromCodeCoverage]
    public class DummyGameObject : GameObject
    {
        public DummyGameObject(Scene scene) : base(scene, "Dummy")
        {
        }

        public static DummyGameObject BuildDummy()
        {
            GameManager gameManager = new GameManager();
            Scene scene = new Scene(gameManager);
            DummyGameObject dummy = new DummyGameObject(scene);

            return dummy;
        }
    }
}
