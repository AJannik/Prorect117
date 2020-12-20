using System;
using Game.Components;
using Game.GameObjectFactory;
using OpenTK;

namespace Game.SceneSystem
{
    internal class SceneFactory
    {
        internal int NumScenes { get; } = 2;

        public Scene BuildScene(int num)
        {
            switch (num)
            {
                case 0:
                    return BuildScene0();
                case 1:
                    return BuildScene1();
                default:
                    throw new ArgumentOutOfRangeException($"There is no Scene numbered {num}!");
            }
        }

        private Scene BuildScene0()
        {
            Scene scene = new Scene();

            ObjectFactory.BuildFloor(scene, new Vector2(0f, -4f));
            GameObject ball = ObjectFactory.BuildBall(scene, Vector2.Zero);
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);

            camera.SetParent(ball);
            camera.GetComponent<CCamera>().Scale = 2f;

            return scene;
        }

        private Scene BuildScene1()
        {
            Scene scene = new Scene();

            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            GameObject parentQuad = ObjectFactory.BuildSprite(scene, Vector2.Zero);
            GameObject childQuad = ObjectFactory.BuildSprite(scene, new Vector2(3f, 0f));
            GameObject childChildQuad = ObjectFactory.BuildSprite(scene, new Vector2(1.5f, 0f));

            camera.GetComponent<CCamera>().Scale = 5f;
            childQuad.SetParent(parentQuad);
            childChildQuad.SetParent(childQuad);
            childQuad.Transform.Scale = new Vector2(0.8f, 0.8f);
            childChildQuad.Transform.Scale = new Vector2(0.8f, 0.8f);

            return scene;
        }
    }
}