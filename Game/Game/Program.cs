﻿using System;
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
            window.TargetUpdateFrequency = 150;
            float counter = 7f;
            int skipedFrames = 0;

            GameObject quad = sceneManager.GetScene(0).GetGameObjects()[0];
            GameObject parentQuad = sceneManager.GetScene(1).GetGameObjects()[1];
            GameObject childQuad = sceneManager.GetScene(1).GetGameObjects()[2];

            void Update(float frameTime)
            {
                // Normal Update
                sceneManager.Update(frameTime);

                // Semi-FixedUpdate for Physics
                int maxSteps = 15;
                while (frameTime > 0f && maxSteps > 0)
                {
                    float deltaTime = MathF.Min(frameTime, Physics.PhysicConstants.FixedDeltaTime);
                    frameTime -= deltaTime;
                    sceneManager.FixedUpdate();
                    maxSteps--;
                }

                // TODO: Remove this, it's only for testing
                if (sceneManager.CurrentScene < sceneManager.scenes.Length - 1)
                {
                    // counter -= deltaTime;
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
                    parentQuad.Transform.Rotation += frameTime * 0.5f;
                    childQuad.Transform.Rotation += frameTime * 1f;
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