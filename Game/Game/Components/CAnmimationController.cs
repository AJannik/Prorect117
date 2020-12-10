using System;
using System.Collections.Generic;
using System.Text;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CAnmimationController : IComponent
    {
        public CAnmimationController()
        {
            Rows = 1;
            Columns = 1;
            Frames = 1;
            ActiveRow = 1;
            ActiveColumn = 1;
        }

        public GameObject MyGameObject { get; set; } = null;

        public Rect TexCoords { get; private set; }

        public int ActiveRow { get; private set; }

        public int ActiveColumn { get; private set; }

        public int Frames { get; private set; }

        public List<Animation> Animations { get; set; }

        public Animation ActiveAnimation { get; set; }

        public Animation StartAnimation { get; private set; }

        private CRender Renderer { get; set; }

        private int Rows { get; set; }

        private int Columns { get; set; }

        public void Update(float deltaTime)
        {
            Renderer = MyGameObject.GetComponent<CRender>();
            if (Renderer == null)
            {
                return;
            }

            ActiveAnimation.Update(deltaTime);
            TexCoords = CalculateTexCoords();
            Renderer.SetTexCoords(TexCoords);
        }

        public void AddAnimation(Animation animation)
        {
            Animations.Add(animation);
        }

        public void SetRowsAndColumns(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Frames = rows * columns;
        }

        /// <summary>
        /// Sets the Active Frame.
        /// </summary>
        /// <param name="frame">Active Frame (starts at 1).</param>
        public void SetActiveFrame(int frame)
        {
            frame = frame % Frames;
            ActiveRow = (frame % Rows) + 1;
            ActiveColumn = frame / Columns;
        }

        /// <summary>
        /// Goes to NextAnimation linked.
        /// </summary>
        public void GoToNextAnimation()
        {
            ActiveAnimation = ActiveAnimation.NextAnimation;
            ActiveAnimation.Update(0f);
        }

        /// <summary>
        /// Goes to Animation with Name.
        /// </summary>
        /// <param name="name">Name of the Animation.</param>
        public void GoToAnimation(string name)
        {
            foreach (Animation animation in Animations)
            {
                if (animation.Name == name)
                {
                    ActiveAnimation = animation;
                    return;
                }
            }

            Console.WriteLine("Couldnt find Animation with that Name: " + name);
        }

        public void LinkAnimation(Animation startAnimation, Animation targetAnimation)
        {
            startAnimation.NextAnimation = targetAnimation;
        }

        public void SetStartAnimation(Animation animation)
        {
            StartAnimation = animation;
        }

        private Rect CalculateTexCoords()
        {
            return new Rect((1 / Rows) * (ActiveRow - 1), (1 / Columns) * (ActiveColumn - 1), (1 / Rows) * ActiveRow, (1 / Columns) * ActiveColumn);
        }
    }
}
