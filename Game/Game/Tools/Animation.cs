using System;
using System.Collections.Generic;
using System.Text;
using Game.Components;

namespace Game.Tools
{
    public class Animation
    {
        public Animation(string name, int frames, int startFrame, bool isLoop)
        {
            Name = name;
            Frames = frames;
            StartFrame = startFrame;
            ActiveFrame = StartFrame;
            Loop = isLoop;
        }

        public Animation(string name, int frames, int startFrame, bool isLoop, bool hasSeperateTexture, string seperateTexturePath, int columns, int rows)
        {
            Name = name;
            Frames = frames;
            StartFrame = startFrame;
            ActiveFrame = StartFrame;
            Loop = isLoop;
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

        public int ActiveFrame { get; private set; }

        public string Name { get; private set; }

        public Animation NextAnimation { get; set; } = null;

        public float TimeBetweenTwoFrames { get; set; } = 1 / 12f;

        public bool HasSeperateTexture { get; private set; } = false;

        public int Columns { get; private set; } = 0;

        public int Rows { get; private set; } = 0;

        public int Texture { get; private set; }

        private float TimeToNextFrame { get; set; } = 0f;

        private bool Loop { get; set; } = false;

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
                    if (Loop)
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