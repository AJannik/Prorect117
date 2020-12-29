using System;
using Game.SceneSystem;
using OpenTK;

namespace Game
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            GameWindow window = new GameWindow(1366, 768);
            window.VSync = VSyncMode.On;
            SceneManager sceneManager = new SceneManager();
            double accumulator = 0f;
            double alpha = 1f;

            void Update(double frameTime)
            {
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

            void Draw(double frameTime)
            {
                sceneManager.Draw((float)alpha * (float)frameTime);
                window.SwapBuffers(); // buffer swap needed for double buffering
            }

            window.UpdateFrame += (_, args) => Update(args.Time);
            window.RenderFrame += (s, a) => Draw(a.Time);
            window.Resize += (_, args) => sceneManager.Resize(window.Width, window.Height);
            window.Run();
        }
    }
}