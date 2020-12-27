using System;
using Game.Components;
using Game.GameObjectFactory;
using OpenTK;

namespace Game.SceneSystem
{
    internal class SceneFactory
    {
        internal int NumScenes { get; } = 4;

        public Scene BuildScene(int num)
        {
            switch (num)
            {
                case 0:
                    return BuildScene0();
                case 1:
                    return BuildScene1();
                case 2:
                    return BuildScene2();
                case 3:
                    return BuildScene3();
                default:
                    throw new ArgumentOutOfRangeException($"There is no Scene numbered {num}!");
            }
        }

        private Scene BuildScene3()
        {
            Scene scene = new Scene();

            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            ObjectFactory.BuildPlatform(scene, new Vector2(0f, -4f), 4);
            GameObject ball = ObjectFactory.BuildBall(scene, Vector2.Zero);

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

        private Scene BuildScene2()
        {
            Scene scene = new Scene();

            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            ObjectFactory.BuildPlatform(scene, new Vector2(0f, -4f), 4);
            ObjectFactory.BuildPlatform(scene, new Vector2(5f, -4f), 4);
            ObjectFactory.BuildPlatform(scene, new Vector2(0f, -6f), 32);

            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(0f, 3.0f));
            camera.SetParent(player);

            camera.GetComponent<CCamera>().Scale = 3f;

            return scene;
        }

        private Scene BuildScene0()
        {
            Scene scene = new Scene();

            ObjectFactory.BuildPlatform(scene, new Vector2(0f, -0.1f), 32);
            ObjectFactory.BuildWall(scene, new Vector2(-15.9f, 7f), 14);
            ObjectFactory.BuildWall(scene, new Vector2(15.9f, 7f), 14);
            ObjectFactory.BuildPlatform(scene, new Vector2(0f, 14.1f), 32);
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(0f, 10f));

            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 12f;

            ObjectFactory.BuildPlatform(scene, new Vector2(-11f, 2f), 2);
            ObjectFactory.BuildPlatform(scene, new Vector2(-14.8f, 5f), 2);
            ObjectFactory.BuildPlatform(scene, new Vector2(-6f, 5.1f), 8);
            ObjectFactory.BuildPlatform(scene, new Vector2(-12f, 10f), 3);
            ObjectFactory.BuildPlatform(scene, new Vector2(-6.6f, 7.9f), 3);
            ObjectFactory.BuildPlatform(scene, new Vector2(-1.5f, 2.1f), 7);
            ObjectFactory.BuildPlatform(scene, new Vector2(1.5f, 9.3f), 7);
            ObjectFactory.BuildPlatform(scene, new Vector2(6.5f, 5.1f), 11);
            ObjectFactory.BuildPlatform(scene, new Vector2(13.8f, 9f), 4);

            ObjectFactory.BuildWall(scene, new Vector2(-8f, 2.5f), 5);
            ObjectFactory.BuildWall(scene, new Vector2(-7f, 11f), 6);
            ObjectFactory.BuildWall(scene, new Vector2(-1.9f, 5.7f), 7);
            ObjectFactory.BuildWall(scene, new Vector2(6f, 2.5f), 5);

            ObjectFactory.BuildSkeletonEnemy(scene, new Vector2(10f, 1f));

            return scene;
        }
    }
}