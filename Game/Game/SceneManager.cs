﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class SceneManager
    {
        private Scene[] scenes;
        private int currentScene = 0;

        public SceneManager(int numScenes)
        {
            scenes = new Scene[numScenes];
        }

        public void Update(float deltaTime)
        {
            scenes[currentScene].Update(deltaTime);
        }

        public void LoadNextScene()
        {
            // TODO: Implement LoadNextScene()
        }

        public void LoadLastScene()
        {
            // TODO: Implement LoadLastScene()
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