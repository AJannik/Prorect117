using OpenTK.Graphics.OpenGL;

namespace Game.SceneSystem
{
    public class SceneManager
    {
        private Scene[] scenes;
        private SceneFactory sceneFactory;
        private int screenWidth = 0;
        private int screenHeight = 0;

        public SceneManager()
        {
            // Building all scenes
            sceneFactory = new SceneFactory();
            scenes = new Scene[sceneFactory.NumScenes];
            BuildScenes();

            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
        }

        public int CurrentScene { get; private set; } = 0;

        public bool DebugMode { get; set; } = true;

        public void Update(float deltaTime)
        {
            scenes[CurrentScene].Update(deltaTime);
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

        public void LoadNextScene()
        {
            if (CurrentScene < scenes.Length - 1)
            {
                UnloadCurrentScene();
                CurrentScene++;

                Resize(screenWidth, screenHeight);
            }
        }

        public void LoadLastScene()
        {
            if (CurrentScene >= 1)
            {
                UnloadCurrentScene();
                CurrentScene--;

                Resize(screenWidth, screenHeight);
            }
        }

        public void UnloadCurrentScene()
        {
            // TODO: Implement UnloadCurrentScene()
        }

        public Scene GetScene(int index)
        {
            return scenes[index];
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