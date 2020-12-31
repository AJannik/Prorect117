using Game.Interfaces;
using Game.Tools;
using OpenTK;

namespace Game.Components.UI
{
    public class CCanvas : IComponent, IResizeable
    {
        public GameObject MyGameObject { get; set; } = null;

        public CCamera Camera { get; set; } = null;

        public Matrix4 CanvasMatrix { get; set; } = Matrix4.Identity;

        public void Resize(int width, int height)
        {
            UpdateMatrix();
        }

        private void UpdateMatrix()
        {
            Matrix4 aspect = Transformation.Scale(9f / 16f, 1f);

            CanvasMatrix = Transformation.Combine(aspect);
        }
    }
}