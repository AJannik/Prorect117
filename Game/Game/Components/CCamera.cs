using System;
using Game.Interfaces;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CCamera : IComponent, IDrawable, IUpdateable, IResizeable
    {
        private float scale = 1f;
        private Matrix4 cameraMatrix = Matrix4.Identity;

        public GameObject MyGameObject { get; set; }

        public float Scale
        {
            get => scale;
            set
            {
                scale = MathF.Max(0.001f, value); // avoid division by 0 and negative
                UpdateMatrix();
            }
        }

        public Matrix4 CameraMatrix => cameraMatrix;

        public int XAspect { get; set; } = 16;

        public int YAspect { get; set; } = 9;

        public int Layer { get; set; } = 0;

        public int ViewportWidth { get; private set; }

        public int ViewportHeight { get; private set; }

        public int ViewportX { get; private set; }

        public int ViewportY { get; private set; }

        public void Update(float deltaTime)
        {
            UpdateMatrix();
        }

        public void Draw(float alpha)
        {
            GL.LoadMatrix(ref cameraMatrix);
        }

        public void Resize(int width, int height)
        {
            // Fixed aspect ratio of 16:9
            ViewportX = 0;
            ViewportY = 0;
            ViewportWidth = width;
            ViewportHeight = height;
            if (width * YAspect > height * XAspect)
            {
                ViewportWidth = height * XAspect / YAspect;
                ViewportX = (width - ViewportWidth) / 2;
            }
            else if (width * YAspect < height * XAspect)
            {
                ViewportHeight = width * YAspect / XAspect;
                ViewportY = (height - ViewportHeight) / 2;
            }

            GL.Viewport(ViewportX, ViewportY, ViewportWidth, ViewportHeight);
            UpdateMatrix();
        }

        public void UpdateMatrix()
        {
            Matrix4 aspect = Transformation.Scale(YAspect / (float)XAspect, 1f);

            // Implement camera scaling
            Matrix4 cameraScale = Transformation.Scale(1f / Scale);

            // Implement panning
            Matrix4 translate = Transformation.Translate(-MyGameObject.Transform.WorldPosition);

            // Calculate the resulting camera matrix
            cameraMatrix = Transformation.Combine(translate, cameraScale, aspect);
        }
    }
}