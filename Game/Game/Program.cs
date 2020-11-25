using System;
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
            
            CRender cRender = new CRender();
            window.RenderFrame += (s, a) => Draw();
            window.Run();
        }
    }
}
