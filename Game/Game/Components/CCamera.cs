using System;
using Game.Interfaces;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CCamera : IComponent
    {
        private Vector2 center;
        private float windowAspectRatio;
        private Matrix4 cameraMatrix = Matrix4.Identity;

        public CCamera()
        {
        }

        public GameObject MyGameObject { get; set; }

        public Matrix4 CameraMatrix => cameraMatrix;

        private CTransform Transform { get; set; }

        public void Update(float deltaTime)
        {
            UpdateMatrix();
        }

        public void Draw()
        {
            UpdateMatrix();
            GL.LoadMatrix(ref cameraMatrix);
        }

        public void Resize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            windowAspectRatio = width / height;
            UpdateMatrix();
        }

        public void UpdateMatrix()
        {
            cameraMatrix = Tools.Transformation.Translate(MyGameObject.Transform.WorldPosition);
        }
    }
}