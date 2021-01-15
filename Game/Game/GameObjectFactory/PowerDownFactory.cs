using Game.Components;
using Game.Components.Collision;
using Game.Components.Renderer;
using Game.SceneSystem;
using OpenTK;

namespace Game.GameObjectFactory
{
    public class PowerDownFactory
    {
        public static GameObject Slowness(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "Slowness");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CCircleTrigger>();
            powerDown.GetComponent<CCircleTrigger>().Geometry.Size = new Vector2(1, 1);

            powerDown.AddComponent<CPowerDownScript>();
            CPowerDownScript pdScript = powerDown.GetComponent<CPowerDownScript>();
            pdScript.Effect = Tools.EffectType.Slow;
            pdScript.SetupTrigger(powerDown.GetComponent<CCircleTrigger>());

            powerDown.AddComponent<CRender>();
            CRender render = powerDown.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.PowerDownRed.png");
            render.Layer = 30;
            render.SetSize(1f, 1f);

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

        public static GameObject Vulnerability(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "Vulnerability");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CCircleTrigger>();
            powerDown.GetComponent<CCircleTrigger>().Geometry.Size = new Vector2(1, 1);

            powerDown.AddComponent<CPowerDownScript>();
            CPowerDownScript pdScript = powerDown.GetComponent<CPowerDownScript>();
            pdScript.Effect = Tools.EffectType.Brittle;
            pdScript.SetupTrigger(powerDown.GetComponent<CCircleTrigger>());

            powerDown.AddComponent<CRender>();
            CRender render = powerDown.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.PowerDownGreen.png");
            render.Layer = 30;
            render.SetSize(1f, 1f);

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

        public static GameObject NoRoll(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "NoRoll");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CCircleTrigger>();
            powerDown.GetComponent<CCircleTrigger>().Geometry.Size = new Vector2(1, 1);

            powerDown.AddComponent<CPowerDownScript>();
            CPowerDownScript pdScript = powerDown.GetComponent<CPowerDownScript>();
            pdScript.Effect = Tools.EffectType.Silenced;
            pdScript.SetupTrigger(powerDown.GetComponent<CCircleTrigger>());

            powerDown.AddComponent<CRender>();
            CRender render = powerDown.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.PowerDownBlue.png");
            render.Layer = 30;
            render.SetSize(1f, 1f);

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
            powerDown.GetComponent<CCircleTrigger>().Geometry.Size = new Vector2(1, 1);

            powerDown.AddComponent<CPowerDownScript>();
            CPowerDownScript pdScript = powerDown.GetComponent<CPowerDownScript>();
            pdScript.Effect = Tools.EffectType.Weakness;
            pdScript.SetupTrigger(powerDown.GetComponent<CCircleTrigger>());

            powerDown.AddComponent<CRender>();
            CRender render = powerDown.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.PowerDownYellow.png");
            render.Layer = 30;
            render.SetSize(1f, 1f);

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