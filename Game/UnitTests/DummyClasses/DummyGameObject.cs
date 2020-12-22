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
            Scene scene = new Scene();
            DummyGameObject dummy = new DummyGameObject(scene);

            return dummy;
        }
    }
}
