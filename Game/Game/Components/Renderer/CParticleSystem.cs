using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Interfaces;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game.Components.Renderer
{
    public enum ParticleSizeMode { Constant, Growing, Shrinking}

    public class CParticleSystem : IComponent, IDrawable, IUpdateable
    {
        private float particleFrequency = 1 / 10f;

        public CParticleSystem()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public int Layer { get; set; } = 20;

        public List<Particle> Particles { get; set; } = new List<Particle>();

        public bool Actice { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the particle alpha is decreased over lifetime.
        /// </summary>
        public bool FadeOut { get; set; } = false;

        public bool FadesIntoColor { get; set; } = false;

        public Color FadeColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets a value indicating whether a ForceField is used. It will accelerate particles in ForceFieldDirection.
        /// </summary>
        public bool UseForceField { get; set; } = false;

        /// <summary>
        /// Gets or sets the Randomness of the speed of Particles.
        /// </summary>
        public float VelocityRandomness { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the Randomness of the angle of Direction.
        /// </summary>
        public float DirectionRandomness { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the Randomness of the particle size.
        /// </summary>
        public float SizeRandomness { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the behaviour of the particle size over time.
        /// </summary>
        public ParticleSizeMode SizeMode { get; set; } = ParticleSizeMode.Constant;

        /// <summary>
        /// Gets or sets the speed at which particle size changes in given ParticleSizeMode.
        /// </summary>
        public float SizeChangeSpeed { get; set; } = 0.1f;

        /// <summary>
        /// Gets or sets the Randomness of X startposition.
        /// </summary>
        public float PositionXRandomness { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the Randomness of Y startposition.
        /// </summary>
        public float PositionYRandomness { get; set; } = 0f;

        public Random Randomizer { get; set; } = new Random();

        public Vector2 ForceFieldDirection { get; set; } = new Vector2(0f, 1f);

        public Color SystemColor { get; set; } = Color.White;

        public Vector2 Direction { get; set; } = new Vector2(0, 1f);

        public float ParticleSize { get; set; } = 0.05f;

        public float MaxParticleLifetime { get; set; } = 3f;

        public int MaxParticles { get; set; } = 50;

        /// <summary>
        /// Gets or sets the Particles that spawn per second.
        /// </summary>
        public float ParticleSpawnRate
        {
            get { return 1f / particleFrequency; } set { particleFrequency = 1f / value; }
        }

        private float ParticleTime { get; set; } = 0f;

        // used for render blending
        private List<Vector2> OldParticlePosition1 { get; set; } = new List<Vector2>();

        private List<Vector2> OldParticlePosition2 { get; set; } = new List<Vector2>();

        private List<Vector2> OldParticlePosition3 { get; set; } = new List<Vector2>();

        private List<Vector2> OldParticlePosition4 { get; set; } = new List<Vector2>();

        public void Update(float deltaTime)
        {
            if (!Actice)
            {
                return;
            }

            foreach (Particle particle in Particles.ToList())
            {
                particle.Update(deltaTime, UseForceField ? ForceFieldDirection : Vector2.Zero);
                if (particle.Lifetime > MaxParticleLifetime)
                {
                    Particles.Remove(particle);
                }
                else if (FadeOut)
                {
                    particle.Alpha = 1f - (0.8f * (particle.Lifetime / MaxParticleLifetime));
                }

                if (FadesIntoColor)
                {
                    particle.ParticleColor = LerpColor(SystemColor, FadeColor, particle.Lifetime / MaxParticleLifetime);
                }

                switch (SizeMode)
                {
                    case ParticleSizeMode.Constant:
                        break;
                    case ParticleSizeMode.Growing:
                        particle.Size += SizeChangeSpeed * deltaTime;
                        break;
                    case ParticleSizeMode.Shrinking:
                        if (particle.Size - (SizeChangeSpeed * deltaTime) > 0)
                        {
                            particle.Size -= SizeChangeSpeed * deltaTime;
                        }

                        break;
                    default:
                        break;
                }
            }

            AddParticles(deltaTime);
        }

        public void Draw(float alpha)
        {
            if (!Actice)
            {
                return;
            }

            int i = 0;
            foreach (Particle particle in Particles)
            {
                DrawSingleParticle(particle, alpha, i);
                i++;
            }
        }

        private void DrawSingleParticle(Particle particle, float alpha, int i)
        {
            Color4 color = particle.ParticleColor;
            color.A = particle.Alpha;
            GL.Color4(color);

            // calculate the corners
            Vector2 pos1 = new Vector2(-(particle.Size / 2f) + particle.RelativePosition.X, -(particle.Size / 2f) + particle.RelativePosition.Y);
            Vector2 pos2 = new Vector2((particle.Size / 2f) + particle.RelativePosition.X, -(particle.Size / 2f) + particle.RelativePosition.Y);
            Vector2 pos3 = new Vector2((particle.Size / 2f) + particle.RelativePosition.X, (particle.Size / 2f) + particle.RelativePosition.Y);
            Vector2 pos4 = new Vector2(-(particle.Size / 2f) + particle.RelativePosition.X, (particle.Size / 2f) + particle.RelativePosition.Y);

            // transform the corners
            pos1 = Transformation.Transform(pos1, MyGameObject.Transform.WorldTransformMatrix);
            pos2 = Transformation.Transform(pos2, MyGameObject.Transform.WorldTransformMatrix);
            pos3 = Transformation.Transform(pos3, MyGameObject.Transform.WorldTransformMatrix);
            pos4 = Transformation.Transform(pos4, MyGameObject.Transform.WorldTransformMatrix);

            // blend
            Vector2 newpos1 = pos1;
            Vector2 newpos2 = pos2;
            Vector2 newpos3 = pos3;
            Vector2 newpos4 = pos4;

            // skip blend if particlelist longer than old list
            if (OldParticlePosition1.Count >= Particles.Count)
            {
                newpos1 = (pos1 * alpha) + (OldParticlePosition1[i] * (1f - alpha));
                newpos2 = (pos2 * alpha) + (OldParticlePosition2[i] * (1f - alpha));
                newpos3 = (pos3 * alpha) + (OldParticlePosition3[i] * (1f - alpha));
                newpos4 = (pos4 * alpha) + (OldParticlePosition4[i] * (1f - alpha));
            }

            // Draw
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(newpos1);
            GL.Vertex2(newpos2);
            GL.Vertex2(newpos3);
            GL.Vertex2(newpos4);

            // update oldpos
            if (OldParticlePosition1.Count < i + 1)
            {
                OldParticlePosition1.Add(pos1);
                OldParticlePosition2.Add(pos2);
                OldParticlePosition3.Add(pos3);
                OldParticlePosition4.Add(pos4);
            }
            else
            {
                OldParticlePosition1[i] = pos1;
                OldParticlePosition2[i] = pos2;
                OldParticlePosition3[i] = pos3;
                OldParticlePosition4[i] = pos4;
            }

            GL.End();
            GL.Color3(Color.White);
        }

        private void AddParticles(float deltaTime)
        {
            ParticleTime += deltaTime;
            while (ParticleTime > particleFrequency)
            {
                ParticleTime -= particleFrequency;
                if (Particles.Count < MaxParticles)
                {
                    Particles.Add(new Particle(
                        new Vector2(((float)Randomizer.NextDouble() - 0.5f) * PositionXRandomness, ((float)Randomizer.NextDouble() - 0.5f) * PositionYRandomness),
                        (((float)Randomizer.NextDouble() * VelocityRandomness) - (VelocityRandomness / 2f) + 1f) * Vector2.Transform(Direction, new Quaternion(new Vector3(0, 0, ((float)Randomizer.NextDouble() * DirectionRandomness) - (DirectionRandomness / 2f)))),
                        SystemColor,
                        (((float)Randomizer.NextDouble() * SizeRandomness) - (SizeRandomness / 2f) + 1f) * ParticleSize));
                }
            }
        }

        private Color LerpColor(Color color1, Color color2, float alpha)
        {
            return new Color((int)((color1.R * (1f - alpha)) + (color2.R * alpha)), (int)((color1.G * (1f - alpha)) + (color2.G * alpha)), (int)((color1.B * (1f - alpha)) + (color2.B * alpha)), (int)((color1.A * (1f - alpha)) + (color2.A * alpha)));
        }
    }
}
