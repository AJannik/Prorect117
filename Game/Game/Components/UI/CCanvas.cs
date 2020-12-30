using Game.Interfaces;
using Game.Tools;
using OpenTK;

namespace Game.Components.UI
{
    public class CCanvas : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; }

        public Matrix4 CanvasMatrix { get; set; } = Matrix4.Identity;

        public void Update(float deltaTime)
        {
            UpdateMatrix();
        }

        private void UpdateMatrix()
        {
            Matrix4 aspect = Transformation.Scale(16f / 9f, 1f);

            Matrix4 canvasScale = Transformation.Scale(MyGameObject.GetComponent<CCamera>().Scale);

            CanvasMatrix = Transformation.Combine(canvasScale, aspect);
        }
    }
}