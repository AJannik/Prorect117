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
            int viewportX = 0;
            int viewportY = 0;
            int viewportWidth = width;
            int viewportHeight = height;
            if (width * YAspect > height * XAspect)
            {
                viewportWidth = height * XAspect / YAspect;
                viewportX = (width - viewportWidth) / 2;
            }
            else if (width * YAspect < height * XAspect)
            {
                viewportHeight = width * YAspect / XAspect;
                viewportY = (height - viewportHeight) / 2;
            }

            GL.Viewport(viewportX, viewportY, viewportWidth, viewportHeight);

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