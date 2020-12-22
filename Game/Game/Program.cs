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
            window.TargetUpdateFrequency = 300;
            window.TargetRenderFrequency = 200;
            window.VSync = VSyncMode.On;
            SceneManager sceneManager = new SceneManager();
            float counter = 7f;
            int skipedFrames = 0;

            GameObject quad = sceneManager.GetScene(0).GetGameObjects()[0];
            GameObject parentQuad = sceneManager.GetScene(1).GetGameObjects()[1];
            GameObject childQuad = sceneManager.GetScene(1).GetGameObjects()[2];

            void Update(float deltaTime)
            {
                // Skip frame if deltaTime is to big because an untypically big deltaTime can cause physic bugs
                if (deltaTime > 0.05f)
                {
                    skipedFrames++;
                    return;
                }

                sceneManager.Update(deltaTime);

                // TODO: Remove this, it's only for testing
                if (sceneManager.CurrentScene < sceneManager.scenes.Length - 1)
                {
                    counter -= deltaTime;
                    if (counter <= 0f)
                    {
                        sceneManager.LoadNextScene();
                        counter = 5f;
                    }
                }
                else
                {
                    // counter -= deltaTime;
                    if (counter <= 0f)
                    {
                        Console.WriteLine($"A total of {skipedFrames} Update-Frames were skiped.");
                        window.Exit();
                        return;
                    }
                }

                // TODO: Remove this, it's only for testing
                if (sceneManager.CurrentScene == 0)
                {
                }
                else if (sceneManager.CurrentScene == 1)
                {
                    parentQuad.Transform.Rotation += deltaTime * 0.5f;
                    childQuad.Transform.Rotation += deltaTime * 1f;
                }
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