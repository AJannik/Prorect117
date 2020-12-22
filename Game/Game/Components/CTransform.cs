using Game.Interfaces;
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

        /// <summary>
        /// Gets or Sets the position.
        /// </summary>
        public Vector2 Position // local coordinates
        {
            get
            {
                return pos;
            }

            set
            {
                pos = value;
                UpdateLocalTransform();
            }
        }

        /// <summary>
        /// Gets the position which has been transformed to world position.
        /// </summary>
        public Vector2 WorldPosition // world coordinates
        {
            get
            {
                if (MyGameObject.GetParent() != null)
                {
                    return Transformation.Transform(pos, MyGameObject.GetParent().Transform.LocalTransformMatrix);
                }

                return pos;
            }
        }

        /// <summary>
        /// Gets or Sets the scale.
        /// </summary>
        public Vector2 Scale // local scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
                UpdateLocalTransform();
            }
        }

        /// <summary>
        /// Gets the scale which has been transformed to world scale.
        /// </summary>
        public Vector2 WorldScale // world scale
        {
            get
            {
                if (MyGameObject.GetParent() != null)
                {
                    // return Transformation.Transform(scale, MyGameObject.GetParent().Transform.TransformMatrix);
                    return Scale * MyGameObject.GetParent().Transform.scale;
                }

                return scale;
            }
        }

        /// <summary>
        /// Gets or Sets rotation of Object in Radiant.
        /// </summary>
        public float Rotation // local rotation
        {
            get
            {
                return rotation.X;
            }

            set
            {
                rotation.X = value;
                UpdateLocalTransform();
            }
        }

        /// <summary>
        /// Gets the roation in Radiant which has been transformed to world rotation.
        /// </summary>
        public float WorldRotation // world rotation
        {
            get
            {
                if (MyGameObject.GetParent() != null)
                {
                    // return Transformation.Transform(rotation, MyGameObject.GetParent().Transform.TransformMatrix).X;
                    return rotation.X + MyGameObject.GetParent().Transform.Rotation;
                }

                return rotation.X;
            }
        }

        public GameObject MyGameObject { get; set; }

        public Matrix4 LocalTransformMatrix { get; private set; }

        public Matrix4 WorldTransformMatrix
        {
            get
            {
                if (MyGameObject.GetParent() != null)
                {
                    return Transformation.Combine(LocalTransformMatrix, MyGameObject.GetParent().Transform.WorldTransformMatrix);
                }

                return LocalTransformMatrix;
            }
        }

        public void Update(float deltaTime)
        {
            if (!MyGameObject.getActive()) return;
        }

        private void UpdateLocalTransform()
        {
            Matrix4 posTransform = Transformation.Translate(pos);
            Matrix4 rotationTransform = Transformation.Rotation(Rotation);
            Matrix4 scaleTransform = Transformation.Scale(scale);

            LocalTransformMatrix = Transformation.Combine(scaleTransform, rotationTransform, posTransform);
        }
    }
}