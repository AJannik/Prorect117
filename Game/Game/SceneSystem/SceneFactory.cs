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
            ObjectFactory.BuildPlatform4(scene, new Vector2(0f, -4f));
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
            ObjectFactory.BuildPlatform4(scene, new Vector2(0f, -4f));
            ObjectFactory.BuildPlatform4(scene, new Vector2(5f, -4f));
            ObjectFactory.BuildGround(scene, new Vector2(0f, -6f));

            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(0f, 3.0f));
            camera.SetParent(player);

            camera.GetComponent<CCamera>().Scale = 3f;

            return scene;
        }

        private Scene BuildScene0()
        {
            Scene scene = new Scene();

            ObjectFactory.BuildGround(scene, new Vector2(0f, -0.1f));
            ObjectFactory.BuildLevelWall(scene, new Vector2(-16.1f, 0f));
            ObjectFactory.BuildLevelWall(scene, new Vector2(16.1f, 0f));
            ObjectFactory.BuildGround(scene, new Vector2(0f, 15.9f));
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(0f, 15f));

            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 12f;

            ObjectFactory.BuildPlatform2(scene, new Vector2(-11f, 2f));
            ObjectFactory.BuildPlatform2(scene, new Vector2(-15f, 5f));
            ObjectFactory.BuildPlatform4(scene, new Vector2(-8f, 5f));
            ObjectFactory.BuildPlatform4(scene, new Vector2(-4f, 5f));
            ObjectFactory.BuildPlatform3(scene, new Vector2(-12f, 8f));
            ObjectFactory.BuildPlatform2(scene, new Vector2(-6f, 7.1f));
            ObjectFactory.BuildPlatform3(scene, new Vector2(-3.5f, 2.1f));
            ObjectFactory.BuildPlatform4(scene, new Vector2(0f, 2.1f));
            ObjectFactory.BuildPlatform4(scene, new Vector2(0f, 9f));
            ObjectFactory.BuildPlatform3(scene, new Vector2(3.5f, 9f));
            ObjectFactory.BuildPlatform4(scene, new Vector2(10f, 5f));
            ObjectFactory.BuildPlatform4(scene, new Vector2(6f, 5f));
            ObjectFactory.BuildPlatform3(scene, new Vector2(2.5f, 5f));
            ObjectFactory.BuildPlatform4(scene, new Vector2(14f, 9f));

            ObjectFactory.BuildWall3(scene, new Vector2(-8f, 1.5f));
            ObjectFactory.BuildWall2(scene, new Vector2(-8f, 4f));
            ObjectFactory.BuildWall4(scene, new Vector2(-7f, 9f));
            ObjectFactory.BuildWall3(scene, new Vector2(-7f, 12.5f));
            ObjectFactory.BuildWall2(scene, new Vector2(-7f, 15f));
            ObjectFactory.BuildWall3(scene, new Vector2(-2.1f, 3.5f));
            ObjectFactory.BuildWall4(scene, new Vector2(-2.1f, 7.1f));
            ObjectFactory.BuildWall3(scene, new Vector2(6f, 1.5f));
            ObjectFactory.BuildWall2(scene, new Vector2(6f, 4f));


            ObjectFactory.BuildSkeletonEnemy(scene, new Vector2(10f, 1f));

            return scene;
        }
    }
}