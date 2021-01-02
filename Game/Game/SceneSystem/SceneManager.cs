using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game.SceneSystem
{
    public class SceneManager
    {
        private Scene[] scenes;
        private SceneFactory sceneFactory;
        private int screenWidth = 1366;
        private int screenHeight = 768;

        public SceneManager(int width, int height)
        {
            screenWidth = width;
            screenHeight = height;

            // Building all scenes
            sceneFactory = new SceneFactory(GameManager);
            scenes = new Scene[sceneFactory.NumScenes];
            BuildScenes();
            RegisterSceneEventListeners();

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

        public void Draw(float alpha)
        {
            scenes[CurrentScene].Draw(alpha, DebugMode);
        }

        public void Resize(int width, int height)
        {
            screenWidth = width;
            screenHeight = height;
            scenes[CurrentScene].Resize(screenWidth, screenHeight);
        }

        public void MouseEvent(MouseButtonEventArgs args)
        {
            scenes[CurrentScene].MouseEvent(args);
        }

        public void UnloadCurrentScene()
        {
            // TODO: Implement UnloadCurrentScene()
        }

        public Scene GetScene(int index)
        {
            return scenes[index];
        }

        private void RegisterSceneEventListeners()
        {
            // Load new Level EventListner
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i].LoadLevelNumber += LoadScene;
            }
        }

        private void LoadScene(object sender, int index)
        {
            // inactive Scene has invoked the event
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