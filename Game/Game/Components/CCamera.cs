using System;
using Game.Interfaces;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CCamera : IComponent
    {
        private float scale = 1f;
        private float invWindowAspectRatio = 1f;
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

        public Matrix4 InvViewportMatrix { get; private set; }

        public void Update(float deltaTime)
        {
            UpdateMatrix();
        }

        public void Draw()
        {
            GL.LoadMatrix(ref cameraMatrix);
        }

        public void Resize(int width, int height)
        {
            // TODO: Make AspectRatio fixed to 16:9
            GL.Viewport(0, 0, width, height);
            invWindowAspectRatio = height / (float)width;

            InvViewportMatrix = Transformation.Combine(Transformation.Scale(2f / width, 2f / height), Transformation.Translate(-Vector2.One));
            UpdateMatrix();
        }

        public void UpdateMatrix()
        {
            // Implement window aspect ratio scaling
            Matrix4 aspect = Transformation.Scale(invWindowAspectRatio, 1f);

            // Implement camera scaling
            Matrix4 cameraScale = Transformation.Scale(1f / Scale);

            // Implement panning
            Matrix4 translate = Transformation.Translate(-MyGameObject.Transform.WorldPosition);

            // Calculate the resulting camera matrix
            cameraMatrix = Transformation.Combine(translate, cameraScale, aspect);
        }
    }
}