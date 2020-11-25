using Game.Tools;
using OpenTK;

namespace Game.Components
{
    public class CTransform : IComponent
    {
        private Vector2 pos = new Vector2(0f, 0f); // local coordinates
        private Vector2 scale = new Vector2(1f, 1f); // local scale
        private Vector2 rotation = new Vector2(0f, 0f); // local roation

        public CTransform()
        {
            UpdateLocalTransform();
        }

        public Vector2 Position // world coordinates
        {
            get
            {
                if (MyGameObject.GetParent() != null)
                {
                    return Transformation.Transform(pos, MyGameObject.GetParent().Transform.TransformMatrix);
                }

                return pos;
            }

            set
            {
                pos = value;
                UpdateLocalTransform();
            }
        }

        public Vector2 Scale // world scale
        {
            get
            {
                if (MyGameObject.GetParent() != null)
                {
                    return Transformation.Transform(scale, MyGameObject.GetParent().Transform.TransformMatrix);
                }

                return scale;
            }

            set
            {
                scale = value;
                UpdateLocalTransform();
            }
        }

        // TODO: radient or degrees?
        public float Rotation // world rotation
        {
            get
            {
                if (MyGameObject.GetParent() != null)
                {
                    return Transformation.Transform(rotation, MyGameObject.GetParent().Transform.TransformMatrix).X;
                }

                return rotation.X;
            }

            set
            {
                rotation.X = value;
                UpdateLocalTransform();
            }
        }

        public GameObject MyGameObject { get; set; }

        private Matrix4 TransformMatrix { get; set; }

        public void Update(float deltaTime)
        {
        }

        // TODO: TESTS
        // TODO: Adjust parent hierarchy. Eventually the top GameObject will be the Scene.
        private void UpdateLocalTransform()
        {
            Matrix4 posTransform = Transformation.Translate(pos);
            Matrix4 rotationTransform = Transformation.Rotation(rotation.X);
            Matrix4 scaleTransform = Transformation.Scale(scale);

            TransformMatrix = Transformation.Combine(posTransform, rotationTransform, scaleTransform);
        }
    }
}