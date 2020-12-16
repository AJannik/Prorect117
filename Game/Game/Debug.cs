using System.Collections.Generic;
using Game.RaycastSystem;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    public class Debug
    {
        private List<Ray> rays = new List<Ray>();

        public void DebugDraw()
        {
            foreach (Ray ray in rays)
            {
                GL.Color3(ray.Color);

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(ray.StartPos);
                GL.Vertex2(ray.EndPos);
                GL.End();

                GL.Color3(Color.White);
            }

            rays.Clear();
        }

        public void DrawRay(Ray ray)
        {
            rays.Add(ray);
        }
    }
}