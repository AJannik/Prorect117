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
            /*
            void Draw()
            {
                // clear screen - what happens without?
                GL.Clear(ClearBufferMask.ColorBufferBit);

                // draw a quad
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(0.0f, 0.0f); // draw first quad corner
                GL.Vertex2(0.5f, 0.0f);
                GL.Vertex2(0.5f, 0.5f);
                GL.Vertex2(0.0f, 0.5f);
                GL.End();
                window.SwapBuffers(); // buffer swap needed for double buffering
            }
            */
            void Update(float deltaTime)
            {
                sceneManager.Update(deltaTime);
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
