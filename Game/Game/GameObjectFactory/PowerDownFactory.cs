using System.Diagnostics.CodeAnalysis;
using Game.Components;
using Game.Components.Collision;
using Game.Components.Renderer;
using Game.Entity;
using Game.SceneSystem;
using OpenTK;

namespace Game.GameObjectFactory
{
    [ExcludeFromCodeCoverage]
    public static class PowerDownFactory
    {
        public static GameObject Slowness(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "Slowness");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CCircleTrigger>();
            powerDown.GetComponent<CCircleTrigger>().Geometry.Size = new Vector2(0.7f, 0.7f);

            powerDown.AddComponent<CPowerDownScript>();
            CPowerDownScript pdScript = powerDown.GetComponent<CPowerDownScript>();
            pdScript.Effect = Tools.EffectType.Slow;
            pdScript.SetupTrigger(powerDown.GetComponent<CCircleTrigger>());

            powerDown.AddComponent<CRender>();
            CRender render = powerDown.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.PowerDowns.slowness.png");
            render.Layer = 22;
            render.SetSize(0.8f, 0.8f);

            powerDown.AddComponent<CParticleSystem>();
            CParticleSystem particleSystem = powerDown.GetComponent<CParticleSystem>();
            particleSystem.SystemColor = Color.White;
            particleSystem.FadeColor = Color.GreenYellow;
            particleSystem.FadeOut = true;
            particleSystem.FadesIntoColor = true;
            particleSystem.MaxParticles = 40;
            particleSystem.ParticleSpawnRate = 30;
            particleSystem.UseForceField = false;
            particleSystem.Actice = true;
            particleSystem.DirectionRandomness = 10f;
            particleSystem.MaxParticleLifetime = 1.5f;
            particleSystem.SizeRandomness = 0.5f;
            particleSystem.SizeMode = ParticleSizeMode.Shrinking;
            particleSystem.SizeChangeSpeed = 0.01f;
            particleSystem.ParticleSize = 0.4f;

            return powerDown;
        }

        public static GameObject Fragile(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "Fragile");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CCircleTrigger>();
            powerDown.GetComponent<CCircleTrigger>().Geometry.Size = new Vector2(0.7f, 0.7f);

            powerDown.AddComponent<CPowerDownScript>();
            CPowerDownScript pdScript = powerDown.GetComponent<CPowerDownScript>();
            pdScript.Effect = Tools.EffectType.Fragile;
            pdScript.SetupTrigger(powerDown.GetComponent<CCircleTrigger>());

            powerDown.AddComponent<CRender>();
            CRender render = powerDown.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.PowerDowns.fragile.png");
            render.Layer = 22;
            render.SetSize(0.8f, 0.8f);

            powerDown.AddComponent<CParticleSystem>();
            CParticleSystem particleSystem = powerDown.GetComponent<CParticleSystem>();
            particleSystem.SystemColor = Color.White;
            particleSystem.FadeColor = Color.GreenYellow;
            particleSystem.FadeOut = true;
            particleSystem.FadesIntoColor = true;
            particleSystem.MaxParticles = 40;
            particleSystem.ParticleSpawnRate = 30;
            particleSystem.UseForceField = false;
            particleSystem.Actice = true;
            particleSystem.DirectionRandomness = 10f;
            particleSystem.MaxParticleLifetime = 1.5f;
            particleSystem.SizeRandomness = 0.5f;
            particleSystem.SizeMode = ParticleSizeMode.Shrinking;
            particleSystem.SizeChangeSpeed = 0.01f;
            particleSystem.ParticleSize = 0.4f;

            return powerDown;
        }

        public static GameObject Silenced(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "Silenced");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CCircleTrigger>();
            powerDown.GetComponent<CCircleTrigger>().Geometry.Size = new Vector2(0.7f, 0.7f);

            powerDown.AddComponent<CPowerDownScript>();
            CPowerDownScript pdScript = powerDown.GetComponent<CPowerDownScript>();
            pdScript.Effect = Tools.EffectType.Silenced;
            pdScript.SetupTrigger(powerDown.GetComponent<CCircleTrigger>());

            powerDown.AddComponent<CRender>();
            CRender render = powerDown.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.PowerDowns.silenced.png");
            render.Layer = 22;
            render.SetSize(0.8f, 0.8f);

            powerDown.AddComponent<CParticleSystem>();
            CParticleSystem particleSystem = powerDown.GetComponent<CParticleSystem>();
            particleSystem.SystemColor = Color.White;
            particleSystem.FadeColor = Color.GreenYellow;
            particleSystem.FadeOut = true;
            particleSystem.FadesIntoColor = true;
            particleSystem.MaxParticles = 40;
            particleSystem.ParticleSpawnRate = 30;
            particleSystem.UseForceField = false;
            particleSystem.Actice = true;
            particleSystem.DirectionRandomness = 10f;
            particleSystem.MaxParticleLifetime = 1.5f;
            particleSystem.SizeRandomness = 0.5f;
            particleSystem.SizeMode = ParticleSizeMode.Shrinking;
            particleSystem.SizeChangeSpeed = 0.01f;
            particleSystem.ParticleSize = 0.4f;

            return powerDown;
        }

        public static GameObject Weakness(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "Weakness");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CCircleTrigger>();
            powerDown.GetComponent<CCircleTrigger>().Geometry.Size = new Vector2(0.7f, 0.7f);

            powerDown.AddComponent<CPowerDownScript>();
            CPowerDownScript pdScript = powerDown.GetComponent<CPowerDownScript>();
            pdScript.Effect = Tools.EffectType.Weakness;
            pdScript.SetupTrigger(powerDown.GetComponent<CCircleTrigger>());

            powerDown.AddComponent<CRender>();
            CRender render = powerDown.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.PowerDowns.weakness.png");
            render.Layer = 22;
            render.SetSize(0.8f, 0.8f);

            powerDown.AddComponent<CParticleSystem>();
            CParticleSystem particleSystem = powerDown.GetComponent<CParticleSystem>();
            particleSystem.SystemColor = Color.White;
            particleSystem.FadeColor = Color.GreenYellow;
            particleSystem.FadeOut = true;
            particleSystem.FadesIntoColor = true;
            particleSystem.MaxParticles = 40;
            particleSystem.ParticleSpawnRate = 30;
            particleSystem.UseForceField = false;
            particleSystem.Actice = true;
            particleSystem.DirectionRandomness = 10f;
            particleSystem.MaxParticleLifetime = 1.5f;
            particleSystem.SizeRandomness = 0.5f;
            particleSystem.SizeMode = ParticleSizeMode.Shrinking;
            particleSystem.SizeChangeSpeed = 0.01f;
            particleSystem.ParticleSize = 0.4f;

            return powerDown;
        }
    }
}