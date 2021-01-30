using Game.Entity;
using Game.Interfaces;
using OpenTK;

namespace Game.Components
{
    public class CPeriodicMovement : IUpdateable, IComponent
    {
        private Vector2 start;
        private Vector2 end;
        private float moveSpeed;

        public GameObject MyGameObject { get; set; }

        public Vector2 Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
                UpdateMoveVector();
            }
        }

        public Vector2 End
        {
            get
            {
                return end;
            }

            set
            {
                end = value;
                UpdateMoveVector();
            }
        }

        public float MoveSpeed
        {
            get
            {
                return moveSpeed;
            }

            set
            {
                moveSpeed = value;
                UpdateMoveVector();
            }
        }

        private bool Forward { get; set; } = true;

        private Vector2 MoveVector { get; set; }

        public void Update(float deltaTime)
        {
            Vector2 movement = MoveVector * deltaTime;
            MyGameObject.Transform.Position = MyGameObject.Transform.Position + movement;

            float x = MyGameObject.Transform.Position.X;
            float y = MyGameObject.Transform.Position.Y;

            if ((x > End.X && x > Start.X) || (x < End.X && x < Start.X) || (y > End.Y && y > Start.Y) || (y < End.Y && y < Start.Y))
            {
                if (Forward)
                {
                    MyGameObject.Transform.Position = Start;
                    MoveVector = MoveVector * -1;
                    Forward = !Forward;
                }
                else
                {
                    MyGameObject.Transform.Position = End;
                    MoveVector = MoveVector * -1;
                    Forward = !Forward;
                }
            }
        }

        private void UpdateMoveVector()
        {
            MoveVector = Vector2.Normalize(Vector2.Subtract(Start, End)) * MoveSpeed;
        }
    }
}