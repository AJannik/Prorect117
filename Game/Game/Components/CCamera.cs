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

        public Vector2 Center
        {
            get
            {
                return center;
            }

            set
            {
                center = value;
                UpdateMatrix();
            }
        }

        public GameObject MyGameObject { get; set; }

        public Matrix4 CameraMatrix => cameraMatrix;

        private GameObject FocusObject { get; set; }

        private CTransform Transform { get; set; }

        public void Update(float deltaTime)
        {
            // throw new NotImplementedException();
        }

        public void Draw()
        {
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
            // TODO: Implement window aspect ratio scaling
            // TODO: Implement camera scaling
            // TODO: Implement rotation around position
            // TODO: Implement panning
            // TODO: Calculate the resulting camera matrix
            throw new NotImplementedException();
        }
    }
}