﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Components;

namespace Game.Tools
{
    public class Animation
    {
        // TODO: remove usage of animationcontroller!
        public Animation(string name, int frames, int startFrame, bool isLoop)
        {
            Name = name;
            Frames = frames;
            StartFrame = startFrame;
            ActiveFrame = StartFrame;
            Loop = isLoop;
        }

        public int Frames { get; private set; }

        public int StartFrame { get; private set; }

        public int ActiveFrame { get; private set; }

        public string Name { get; private set; }

        public Animation NextAnimation { get; set; } = null;

        public float TimeBetweenTwoFrames { get; set; } = 0.2f;

        private float TimeToNextFrame { get; set; } = 0f;

        private bool Loop { get; set; } = false;

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
