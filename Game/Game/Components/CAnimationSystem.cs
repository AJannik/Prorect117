using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CAnimationSystem : IComponent
    {
        private CRender render;

        public GameObject MyGameObject { get; set; } = null;

        public Rect TexCoords { get; private set; }

        public int DefaultFrames { get; private set; } = 1;

        public int Frames { get; private set; } = 1;

        public List<Animation> Animations { get; set; } = new List<Animation>();

        public Animation ActiveAnimation { get; set; }

        public Animation StartAnimation { get; private set; }

        public CRender Renderer
        {
            get
            {
                return render;
            }

            set
            {
                render = value;
                DefaultTexture = render.Texture;
            }
        }

        public int DefaultTexture { get; set; }

        private int DefaultRows { get; set; } = 1;

        private int DefaultColumns { get; set; } = 1;

        private int Rows { get; set; } = 1;

        private int Columns { get; set; } = 1;

        private bool ForceEnd { get; set; } = false;

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
            if (animation == null)
            {
                return;
            }

            Animations.Add(animation);
        }

        public void SetDefaultColumnsAndRows(int columns, int rows)
        {
            DefaultColumns = columns;
            Columns = columns;
            DefaultRows = rows;
            Rows = rows;
            DefaultFrames = rows * columns;
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
            int activeRow = frameID / Columns;
            int activeColumn = frameID % Columns;

            TexCoords = new Rect(activeColumn / (float)Columns, 1f - ((activeRow + 1f) / Rows), 1f / Columns, 1f / Rows);
        }

        /// <summary>
        /// Goes to NextAnimation linked. IF there is none goes back to StartAnimation.
        /// </summary>
        public void GoToNextAnimation()
        {
            ForceEnd = false;
            ActiveAnimation.ActiveFrame = ActiveAnimation.StartFrame;
            if (ActiveAnimation.NextAnimation == null)
            {
                ActiveAnimation = StartAnimation;
            }
            else
            {
                ActiveAnimation = ActiveAnimation.NextAnimation;
            }

            ActiveAnimation.Update(0f);
            if (ActiveAnimation.HasSeperateTexture)
            {
                Renderer.SetTexture(ActiveAnimation.Texture);
                SetColumnsAndRows(ActiveAnimation.Columns, ActiveAnimation.Rows);
                Frames = ActiveAnimation.Frames + ActiveAnimation.StartFrame;
            }
            else
            {
                Renderer.SetTexture(DefaultTexture);
                SetColumnsAndRows(DefaultColumns, DefaultRows);
                Frames = DefaultFrames;
            }
        }

        /// <summary>
        /// Plays the animation with given name if its not already playing and a loop.
        /// </summary>
        /// <param name="name">Name of the Animation.</param>
        public void PlayAnimation(string name)
        {
            if (ActiveAnimation == null)
            {
                return;
            }

            if ((ActiveAnimation.Name == name && ActiveAnimation.IsLoop) || ForceEnd)
            {
                return;
            }

            foreach (Animation animation in Animations)
            {
                if (animation.Name == name)
                {
                    ActiveAnimation.ActiveFrame = ActiveAnimation.StartFrame;
                    ActiveAnimation = animation;
                    if (animation.HasSeperateTexture)
                    {
                        Renderer.SetTexture(animation.Texture);
                        SetColumnsAndRows(ActiveAnimation.Columns, ActiveAnimation.Rows);
                        Frames = ActiveAnimation.Frames + ActiveAnimation.StartFrame;
                    }
                    else
                    {
                        Renderer.SetTexture(DefaultTexture);
                        SetColumnsAndRows(DefaultColumns, DefaultRows);
                        Frames = DefaultFrames;
                    }

                    return;
                }
            }

            Console.WriteLine("Couldnt find Animation with that Name: " + name);
        }

        /// <summary>
        /// Plays the animation with given name if its not already playing and a loop.
        /// </summary>
        /// <param name="name">Name of the Animation.</param>
        /// <param name="forceEnd">Animation will play all the way without any interrupt except when another force is called.</param>
        public void PlayAnimation(string name, bool forceEnd)
        {
            // remove previous force if forcing
            if (forceEnd)
            {
                ForceEnd = false;
            }

            PlayAnimation(name);
            if (!ActiveAnimation.IsLoop)
            {
                ForceEnd = forceEnd;
            }
        }

        /// <summary>
        /// Plays the animation with given name if its not already playing and a loop.
        /// </summary>
        /// <param name="name">Name of the Animation.</param>
        /// <param name="forceEnd">Animation will play all the way without any interrupt.</param>
        /// <param name="faceLeft">Will force animation to face a certain direction.</param>
        public void PlayAnimation(string name, bool forceEnd, bool faceLeft)
        {
            // remove previous force if forcing
            if (forceEnd)
            {
                ForceEnd = false;
                Renderer.Flipped = faceLeft;
            }

            PlayAnimation(name);
            if (!ActiveAnimation.IsLoop && !ForceEnd)
            {
                ForceEnd = forceEnd;
            }

            if (!ForceEnd)
            {
                Renderer.Flipped = faceLeft;
            }
        }

        /// <summary>
        /// Returns the Animation with given Name. If not found returns null.
        /// </summary>
        /// <param name="name">Name of the Animation.</param>
        /// <returns>Animation with the Name.</returns>
        public Animation GetAnimation(string name)
        {
            foreach (Animation animation in Animations)
            {
                if (animation.Name == name)
                {
                    return animation;
                }
            }

            return null;
        }

        public void LinkAnimation(Animation startAnimation, Animation targetAnimation)
        {
            startAnimation.NextAnimation = targetAnimation;
        }

        public void SetStartAnimation(Animation animation)
        {
            StartAnimation = animation;
            ActiveAnimation = StartAnimation;
        }
    }
}