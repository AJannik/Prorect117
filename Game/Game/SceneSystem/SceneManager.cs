﻿using System;
using OpenTK.Graphics.OpenGL;

namespace Game.SceneSystem
{
    public class SceneManager
    {
        public Scene[] scenes;
        private SceneFactory sceneFactory;
        private int screenWidth = 1366;
        private int screenHeight = 768;

        public SceneManager()
        {
            // Building all scenes
            sceneFactory = new SceneFactory();
            scenes = new Scene[sceneFactory.NumScenes];
            BuildScenes();

            // Load new Level EventListner
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i].LoadLevelNumber += LoadScene;
            }

            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
        }

        public int CurrentScene { get; private set; } = 0;

        public bool DebugMode { get; set; } = true;

        public GameManager GameManager { get; } = new GameManager();

        public void Update(float deltaTime)
        {
            GameManager.Update(deltaTime);
            scenes[CurrentScene].Update(deltaTime);
        }

        public void FixedUpdate(float deltaTime)
        {
            scenes[CurrentScene].FixedUpdate(deltaTime);
        }

        public void Draw()
        {
            scenes[CurrentScene].Draw(DebugMode);
        }

        public void Resize(int width, int height)
        {
            screenWidth = width;
            screenHeight = height;
            scenes[CurrentScene].Resize(screenWidth, screenHeight);
        }

        public void UnloadCurrentScene()
        {
            // TODO: Implement UnloadCurrentScene()
        }

        public Scene GetScene(int index)
        {
            return scenes[index];
        }

        private void LoadScene(object sender, int index)
        {
            if (sender != scenes[CurrentScene])
            {
                return;
            }

            if (index >= 0 && index < sceneFactory.NumScenes && index != CurrentScene)
            {
                UnloadCurrentScene();
                CurrentScene = index;

                Resize(screenWidth, screenHeight);
            }
        }

        private void BuildScenes()
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = sceneFactory.BuildScene(i);
            }
        }
    }
}