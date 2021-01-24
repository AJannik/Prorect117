using Game.Interfaces;
using Game.Tools;
using OpenTK;

namespace Game.Components.UI.BaseComponents
{
    public class CCanvas : IComponent, IResizeable
    {
        public GameObject MyGameObject { get; set; } = null;

        public CCamera Camera { get; set; } = null;

        public Matrix4 CanvasDrawMatrix { get; set; } = Matrix4.Identity;

        public Matrix4 CanvasMouseMatrix { get; set; } = Matrix4.Identity;

        public void Resize(int width, int height)
        {
            UpdateDrawMatrix();
            UpdateMouseMatrix();
        }

        private void UpdateDrawMatrix()
        {
            Matrix4 aspect = Transformation.Scale((float)Camera.YAspect / (float)Camera.XAspect, 1f);

            CanvasDrawMatrix = Transformation.Combine(aspect);
        }

        private void UpdateMouseMatrix()
        {
            float width = Camera.ViewportWidth;
            float height = Camera.ViewportHeight;
            Matrix4 aspect = Transformation.Translate(-Camera.ViewportX, -Camera.ViewportY / 2f);
            Matrix4 translate = Transformation.Translate(-width / 2f, -height / 2f);
            Matrix4 scale = Transformation.Scale(2f / width, -2f / height);

            CanvasMouseMatrix = Transformation.Combine(translate, aspect, scale);
        }
    }
}