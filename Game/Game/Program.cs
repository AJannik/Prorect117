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
            var window = new GameWindow(1366, 768);
            window.VSync = VSyncMode.On;
            SceneManager sceneManager = new SceneManager();
            window.TargetRenderFrequency = 60;
            float accumulator = 0f;

            void Update(float frameTime)
            {
                // Normal Update
                sceneManager.Update(frameTime);

                // FixedUpdate for Physics
                accumulator += frameTime;
                while (accumulator >= Physics.PhysicConstants.FixedDeltaTime)
                {
                    sceneManager.FixedUpdate(Physics.PhysicConstants.FixedDeltaTime);
                    accumulator -= Physics.PhysicConstants.FixedDeltaTime;
                }

                // TODO: Add RenderBlending and unlock Render-Framerate
                // alpha = accumulator / Physics.PhysicConstants.FixedDeltaTime;

                /*
                // Semi-FixedUpdate for Physics
                int maxSteps = 10;
                while (frameTime > 0f && maxSteps > 0)
                {
                    float deltaTime = MathF.Min(frameTime, Physics.PhysicConstants.FixedDeltaTime);
                    sceneManager.FixedUpdate(deltaTime);
                    frameTime -= deltaTime;
                    maxSteps--;
                }
                */
            }

            void Draw()
            {
                sceneManager.Draw();
                window.SwapBuffers(); // buffer swap needed for double buffering
            }

            window.UpdateFrame += (_, args) => Update((float)args.Time);
            window.RenderFrame += (s, a) => Draw();
            window.Resize += (_, args) => sceneManager.Resize(window.Width, window.Height);
            window.Run();
        }
    }
}