using Game;
using Game.SceneSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.DummyClasses
{
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
