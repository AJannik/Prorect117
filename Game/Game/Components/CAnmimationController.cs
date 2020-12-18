using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;
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
            if (MyGameObject != null)
            {
                Renderer = MyGameObject.GetComponent<CRender>();
                DefaultTexture = Renderer.Texture;
            }
        }

        public GameObject MyGameObject { get; set; } = null;

        public Rect TexCoords { get; private set; }

        public int Frames { get; private set; }

        public List<Animation> Animations { get; set; }

        public Animation ActiveAnimation { get; set; }

        public Animation StartAnimation { get; private set; }

        private CRender Renderer { get; set; }

        private int Rows { get; set; }

        private int Columns { get; set; }

        private int DefaultTexture { get; set; }

        public void Update(float deltaTime)
        {
            if (Renderer == null)
            {
                return;
            }

            int retFrame = ActiveAnimation.Update(deltaTime);
            if (retFrame == -1)
            {
                GoToNextAnimation();
            }
            else
            {
                SetActiveFrame(retFrame);
            }

            Renderer.SetTexCoords(TexCoords);
        }

        public void AddAnimation(Animation animation)
        {
            Animations.Add(animation);
        }

        public void SetColumnsAndRows(int columns, int rows)
        {
            Rows = rows;
            Columns = columns;
            Frames = rows * columns;
        }

        /// <summary>
        /// Sets the Active Frame.
        /// </summary>
        /// <param name="frameID">Active Frame (starts at 0).</param>
        public void SetActiveFrame(int frameID)
        {
            frameID = frameID % Frames;
            int activeRow = frameID / Columns;
            int activeColumn = frameID % Columns;

            TexCoords = new Rect(activeColumn / (float)Columns, 1f - ((activeRow + 1f) / Rows), 1 / Columns, 1 / Rows);
        }

        /// <summary>
        /// Goes to NextAnimation linked.
        /// </summary>
        public void GoToNextAnimation()
        {
            ActiveAnimation = ActiveAnimation.NextAnimation;
            ActiveAnimation.Update(0f);
            if (ActiveAnimation.HasSeperateTexture)
            {
                Renderer.SetTexture(ActiveAnimation.Texture);
            }
            else
            {
                Renderer.SetTexture(DefaultTexture);
            }
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
                    if (animation.HasSeperateTexture)
                    {
                        Renderer.SetTexture(animation.Texture);
                    }
                    else
                    {
                        Renderer.SetTexture(DefaultTexture);
                    }

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
    }
}
