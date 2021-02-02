using System;
using System.Diagnostics.CodeAnalysis;
using Game.Components;
using Game.Components.Collision;
using Game.Components.Player;
using Game.Components.UI;
using Game.Components.UI.BaseComponents;
using Game.Entity;
using Game.GameObjectFactory;
using OpenTK;

namespace Game.SceneSystem
{
    [ExcludeFromCodeCoverage]
    internal class SceneFactory
    {
        public SceneFactory(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        internal int NumScenes { get; } = 7;

        private GameManager GameManager { get; }

        public Scene BuildScene(int num)
        {
            return num switch
            {
                0 => BuildMainMenu(),
                1 => BuildTutorialLevel(),
                2 => BuildLevel1(),
                3 => BuildLevel2(),
                4 => BuildLevel3(),
                5 => BuildLevel4(),
                6 => BuildGameOverScene(), // Last scene
                _ => throw new ArgumentOutOfRangeException($"There is no Scene numbered {num}!"),
            };
        }

        private Scene BuildMainMenu()
        {
            Scene scene = new Scene(GameManager);

            GameObject menuManager = new GameObject(scene, "MenuManager");
            menuManager.AddComponent<CMainMenuManager>();

            // Camera
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.GetComponent<CCamera>().Scale = 6f;

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Buttons
            GameObject skipTutorialButton = GuiFactory.BuildButton(scene, canvas, new Vector2(0f, 0.3f), new Vector2(1f, 0.15f), "START");
            skipTutorialButton.GetComponent<CButton>().ButtonClicked += menuManager.GetComponent<CMainMenuManager>().OnStartButton;

            GameObject tutorialButton = GuiFactory.BuildButton(scene, canvas, new Vector2(0f, 0f), new Vector2(1f, 0.15f), "START TUTORIAL LEVEL");
            tutorialButton.GetComponent<CButton>().ButtonClicked += menuManager.GetComponent<CMainMenuManager>().OnTutorialButton;

            GameObject exitButton = GuiFactory.BuildButton(scene, canvas, new Vector2(0f, -0.3f), new Vector2(1f, 0.15f), "EXIT");
            exitButton.GetComponent<CButton>().ButtonClicked += menuManager.GetComponent<CMainMenuManager>().OnExitButton;

            // Background
            GameObject background = ObjectFactory.BuildBackground(scene, camera.Transform);
            camera.AddComponent<CPeriodicMovement>();
            CPeriodicMovement periodicMovement = camera.GetComponent<CPeriodicMovement>();
            periodicMovement.Start = new Vector2(0, 0);
            periodicMovement.End = new Vector2(100, 0);
            periodicMovement.MoveSpeed = 2f;
            background.SetParent(camera);

            return scene;
        }

        private Scene BuildTutorialLevel()
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

            // Decorations
            ObjectFactory.BuildSprite(scene, new Vector2(3f, 3f), new Vector2(4, 4), "Environment.TreeSmall.png");
            ObjectFactory.BuildSprite(scene, new Vector2(24f, 8f), new Vector2(4, 4), "Environment.TreeSmall.png");
            ObjectFactory.BuildSprite(scene, new Vector2(16, 2), new Vector2(2, 2), "Environment.Rock.png");
            ObjectFactory.BuildSprite(scene, new Vector2(12f, 12f), new Vector2(3, 2), "Environment.Bush.png");

            // Player, exit and camera
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(2.5f, 2.5f));
            GameObject levelEnd = StaticRigidbodyFactory.BuildLevelEnd(scene, new Vector2(15.5f, 5.0f), new Vector2(1, 2));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 6f;

            // Enemies
            EnemyFactory.BuildBanditEnemy(scene, new Vector2(22.5f, 2f));
            EnemyFactory.BuildBanditEnemy(scene, new Vector2(27.5f, 2f));

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Shop
            GameObject shopScreen = GuiFactory.BuildShopScreen(scene, canvas, Vector2.Zero);
            levelEnd.GetComponent<CDoor>().ShopScreen = shopScreen.GetComponent<CShopScreen>();
            shopScreen.GetComponent<CShopScreen>().Player = player;
            shopScreen.Active = false;

            // Coin, PlayerHP and Key HUD
            GuiFactory.BuildHudElements(scene, canvas, player.GetComponent<CPlayerCombatController>());

            // Tutorial Windows
            GuiFactory.BuildControls(scene, canvas);
            GameObject goalWindow = GuiFactory.BuildGoalWindow(scene, canvas);
            GameObject boxTrigger = ObjectFactory.BuildBoxTrigger(scene, new Vector2(10f, 9f));
            boxTrigger.GetComponent<CBoxTrigger>().TriggerEntered += goalWindow.GetComponent<CTutorialWindow>().Open;
            GameObject powerDownWindow = GuiFactory.BuildPowerDownWindow(scene, canvas);

            // Collectables
            ObjectFactory.BuildKey(scene, new Vector2(30.5f, 1.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(3.5f, 13.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(11.5f, 11.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(18.5f, 12.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(30.5f, 13.5f));

            // Power Downs
            GameObject fragile = PowerDownFactory.Fragile(scene, new Vector2(20.5f, 13f));
            fragile.GetComponent<CCircleTrigger>().TriggerEntered += powerDownWindow.GetComponent<CTutorialWindow>().Open;
            PowerDownFactory.Weakness(scene, new Vector2(28.5f, 13.5f));
            PowerDownFactory.Slowness(scene, new Vector2(18.5f, 7.5f));

            // Background
            GameObject background = ObjectFactory.BuildBackground(scene, camera.Transform);
            background.SetParent(camera);

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

            // Decorations
            ObjectFactory.BuildSprite(scene, new Vector2(5f, 9f), new Vector2(4, 4), "Environment.TreeSmall.png");
            ObjectFactory.BuildSprite(scene, new Vector2(23f, 7f), new Vector2(4, 4), "Environment.TreeSmall.png");
            ObjectFactory.BuildSprite(scene, new Vector2(68f, 12f), new Vector2(4, 4), "Environment.TreeSmall.png");
            ObjectFactory.BuildSprite(scene, new Vector2(98f, 13f), new Vector2(4, 4), "Environment.TreeSmall.png");
            
            ObjectFactory.BuildSprite(scene, new Vector2(2, 2), new Vector2(2, 2), "Environment.Rock.png");
            ObjectFactory.BuildSprite(scene, new Vector2(7, 14), new Vector2(2, 2), "Environment.Rock.png");
            ObjectFactory.BuildSprite(scene, new Vector2(20, 2), new Vector2(2, 2), "Environment.Rock.png");
            ObjectFactory.BuildSprite(scene, new Vector2(33, 14), new Vector2(2, 2), "Environment.Rock.png");
            
            ObjectFactory.BuildSprite(scene, new Vector2(80, 2), new Vector2(2, 2), "Environment.Rock.png");
            ObjectFactory.BuildSprite(scene, new Vector2(50, 13), new Vector2(2, 2), "Environment.Rock.png");
            ObjectFactory.BuildSprite(scene, new Vector2(74, 11), new Vector2(2, 2), "Environment.Rock.png");
            ObjectFactory.BuildSprite(scene, new Vector2(122, 10), new Vector2(2, 2), "Environment.Rock.png");

            
            // Player, exit and camera
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(4.5f, 5.1f));
            GameObject levelEnd = StaticRigidbodyFactory.BuildLevelEnd(scene, new Vector2(125.5f, 13.5f), new Vector2(3, 3));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 30f;

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

            // Shop
            GameObject shopScreen = GuiFactory.BuildShopScreen(scene, canvas, Vector2.Zero);
            levelEnd.GetComponent<CDoor>().ShopScreen = shopScreen.GetComponent<CShopScreen>();
            shopScreen.GetComponent<CShopScreen>().Player = player;
            shopScreen.Active = false;

            // Coin, PlayerHP and Key HUD
            GuiFactory.BuildHudElements(scene, canvas, player.GetComponent<CPlayerCombatController>());

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

            // Power Downs
            PowerDownFactory.Silenced(scene, new Vector2(27.5f, 10.5f));
            PowerDownFactory.Fragile(scene, new Vector2(31f, 2.5f));
            PowerDownFactory.Slowness(scene, new Vector2(60f, 14f));
            PowerDownFactory.Weakness(scene, new Vector2(72f, 14f));
            PowerDownFactory.Fragile(scene, new Vector2(112.5f, 4.5f));
            PowerDownFactory.Silenced(scene, new Vector2(120f, 6.5f));

            // Level dead-zones
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(40f, -1f), new Vector2(4f, 1f), new Vector2(37f, 2.5f), 30);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(53.5f, -1f), new Vector2(21f, 1f), new Vector2(43f, 2.5f), 30);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(104f, -1f), new Vector2(22f, 1f), new Vector2(92.5f, 2.5f), 30);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(120.5f, -1f), new Vector2(9f, 1f), new Vector2(116f, 2.5f), 30);

            // Spike sprites
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(40f, -1.8f), 5);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(53.5f, -1.8f), 22);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(104f, -1.8f), 23);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(120.5f, -1.8f), 10);

            // Background
            GameObject background = ObjectFactory.BuildBackground(scene, camera.Transform);
            background.SetParent(camera);

            return scene;
        }

        private Scene BuildLevel2()
        {
            Scene scene = new Scene(GameManager);

            // Level border
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16f, -78.5f), 32, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16f, -127.5f), 32, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(0.5f, -103f), 48, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(31.5f, -103f), 48, "Content.StoneBrickTilemap.png");

            // Level platforms
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(23f, -82.5f), 10, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(8f, -83.5f), 6, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(14.5f, -83.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(29f, -84.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(10.5f, -86.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16.5f, -86.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(3.5f, -87.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(29f, -87.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(18.5f, -89.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(8f, -90.5f), 6, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(26.5f, -90.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(6f, -93.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(15.5f, -93.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(24f, -93.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(20.5f, -94.5f), 1, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(3f, -95.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(11f, -96.5f), 8, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(20.5f, -98.5f), 11, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(4.5f, -99.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(9f, -100.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(13.5f, -101.5f), 1, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(27.5f, -101.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(2f, -103.5f), 2, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(15.5f, -103.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(8.5f, -104.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(22f, -104.5f), 6, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(5.5f, -107.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(18f, -107.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(10.5f, -108.5f), 7, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(26.5f, -108.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(3f, -111.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(20f, -111.5f), 2, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(15f, -112.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(25.5f, -112.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(30.5f, -112.5f), 1, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(9f, -113.5f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(20.5f, -115.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(5.5f, -116.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(11.5f, -116.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(28.5f, -116.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(18f, -118.5f), 6, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(7.5f, -119.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(30.5f, -120.5f), 1, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(15.5f, -121.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(24.5f, -121.5f), 7, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(7.5f, -123.5f), 9, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16.5f, -124.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(22.5f, -124.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(28f, -124.5f), 2, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(4.5f, -126.5f), 7, "Content.StoneBrickTilemap.png");

            // Moving platforms
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(1.5f, -82.5f), new Vector2(1.5f, -93.5f), 1, "Content.StoneBrickTilemap.png");
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(30f, -93.5f), new Vector2(30f, -105.5f), 2, "Content.StoneBrickTilemap.png");
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(2f, -116.5f), new Vector2(2f, -123.5f), 2, "Content.StoneBrickTilemap.png");

            // Level walls
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(15.5f, -82f), 2, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(2.5f, -84.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(18.5f, -84.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(27.5f, -83.5f), 1, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(5.5f, -87f), 6, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(13.5f, -88.5f), 9, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(21.5f, -90.5f), 11, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(16.5f, -88f), 2, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(24.5f, -88.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(10.5f, -93.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(28.5f, -96f), 10, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(4.5f, -94.5f), 1, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(17.5f, -95f), 2, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(7.5f, -98.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(14.5f, -100f), 6, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(22.5f, -101.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(5.5f, -103.5f), 7, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(19.5f, -102.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(27.5f, -105f), 6, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(16.5f, -105.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(10.5f, -105.5f), 1, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(19.5f, -109.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(10.5f, -111f), 4, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(24.5f, -110.5f), 3, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(15.5f, -115.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(23.5f, -115.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(7.5f, -115f), 2, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(5.5f, -118f), 2, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(12.5f, -122f), 10, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(20.5f, -121.5f), 5, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(17.5f, -123f), 2, "Content.StoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(27.5f, -123f), 2, "Content.StoneBrickTilemap.png");

            // Player, exit and camera
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(10.5f, -125.9f));
            GameObject levelEnd = StaticRigidbodyFactory.BuildLevelEnd(scene, new Vector2(29.5f, -82.5f), new Vector2(3, 3));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 6f;

            // Enemies
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(22.5f, -81f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(8.5f, -82f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(19.5f, -97f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(2.5f, -110f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(15.5f, -111f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(7.5f, -122f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(16.5f, -126f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(24.5f, -126f));

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Shop
            GameObject shopScreen = GuiFactory.BuildShopScreen(scene, canvas, Vector2.Zero);
            levelEnd.GetComponent<CDoor>().ShopScreen = shopScreen.GetComponent<CShopScreen>();
            shopScreen.GetComponent<CShopScreen>().Player = player;
            shopScreen.Active = false;

            // Coin, PlayerHP and Key HUD
            GuiFactory.BuildHudElements(scene, canvas, player.GetComponent<CPlayerCombatController>());

            // Collectables
            ObjectFactory.BuildCoin(scene, new Vector2(14.5f, -85.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(30.5f, -86.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(17.5f, -87.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(4.5f, -88.5f));
            ObjectFactory.BuildKey(scene, new Vector2(17.5f, -88.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(20.5f, -90.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(22.5f, -94.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(1.5f, -96.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(8.5f, -98.5f));
            ObjectFactory.BuildKey(scene, new Vector2(8.5f, -99.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(15.5f, -99.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(4.5f, -100.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(20.5f, -103.5f));
            ObjectFactory.BuildKey(scene, new Vector2(21.5f, -103.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(1.5f, -104.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(6.5f, -106.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(18.5f, -108.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(9.5f, -109.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(11.5f, -109.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(25.5f, -110.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(6.5f, -118.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(16.5f, -122.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(16.5f, -123.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(21.5f, -123.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(23.5f, -123.5f));

            // Power Downs
            PowerDownFactory.Weakness(scene, new Vector2(28f, -86f));
            PowerDownFactory.Silenced(scene, new Vector2(10.5f, -98.5f));
            PowerDownFactory.Fragile(scene, new Vector2(18f, -103.5f));
            PowerDownFactory.Silenced(scene, new Vector2(9f, -106.5f));
            PowerDownFactory.Weakness(scene, new Vector2(14f, -124.5f));
            PowerDownFactory.Slowness(scene, new Vector2(26f, -123f));

            // Level dead-zones
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(4f, -86.5f), new Vector2(2f, 1f), new Vector2(5.5f, -81.5f), 25);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(2.5f, -94.5f), new Vector2(3f, 1f), new Vector2(4.5f, -91.5f), 25);

            // Spike sprites
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(4.5f, -86.5f), 2);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(3f, -94.5f), 3);

            // Background
            GameObject background = ObjectFactory.BuildBackground(scene, camera.Transform);
            background.SetParent(camera);

            return scene;
        }

        private Scene BuildLevel3()
        {
            Scene scene = new Scene(GameManager);

            // Level border
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16f, -0.5f), 32, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16f, -78.5f), 32, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(0.5f, -39.5f), 77, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(31.5f, -39.5f), 77, "Content.DarkStoneBrickTilemap.png");

            // Level platforms
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(4.5f, -3.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(21f, -3.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(27f, -3.5f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(8f, -4.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(15.5f, -4.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(20f, -6.5f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(25.5f, -6.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(30.5f, -6.5f), 1, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(6.5f, -7.5f), 11, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(18.5f, -9.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(3.5f, -10.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(11f, -10.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(19.5f, -12.5f), 9, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(27f, -12.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(8.5f, -13.5f), 11, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(29.5f, -14.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(15.5f, -15.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(5f, -16.5f), 8, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(23f, -16.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(28f, -17.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(15f, -18.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(7.5f, -20.5f), 9, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(27f, -20.5f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(18f, -21.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(23.5f, -21.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(10f, -23.5f), 8, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(30f, -23.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16.5f, -24.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(25.5f, -24.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(7.5f, -26.5f), 7, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(23f, -26.5f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(14.5f, -27.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(28f, -27.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(24.5f, -29.5f), 7, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(12.5f, -30.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16f, -31.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(20.5f, -31.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(4f, -32.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(26.5f, -32.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(10.5f, -33.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(16.5f, -34.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(23f, -35.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(29f, -36.5f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(7.5f, -37.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(14.5f, -37.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(7.5f, -40.5f), 9, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(21.5f, -40.5f), 15, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(11.5f, -43.5f), 7, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(19.5f, -43.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(27.5f, -43.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(24f, -46.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(3f, -47.5f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(10f, -47.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(20.5f, -49.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(9f, -50.5f), 12, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(26.5f, -50.5f), 9, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(17.5f, -52.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(7.5f, -53.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(24f, -53.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(28f, -55.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(20.5f, -56.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(13f, -57.5f), 8, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(4.5f, -60.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(8f, -60.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(14.5f, -60.5f), 7, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(24.5f, -60.5f), 9, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(4f, -63.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(18f, -63.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(27.5f, -63.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(24.5f, -66.5f), 9, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(14f, -67.5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(7.5f, -69.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(19f, -71.5f), 8, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(28f, -72.5f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(6.5f, -75.5f), 7, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(14f, -75.5f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(25f, -75.5f), 8, "Content.DarkStoneBrickTilemap.png");

            // Moving platforms
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(2f, -22.5f), new Vector2(2f, -29.5f), 2, "Content.DarkStoneBrickTilemap.png");
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(2f, -38.5f), new Vector2(2f, -45.5f), 2, "Content.DarkStoneBrickTilemap.png");
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(30f, -42.5f), new Vector2(30f, -47.5f), 2, "Content.DarkStoneBrickTilemap.png");
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(2f, -53.5f), new Vector2(2f, -60.5f), 2, "Content.DarkStoneBrickTilemap.png");
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(30f, -57.5f), new Vector2(30f, -69.5f), 2, "Content.DarkStoneBrickTilemap.png");
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(1.5f, -68.5f), new Vector2(1.5f, -75.5f), 1, "Content.DarkStoneBrickTilemap.png");

            // Level walls
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(9.5f, -2.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(22.5f, -5.5f), 9, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(27.5f, -5f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(17.5f, -7f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(14.5f, -11.5f), 7, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(28.5f, -12f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(11.5f, -17f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(17.5f, -20f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(3.5f, -22f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(14.5f, -22.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(21.5f, -24f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(18.5f, -26f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(24.5f, -25.5f), 1, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(5.5f, -28.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(10.5f, -28.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(15.5f, -29.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(27.5f, -28.5f), 1, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(21.5f, -30.5f), 1, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(19.5f, -35f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(6.5f, -35f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(24.5f, -36.5f), 7, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(11.5f, -37f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(3.5f, -37.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(8.5f, -42f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(14.5f, -42f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(23.5f, -42.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(28.5f, -42f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(18.5f, -46.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(26.5f, -45f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(4.5f, -46f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(15.5f, -49f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(7.5f, -49f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(3.5f, -54.5f), 7, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(22.5f, -54.5f), 7, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(16.5f, -55f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(6.5f, -58.5f), 9, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(28.5f, -58f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(12.5f, -66f), 10, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(23.5f, -63.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(5.5f, -65f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(9.5f, -66f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(17.5f, -66.5f), 5, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(28.5f, -65f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(2.5f, -69f), 6, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(8.5f, -68f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(20.5f, -68f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(26.5f, -68.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(6.5f, -71.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(3.5f, -73f), 4, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(15.5f, -73.5f), 3, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(9.5f, -74f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(12.5f, -74f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(25.5f, -74f), 2, "Content.DarkStoneBrickTilemap.png");
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(18.5f, -76f), 4, "Content.DarkStoneBrickTilemap.png");

            // Player, exit and camera
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(27.5f, -76.9f));
            GameObject levelEnd = StaticRigidbodyFactory.BuildLevelEnd(scene, new Vector2(7.5f, -2.5f), new Vector2(3, 3));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 6f;

            // Enemies
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(5.5f, -6f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(18.5f, -11f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(10.5f, -12f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(4.5f, -15f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(15.5f, -17f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(6.5f, -19f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(10.5f, -22f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(6.5f, -25f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(22.5f, -28f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(18.5f, -39f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(11.5f, -42f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(10.5f, -49f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(25.5f, -49f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(11.5f, -56f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(13.5f, -59f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(24.5f, -59f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(17.5f, -70f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(26.5f, -71f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(4.5f, -77f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(12.5f, -77f));

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Shop
            GameObject shopScreen = GuiFactory.BuildShopScreen(scene, canvas, Vector2.Zero);
            levelEnd.GetComponent<CDoor>().ShopScreen = shopScreen.GetComponent<CShopScreen>();
            shopScreen.GetComponent<CShopScreen>().Player = player;
            shopScreen.Active = false;

            // Coin, PlayerHP and Key HUD
            GuiFactory.BuildHudElements(scene, canvas, player.GetComponent<CPlayerCombatController>());

            // Collectables
            ObjectFactory.BuildCoin(scene, new Vector2(21.5f, -2.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(26.5f, -4.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(21.5f, -5.5f));
            ObjectFactory.BuildKey(scene, new Vector2(26.5f, -5.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(18.5f, -8.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(1.5f, -9.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(13.5f, -12.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(12.5f, -14.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(15.5f, -14.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(30.5f, -16.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(22.5f, -25.5f));
            ObjectFactory.BuildKey(scene, new Vector2(23.5f, -25.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(14.5f, -29.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(20.5f, -32.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(10.5f, -34.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(7.5f, -41.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(9.5f, -41.5f));
            ObjectFactory.BuildKey(scene, new Vector2(9.5f, -42.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(27.5f, -42.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(8.5f, -49.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(14.5f, -51.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(23.5f, -52.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(7.5f, -61.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(11.5f, -61.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(5.5f, -62.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(27.5f, -64.5f));
            ObjectFactory.BuildKey(scene, new Vector2(27.5f, -65.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(26.5f, -73.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(14.5f, -74.5f));
            ObjectFactory.BuildKey(scene, new Vector2(26.5f, -74.5f));

            // Power Downs
            PowerDownFactory.Silenced(scene, new Vector2(27f, -2f));
            PowerDownFactory.Slowness(scene, new Vector2(15f, -2.5f));
            PowerDownFactory.Fragile(scene, new Vector2(13f, -40f));
            PowerDownFactory.Weakness(scene, new Vector2(14f, -49f));
            PowerDownFactory.Fragile(scene, new Vector2(25f, -65f));
            PowerDownFactory.Slowness(scene, new Vector2(30f, -74f));

            // Level dead-zones
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(30f, -13.5f), new Vector2(2f, 1f), new Vector2(28.5f, -8.5f), 25);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(2.5f, -46.5f), new Vector2(3f, 1f), new Vector2(4.5f, -43.5f), 25);

            // Spike sprites
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(30.5f, -13.5f), 2);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(3f, -46.5f), 3);

            // Background
            GameObject background = ObjectFactory.BuildBackground(scene, camera.Transform);
            background.SetParent(camera);

            return scene;
        }

        private Scene BuildLevel4()
        {
            Scene scene = new Scene(GameManager);

            // Level border
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(64f, 0.5f), 128);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(64f, 31.5f), 128);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(0.5f, 16f), 30);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(127.5f, 16f), 30);

            // Level  walls
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(12.5f, 10f), 18); 
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(32.5f, 23f), 16);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(43.5f, 4f), 6);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(47.5f, 4f), 6);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(62f, 2f), 2);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(67.5f, 3f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(50.5f, 15f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(59.5f, 15f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(67.5f, 15f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(43.5f, 16f), 6);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(72.5f, 15f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(80.5f, 14.5f), 3);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(87.5f, 15.5f), 5);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(106.5f, 15f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(110f, 3f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(115f, 3f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(120f, 3f), 4);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(20.5f, 1.5f), 1);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(24.5f, 1.5f), 1);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(78.5f, 1.5f), 1);
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(86.5f, 1.5f), 1);

            // Level Platforms
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(3f, 5.5f), 4);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(18f, 19.5f), 12);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(30.5f, 22.5f), 3);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(75f, 12.5f), 64);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(35.5f, 22.5f), 5);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(83f, 5.5f), 32);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(82.5f, 1.5f), 5);
            StaticRigidbodyFactory.BuildPlatform(scene, new Vector2(10.5f, 17f), 3);

            // Level spikes
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(23f, 1.5f), 3);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(46f, 1.5f), 3);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(70.5f, 13.5f), 4);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(80f, 1.5f), 1);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(46f, 13.5f), 3);
            StaticRigidbodyFactory.BuildSpikes(scene, new Vector2(86f, 1.5f), 1);

            // Level Deadly area
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(45.5f, 1.5f), new Vector2(3f, 1f), new Vector2(43.5f, 7.5f), 30);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(79.5f, 1.5f), new Vector2(1f, 1f), new Vector2(82.5f, 2f), 30);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(85.5f, 1.5f), new Vector2(1f, 1f), new Vector2(93.5f, 1.5f), 30);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(45.5f, 13.5f), new Vector2(3f, 1f), new Vector2(43.5f, 19.5f), 30);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(70f, 13.5f), new Vector2(4f, 1f), new Vector2(67.5f, 18.5f), 30);
            StaticRigidbodyFactory.BuildDeadlyArea(scene, new Vector2(22.5f, 1.5f), new Vector2(3f, 1f), new Vector2(26.5f, 3.5f), 30);

            // Moving platforms
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(20.5f, 15f), new Vector2(30f, 15f), 3);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(110f, 7.5f), new Vector2(110f, 12.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(109.5f, 16.5f), new Vector2(110f, 27.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(115f, 9.5f), new Vector2(115f, 11.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(115f, 15.5f), new Vector2(115f, 25.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(120f, 12.5f), new Vector2(120f, 23.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(125f, 15.5f), new Vector2(125f, 19.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(117.5f, 23.5f), new Vector2(117.5f, 29.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(118f, 14.5f), new Vector2(118f, 20.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(113f, 13.5f), new Vector2(113f, 19.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(112.5f, 23.5f), new Vector2(112.5f, 28.5f), 2);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(10.5f, 3f), new Vector2(10.5f, 12.5f), 3);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(2.5f, 8f), new Vector2(2.5f, 17.5f), 3);
            ObjectFactory.BuildMovingPlatform(scene, new Vector2(40.5f, 3.5f), new Vector2(40.5f, 18.5f), 3);

            // Moving spikes
            ObjectFactory.BuildMovingSpikes(scene, new Vector2(21f, 16f), new Vector2(30.5f, 16f), 3);
            ObjectFactory.BuildMovingSpikes(scene, new Vector2(90f, 13.5f), new Vector2(105f, 13.5f), 3);
            ObjectFactory.BuildMovingSpikes(scene, new Vector2(105f, 13.5f), new Vector2(90f, 13.5f), 3);

            // Moving deadly area
            ObjectFactory.BuildMovingDeadlyArea(scene, new Vector2(20.5f, 16f), new Vector2(30f, 16f), new Vector2(3, 1), new Vector2(23, 23f), 30);
            ObjectFactory.BuildMovingDeadlyArea(scene, new Vector2(89.5f, 13.5f), new Vector2(104.5f, 13.5f), new Vector2(3, 1), new Vector2(87.5f, 19.5f), 30);
            ObjectFactory.BuildMovingDeadlyArea(scene, new Vector2(104.5f, 13.5f), new Vector2(89.5f, 13.5f), new Vector2(3, 1), new Vector2(87.5f, 19.5f), 30);

            // Player, exit and camera
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(2.5f, 2.5f));
            GameObject levelEnd = StaticRigidbodyFactory.BuildLevelEnd(scene, new Vector2(124.5f, 28.5f), new Vector2(1, 2));
            levelEnd.GetComponent<CDoor>().LastLevel = true;
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 6f;

            // Enemies
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(10f, 1.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(3f, 6.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(19.5f, 21f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(21.5f, 21f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(15f, 1.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(26f, 1.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(29f, 1.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(50f, 1.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(55f, 1.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(75f, 1.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(70f, 1.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(82.5f, 2.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(72.5f, 6.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(82.5f, 6.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(90.5f, 6.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(100f, 1.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(118.5f, 6f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(78.5f, 13.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(83.5f, 13.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(60f, 13.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(55f, 13.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(34f, 23.5f));
            EnemyFactory.BuildSkeletonEnemy(scene, new Vector2(124.5f, 27.5f));

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Coin, PlayerHP and Key HUD
            GuiFactory.BuildHudElements(scene, canvas, player.GetComponent<CPlayerCombatController>());

            // Collectables
            // Keys
            ObjectFactory.BuildKey(scene, new Vector2(124f, 24.5f));
            ObjectFactory.BuildKey(scene, new Vector2(97f, 16.5f));
            ObjectFactory.BuildKey(scene, new Vector2(72.5f, 3f));

            // Coins
            ObjectFactory.BuildCoin(scene, new Vector2(3f, 7f));
            ObjectFactory.BuildCoin(scene, new Vector2(15f, 22f));
            ObjectFactory.BuildCoin(scene, new Vector2(22f, 22f));
            ObjectFactory.BuildCoin(scene, new Vector2(31f, 24.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(34.5f, 24.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(50.5f, 22.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(59.5f, 23.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(67.5f, 22.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(55.5f, 14.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(64.5f, 15.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(82.5f, 4.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(95.5f, 3.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(75.5f, 8.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(85.5f, 9.5f));
            ObjectFactory.BuildCoin(scene, new Vector2(100.5f, 8.5f));

            // Power Downs
            PowerDownFactory.Fragile(scene, new Vector2(98.5f, 3f));
            PowerDownFactory.Slowness(scene, new Vector2(16.5f, 3f));
            PowerDownFactory.Weakness(scene, new Vector2(97f, 15f));
            PowerDownFactory.Silenced(scene, new Vector2(124f, 3f));

            // Background
            GameObject background = ObjectFactory.BuildBackground(scene, camera.Transform);
            background.SetParent(camera);

            return scene;
        }

        private Scene BuildGameOverScene()
        {
            Scene scene = new Scene(GameManager);

            GameObject gameOverManager = new GameObject(scene, "GameOverManager");
            gameOverManager.AddComponent<CGameOverUI>();

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
            gameOverManager.GetComponent<CGameOverUI>().Title = textField.GetComponent<CGuiTextRender>();

            GameObject coinUi = GuiFactory.BuildGameOverCoinUi(scene, canvas, Vector2.Zero);
            gameOverManager.GetComponent<CGameOverUI>().CoinsText = coinUi.GetChild(1).GetComponent<CGuiTextRender>();

            // Buttons
            GameObject exitButton = GuiFactory.BuildButton(scene, canvas, new Vector2(0.5f, -0.4f), new Vector2(1.2f, 0.15f), "EXIT");
            exitButton.GetComponent<CButton>().ButtonClicked += gameOverManager.GetComponent<CGameOverUI>().OnBtnExit;
            GameObject restartButton = GuiFactory.BuildButton(scene, canvas, new Vector2(-0.5f, -0.4f), new Vector2(1.2f, 0.15f), "RETURN TO MAIN MENU");
            restartButton.GetComponent<CButton>().ButtonClicked += gameOverManager.GetComponent<CGameOverUI>().OnBtnReturn;

            // Background
            GameObject background = ObjectFactory.BuildBackground(scene, camera.Transform);
            camera.AddComponent<CPeriodicMovement>();
            CPeriodicMovement periodicMovement = camera.GetComponent<CPeriodicMovement>();
            periodicMovement.Start = new Vector2(0, 0);
            periodicMovement.End = new Vector2(100, 0);
            periodicMovement.MoveSpeed = 2f;
            background.SetParent(camera);

            return scene;
        }
    }
}