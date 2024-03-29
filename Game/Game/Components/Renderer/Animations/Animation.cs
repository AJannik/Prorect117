﻿namespace Game.Components.Renderer.Animations
{
    public class Animation
    {
        private float animationTime = 1 / 12f;

        public Animation(string name, int frames, int startFrame, bool isLoop)
        {
            Name = name;
            Frames = frames;
            StartFrame = startFrame;
            ActiveFrame = StartFrame;
            IsLoop = isLoop;
        }

        public Animation(string name, int frames, int startFrame, bool isLoop, bool hasSeperateTexture, string seperateTexturePath, int columns, int rows)
        {
            Name = name;
            Frames = frames;
            StartFrame = startFrame;
            ActiveFrame = StartFrame;
            IsLoop = isLoop;
            HasSeperateTexture = hasSeperateTexture;
            Columns = columns;
            Rows = rows;
            if (hasSeperateTexture)
            {
                Texture = TextureTools.LoadFromResource(seperateTexturePath);
            }
        }

        public int Frames { get; private set; }

        public int StartFrame { get; private set; }

        public int ActiveFrame { get; set; }

        public string Name { get; private set; }

        public Animation NextAnimation { get; set; } = null;

        public float TimeBetweenTwoFrames
        {
            get
            {
                return animationTime;
            }

            set
            {
                animationTime = value;
                TimeToNextFrame = animationTime;
            }
        }

        public bool HasSeperateTexture { get; private set; } = false;

        public int Columns { get; private set; } = 0;

        public int Rows { get; private set; } = 0;

        public int Texture { get; private set; }

        public bool IsLoop { get; set; } = false;

        private float TimeToNextFrame { get; set; } = 1 / 12f;

        /// <summary>
        /// Updates animation and returns the FrameID or -1 for GoToNext.
        /// </summary>
        /// <param name="deltaTime">Time between calculated frames.</param>
        /// <returns>FrameID of current frame. Returns -1 if the animation ended.</returns>
        public int Update(float deltaTime)
        {
            TimeToNextFrame -= deltaTime;

            // check if time has been reached
            if (TimeToNextFrame < 0)
            {
                TimeToNextFrame += TimeBetweenTwoFrames;
                ActiveFrame++;

                // check if last frame+1 has been reached
                if (ActiveFrame >= StartFrame + Frames)
                {
                    if (IsLoop)
                    {
                        ActiveFrame = StartFrame;
                    }
                    else
                    {
                        // reset to start and go to next animation
                        ActiveFrame = StartFrame;

                        // return gotonext
                        return -1;
                    }
                }

                // return Active Frame
                return ActiveFrame;
            }

            return ActiveFrame;
        }
    }
}