using System;
using Game.Components;
using Game.Components.UI;
using Game.GameObjectFactory;
using OpenTK;

namespace Game.SceneSystem
{
    internal class SceneFactory
    {
        public SceneFactory(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        internal int NumScenes { get; } = 3;

        private GameManager GameManager { get; }

        public Scene BuildScene(int num)
        {
            switch (num)
            {
                case 0:
                    return BuildLevel0();
                case 1:
                    return BuildLevel1();
                case 2:
                    // Last scene
                    return BuildGameOverScene();
                default:
                    throw new ArgumentOutOfRangeException($"There is no Scene numbered {num}!");
            }
        }

        private Scene BuildLevel0()
        {
            Scene scene = new Scene(GameManager);

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
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(2.5f, 2.5f));
            GameObject levelEnd = StaticRigidbodyFactory.BuildLevelEnd(scene, new Vector2(15.5f, 5.0f), new Vector2(1, 2));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 6f;

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Shop
            GameObject shopScreen = GuiFactory.BuildShopScreen(scene, canvas, Vector2.Zero);
            levelEnd.GetComponent<CDoor>().ShopScreen = shopScreen.GetComponent<CShopScreen>();
            shopScreen.GetComponent<CShopScreen>().Player = player;
            shopScreen.Active = false;

            // Coin and Key UI
            GuiFactory.BuildCoinHUD(scene, canvas, new Vector2(0.85f, 0.9f));
            GuiFactory.BuildKeyUI(scene, canvas, new Vector2(0.7f, 0.9f));

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
            Scene scene = new Scene(GameManager);

            // Level border
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(64f, 15.5f), 128);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(19.5f, 0.5f), 39);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(42.5f, 0.5f), 3);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(48f, 0.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(78.5f, 0.5f), 31);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(115.5f, 0.5f), 3);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(0.5f, 8f), 14);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(127.5f, 9.5f), 11);

            // Level platforms
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(4f, 6.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(9f, 12.5f), 10);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(5f, 3.5f), 4);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(10.5f, 9.5f), 3);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(10.5f, 4.5f), 3);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(15f, 4.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(17.5f, 3.5f), 3);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(17.5f, 8.5f), 1);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(22.5f, 11.5f), 3);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(23f, 4.5f), 4);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(30f, 12.5f), 12);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(28f, 7.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(33f, 6.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(35f, 9.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(34f, 3.5f), 4);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(46.5f, 11.5f), 11);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(51f, 8.5f), 14);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(51.5f, 2.5f), 1);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(54.5f, 3.5f), 1);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(58f, 12.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(66.5f, 9.5f), 19);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(59f, 1.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(64f, 12.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(78.5f, 3.5f), 25);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(74f, 6.5f), 10);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(74.5f, 12.5f), 7);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(82f, 8.5f), 8);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(88f, 12.5f), 10);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(91.5f, 7.5f), 3);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(96.5f, 10.5f), 7);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(94.5f, 2.5f), 1);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(97.5f, 2.5f), 1);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(100.5f, 2.5f), 1);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(103.5f, 2.5f), 1);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(107f, 3.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(110f, 5.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(113f, 8.5f), 2);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(112.5f, 1.5f), 1);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(119f, 4.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(120f, 12.5f), 6);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(122.5f, 8.5f), 5);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(124.5f, 11.5f), 5);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(126f, 4.5f), 2);

            // Moving platforms
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(2f, 9.5f), new Vector2(2f, 12.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(15f, 7.5f), new Vector2(15f, 10.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(26f, 3.5f), new Vector2(26f, 7.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(40f, 0.5f), new Vector2(40f, 11.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(115.5f, 7.5f), new Vector2(115.5f, 12.5f), 1);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(118f, 0.5f), new Vector2(126f, 0.5f), 2);

            // Level walls
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(6.5f, 5f), 2);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(9.5f, 11f), 2);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(9.5f, 2.5f), 3);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(11.5f, 7f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(16.5f, 9.5f), 11);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(21.5f, 2.5f), 3);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(24.5f, 8.5f), 7);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(29.5f, 5.5f), 9);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(35.5f, 5f), 2);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(38.5f, 9f), 12);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(41.5f, 6f), 10);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(54.5f, 13f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(63.5f, 11f), 2);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(63.5f, 3.5f), 5);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(66.5f, 6.5f), 5);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(78.5f, 7.5f), 1);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(80.5f, 12f), 6);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(90.5f, 5.5f), 3);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(92.5f, 10f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(114.5f, 7f), 12);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(117.5f, 8.5f), 7);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(124.5f, 4.5f), 7);

            // Player, exit and camera
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(4.5f, 5.1f));
            GameObject levelEnd = StaticRigidbodyFactory.BuildLevelEnd(scene, new Vector2(125.5f, 13.5f), new Vector2(3, 3));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 6f;

            // Enemies
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(7.5f, 14f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(16.5f, 2f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(23.5f, 2f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(27.5f, 2f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(29.5f, 14f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(59.5f, 14f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(66.5f, 11f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(71.5f, 5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(72.5f, 11f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(72.5f, 8f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(74.5f, 14f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(78.5f, 10f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(82.5f, 5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(83.5f, 10f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(87.5f, 14f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(122.5f, 10f));

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Coin and Key UI
            GuiFactory.BuildCoinHUD(scene, canvas, new Vector2(0.85f, 0.9f));
            GuiFactory.BuildKeyUI(scene, canvas, new Vector2(0.7f, 0.9f));

            // Shopscreen
            GameObject shopScreen = GuiFactory.BuildShopScreen(scene, canvas, Vector2.Zero);
            levelEnd.GetComponent<CDoor>().ShopScreen = shopScreen.GetComponent<CShopScreen>();
            shopScreen.GetComponent<CShopScreen>().Player = player;
            shopScreen.Active = false;

            // Collectables
            ObjectFactory.BuildCoin(scene, new Vector2(10.5f, 3.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(22.5f, 9.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(25.5f, 2.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(33.5f, 4.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(46.5f, 9.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(48.5f, 2.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(51.5f, 4.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(54.5f, 5.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(56.5f, 13.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(59.5f, 3.5f));
            ObjectFactory.BuildKey(scene, new Vector2(63.5f, 7.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(64.5f, 10.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(74.5f, 7.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(75.5f, 13.5f));
            ObjectFactory.BuildKey(scene, new Vector2(75.5f, 4.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(84.5f, 13.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(89.5f, 4.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(100.5f, 4.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(112.5f, 3.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(112.5f, 2.5f));
            ObjectFactory.BuildKey(scene, new Vector2(125.5f, 6.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(126.5f, 6.5f));

            return scene;
        }

        private Scene BuildGameOverScene()
        {
            Scene scene = new Scene(GameManager);

            // Camera
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.GetComponent<CCamera>().Scale = 6f;

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Text
            GameObject textField = GuiFactory.BuildTextField(scene, canvas, new Vector2(0f, 0.5f), "YOU DIED!");
            textField.GetComponent<CGuiTextRender>().Centered = true;
            textField.GetComponent<CGuiTextRender>().SetSize(0.3f);

            GameObject coinUI = GuiFactory.BuildGameOverCoinUI(scene, canvas, Vector2.Zero);

            GameObject exitButton = GuiFactory.BuildButton(scene, canvas, new Vector2(0f, -0.3f), new Vector2(0.6f, 0.2f), "Exit");
            exitButton.GetComponent<CButton>().ButtonClicked += coinUI.GetComponent<CGameOverUI>().OnBtnExit;

            return scene;
        }
    }
}