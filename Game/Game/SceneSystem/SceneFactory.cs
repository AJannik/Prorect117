using System;
using Game.Components;
using Game.Components.Renderer;
using Game.Components.UI;
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
                    return BuildLevel0();
                case 1:
                    return BuildLevel1();
                default:
                    throw new ArgumentOutOfRangeException($"There is no Scene numbered {num}!");
            }
        }

        private Scene BuildLevel0()
        {
            Scene scene = new Scene();

            // Level border
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16f, 0.5f), 32);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16f, 15.5f), 32);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(0.5f, 8f), 14);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(31.5f, 8f), 14);

            // Level platforms
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(2f, 5.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(3.5f, 12.5f), 5);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(11.5f, 7.5f), 11);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(6f, 3.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(11f, 10.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16f, 3.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(20f, 11.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(24f, 5.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(29f, 9.5f), 4);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(30f, 12.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(30f, 3.5f), 2);

            // Level walls
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(9.5f, 13f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(9.5f, 4f), 6);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(17.5f, 7.5f), 7);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(24.5f, 3f), 4);

            // Player, exit and camera
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(2.5f, 2f));
            ObjectFactory.BuildLevelEnd(scene, new Vector2(15.5f, 5.0f), new Vector2(1, 2));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 12f;

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Coin UI
            GameObject coin = GuiFactory.BuildGuiImage(scene, canvas, new Vector2(0.9f, 0.9f), "goldcoin1.png");
            coin.GetComponent<CImageRender>().SetSize(0.1f, 0.1f);
            GameObject coinText = GuiFactory.BuildTextField(scene, canvas, new Vector2(0.85f, 0.87f), "0");
            coinText.GetComponent<CGuiTextRender>().Centered = true;
            coinText.GetComponent<CGuiTextRender>().Size = 0.05f;

            // Enemies
            EnemyFactory.BuildBanditEnemy(scene, new Vector2(22.5f, 2f));
            EnemyFactory.BuildBanditEnemy(scene, new Vector2(27.5f, 2f));

            // Collectables
            ObjectFactory.BuildKey(scene, new Vector2(20.5f, 12.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(3.5f, 13.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(11.5f, 11.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(30.5f, 13.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(30.5f, 1.5f));

            return scene;
        }

        private Scene BuildLevel1()
        {
            Scene scene = new Scene();

            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(-13f, 1f));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 12f;

            return scene;
        }
    }
}