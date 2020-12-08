﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    public class SceneManager
    {
        private Scene[] scenes;

        public SceneManager(int numScenes)
        {
            scenes = new Scene[numScenes];

            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
        }

        public int CurrentScene { get; private set; } = 0;

        public void Update(float deltaTime)
        {
            scenes[CurrentScene].Update(deltaTime);
        }

        public void Draw()
        {
            scenes[CurrentScene].Draw();
        }

        public void LoadNextScene()
        {
            if (CurrentScene < scenes.Length - 1)
            {
                UnloadCurrentScene();
                CurrentScene++;

                // TODO: Implement LoadNextScene()
            }
        }

        public void LoadLastScene()
        {
            if (CurrentScene >= 1)
            {
                UnloadCurrentScene();
                CurrentScene--;

                // TODO: Implement LoadLastScene()
            }
        }

        public void UnloadCurrentScene()
        {
            // TODO: Implement UnloadCurrentScene()
        }

        public void SetScene(int index, Scene scene)
        {
            scenes[index] = scene;
        }

        public Scene GetScene(int index)
        {
            return scenes[index];
        }
    }
}