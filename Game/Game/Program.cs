using System;
using Game.Components;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            var window = new GameWindow(512, 512);
            SceneManager sceneManager = new SceneManager(1);

            sceneManager.SetScene(0, BuildScene());
            GameObject quad = sceneManager.GetScene(0).GetGameObjects()[0];

            void Update(float deltaTime)
            {
                sceneManager.Update(deltaTime);

                // TODO: Remove this, only for testing
                quad.Transform.Position += new Vector2(0.001f, 0f);
                if (quad.Transform.Position.X >= 1.2f)
                {
                    quad.Transform.Position = new Vector2(-1f, 0.2f);
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

        private static Scene BuildScene()
        {
            Scene scene = new Scene();
            GameObject quad = new GameObject(scene);
            GameObject camera = new GameObject(scene);
            quad.AddComponent<CRender>();
            quad.Transform.Position = new Vector2(0.3f, 0.5f);

            camera.AddComponent<CCamera>();

            return scene;
        }
    }
}
