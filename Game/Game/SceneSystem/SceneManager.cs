using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game.SceneSystem
{
    internal class SceneManager
    {
        private Scene[] scenes;
        private SceneFactory sceneFactory;
        private int screenWidth = 1366;
        private int screenHeight = 768;
        private Program program;

        public SceneManager(int width, int height, Program program)
        {
            screenWidth = width;
            screenHeight = height;
            this.program = program;

            // Building all scenes
            sceneFactory = new SceneFactory(GameManager);
            scenes = new Scene[sceneFactory.NumScenes];
            BuildScenes();
            RegisterSceneEventListeners();

            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);

            Start();
            GameManager.EndGameEvent += LoadScene;
            GameManager.RestartGameEvent += Restart;
        }

        public int CurrentScene { get; private set; } = 0;

        public bool DebugMode { get; } = false;

        public GameManager GameManager { get; } = new GameManager();

        public void Start()
        {
            scenes[CurrentScene].Start();
        }

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

        public Scene GetScene(int index)
        {
            return scenes[index];
        }

        public void ExitEvent(object sender, int i)
        {
            program.Exit();
        }

        private void Restart(object sender, int num)
        {
            LoadScene(this, -CurrentScene);
            for (int i = 1; i < sceneFactory.NumScenes; i++)
            {
                scenes[i] = sceneFactory.BuildScene(i);
            }
        }

        private void RegisterSceneEventListeners()
        {
            // Load new Level EventListener
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i].LoadLevelNumber += LoadScene;
                scenes[i].ExitEvent += ExitEvent;
            }
        }

        private void LoadScene(object sender, int index)
        {
            // GameManager detected Game End-condition
            if (sender == GameManager)
            {
                CurrentScene = sceneFactory.NumScenes - 1;

                Resize(screenWidth, screenHeight);
                Start();
            }

            // inactive Scene has invoked the event
            if (sender != scenes[CurrentScene] && sender != this)
            {
                return;
            }

            if (CurrentScene + index >= 0 && CurrentScene + index < sceneFactory.NumScenes)
            {
                CurrentScene += index;

                Resize(screenWidth, screenHeight);
                Start();
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