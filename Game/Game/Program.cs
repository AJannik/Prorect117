﻿using System;
using Game.Components;
using Game.GameObjectFactory;
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
            SceneManager sceneManager = new SceneManager(2);
            float counter = 7f;
            int skipedFrames = 0;

            sceneManager.SetScene(0, BuildScene1());
            sceneManager.SetScene(1, BuildScene2());
            GameObject quad = sceneManager.GetScene(0).GetGameObjects()[0];
            GameObject parentQuad = sceneManager.GetScene(1).GetGameObjects()[0];
            GameObject childQuad = sceneManager.GetScene(1).GetGameObjects()[1];

            void Update(float deltaTime)
            {
                // Skip frame if deltaTime is to big because an untypically big deltaTime can cause physic bugs
                if (deltaTime > 0.03f)
                {
                    skipedFrames++;
                    return;
                }

                sceneManager.Update(deltaTime);

                // TODO: Remove this, it's only for testing
                if (sceneManager.CurrentScene == 0)
                {
                    counter -= deltaTime;
                    if (counter <= 0f)
                    {
                        sceneManager.LoadNextScene();
                        counter = 10f;
                    }
                }
                else
                {
                    counter -= deltaTime;
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

        private static Scene BuildScene1()
        {
            Scene scene = new Scene();
            ObjectFactory.BuildBall(scene);
            ObjectFactory.BuildFloor(scene);

            return scene;
        }

        private static Scene BuildScene2()
        {
            Scene scene = new Scene();
            GameObject parentQuad = new GameObject(scene);
            GameObject childQuad = new GameObject(scene, parentQuad);
            GameObject childChildQuad = new GameObject(scene, childQuad);

            parentQuad.AddComponent<CRender>();
            parentQuad.AddComponent<CCamera>();
            parentQuad.GetComponent<CCamera>().Scale = 5f;
            childQuad.AddComponent<CRender>();
            childChildQuad.AddComponent<CRender>();
            childQuad.Transform.Position = new Vector2(3f, 0f);
            childQuad.Transform.Scale = new Vector2(0.8f, 0.8f);
            childChildQuad.Transform.Scale = new Vector2(0.8f, 0.8f);
            childChildQuad.Transform.Position = new Vector2(1.5f, 0f);

            return scene;
        }
    }
}