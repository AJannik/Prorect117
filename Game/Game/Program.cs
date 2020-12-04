using System;
using Game.Components;
using OpenTK;

namespace Game
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            var window = new GameWindow(512, 512);
            SceneManager sceneManager = new SceneManager(2);
            float counter = 5f;

            sceneManager.SetScene(0, BuildScene1());
            sceneManager.SetScene(1, BuildScene2());
            GameObject quad = sceneManager.GetScene(0).GetGameObjects()[0];
            GameObject parentQuad = sceneManager.GetScene(1).GetGameObjects()[0];

            void Update(float deltaTime)
            {
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
                        window.Exit();
                        return;
                    }
                }

                // TODO: Remove this, it's only for testing
                if (sceneManager.CurrentScene == 0)
                {
                    quad.Transform.Position += new Vector2(0.001f, 0f);
                    if (quad.Transform.Position.X >= 1.2f)
                    {
                        quad.Transform.Position = new Vector2(-1f, 0.2f);
                    }
                }
                else if (sceneManager.CurrentScene == 1)
                {
                    parentQuad.Transform.Rotation += deltaTime * 0.5f;
                }
            }

            void Draw()
            {
                sceneManager.Draw();
                window.SwapBuffers(); // buffer swap needed for double buffering
            }

            window.UpdateFrame += (_, args) => Update((float)args.Time);
            window.RenderFrame += (s, a) => Draw();
            window.Run();
        }

        private static Scene BuildScene1()
        {
            Scene scene = new Scene();
            GameObject quad = new GameObject(scene);
            GameObject camera = new GameObject(scene);
            quad.AddComponent<CRender>();
            quad.Transform.Position = new Vector2(0.3f, 0.5f);

            camera.AddComponent<CCamera>();

            return scene;
        }

        private static Scene BuildScene2()
        {
            Scene scene = new Scene();
            GameObject parentQuad = new GameObject(scene);
            GameObject childQuad = new GameObject(scene, parentQuad);

            parentQuad.AddComponent<CRender>();
            childQuad.AddComponent<CRender>();
            childQuad.Transform.Position = new Vector2(0.5f, 0f);

            return scene;
        }
    }
}