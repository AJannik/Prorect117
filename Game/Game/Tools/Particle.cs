using System;
using System.Collections.Generic;
using System.Text;
using Game.Components;
using Game.Interfaces;
using OpenTK;

namespace Game.Tools
{
    public class Particle
    {
        private float alpha = 1f;

        public Particle(Vector2 startOffset, Vector2 startVelocity, Color color, float size)
        {
            RelativePosition = startOffset;
            Velocity = startVelocity;
            ParticleColor = color;
        }

        public Particle(Vector2 startOffset, Vector2 startVelocity, Color color, float size, float startLifetime)
        {
            RelativePosition = startOffset;
            Velocity = startVelocity;
            ParticleColor = color;
            Lifetime = startLifetime;
        }

        public Vector2 RelativePosition { get; set; }

        public Vector2 Velocity { get; set; }

        public Color ParticleColor { get; set; } = Color.White;

        public float Size { get; set; } = 0.05f;

        public float Alpha
        {
            get { return alpha; }
            set { alpha = Math.Clamp(value, 0f, 1f); }
        }

        public float Lifetime { get; set; } = 0f;

        public void Update(float deltaTime, Vector2 force)
        {
            RelativePosition += Velocity * deltaTime;
            Lifetime += deltaTime;
            Velocity += force * deltaTime;
        }
    }
}
