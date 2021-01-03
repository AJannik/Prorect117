﻿using System;
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

        internal int NumScenes { get; } = 2;

        private GameManager GameManager { get; }

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
            ObjectFactory.BuildLevelEnd(scene, new Vector2(15.5f, 5.0f), new Vector2(1, 2));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 6f;

            // Canvas
            GameObject canvas = GuiFactory.BuildCanvas(scene);
            canvas.GetComponent<CCanvas>().Camera = camera.GetComponent<CCamera>();

            // Test Button
            GameObject button = GuiFactory.BuildButton(scene, canvas, new Vector2(-0.7f, 0.5f), "Click Button");
            GameObject mainMenuManager = GuiFactory.BuildMainMenuManager(scene);
            button.GetComponent<CButton>().ButtonClicked += mainMenuManager.GetComponent<CMainMenuManager>().OnButtonClick;

            // Coin UI
            GameObject coinHUD = GuiFactory.BuildCoinHUD(scene, canvas, new Vector2(0.9f, 0.9f));

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

            // Level walls
            StaticRigidbodyFactory.BuildWall(scene, new Vector2(6.5f, 5f), 2);

            // Player, exit and camera
            GameObject player = ObjectFactory.BuildPlayer(scene, new Vector2(4.5f, 5f));
            ObjectFactory.BuildLevelEnd(scene, new Vector2(125.5f, 13.5f), new Vector2(3, 3));
            GameObject camera = ObjectFactory.BuildCamera(scene, Vector2.Zero);
            camera.SetParent(player);
            camera.GetComponent<CCamera>().Scale = 12f;

            // Enemies

            // Collectables

            return scene;
        }
    }
}