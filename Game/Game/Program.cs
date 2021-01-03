using System;
using Game.SceneSystem;
using OpenTK;
using OpenTK.Input;

namespace Game
{
    internal class Program
    {
        private int width;
        private int height;
        private GameWindow window;
        private SceneManager sceneManager;
        private double accumulator;
        private double alpha;

        public Program()
        {
            width = 1366;
            height = 768;
            window = new GameWindow(width, height);
            window.VSync = VSyncMode.On;
            sceneManager = new SceneManager(width, height, this);
            accumulator = 0f;
            alpha = 1f;
            SetupEventListeners();
        }

        public void Exit()
        {
            window.Exit();
        }

        [STAThread]
        private static void Main()
        {
            Program program = new Program();
            program.Start();
        }

        private void SetupEventListeners()
        {
            window.UpdateFrame += (_, args) => Update(args.Time);
            window.RenderFrame += (s, a) => Draw(a.Time);
            window.Resize += (_, args) => sceneManager.Resize(window.Width, window.Height);
            window.MouseDown += (_, args) => MouseEvent(args);
        }

        private void Start()
        {
            window.Run();
        }

        private void Update(double frameTime)
        {
            if (frameTime > 0.1f)
            {
                frameTime = 0.1f;
            }

            // Normal Update
            sceneManager.Update((float)frameTime);

            // FixedUpdate for Physics
            accumulator += frameTime;
            while (accumulator >= Physics.PhysicConstants.FixedDeltaTime)
            {
                sceneManager.FixedUpdate(Physics.PhysicConstants.FixedDeltaTime);
                accumulator -= Physics.PhysicConstants.FixedDeltaTime;
            }

            // RenderBlending so we can use unlocked RenderFrequency
            alpha = accumulator / Physics.PhysicConstants.FixedDeltaTime;
        }

        private void Draw(double frameTime)
        {
            sceneManager.Draw((float)alpha * (float)frameTime);
            window.SwapBuffers(); // buffer swap needed for double buffering
        }

        private void MouseEvent(MouseButtonEventArgs args)
        {
            sceneManager.MouseEvent(args);
        }
    }
}