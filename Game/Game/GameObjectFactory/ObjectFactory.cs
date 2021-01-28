using System.Diagnostics.CodeAnalysis;
using Game.Components;
using Game.Components.Collision;
using Game.Components.Combat;
using Game.Components.Player;
using Game.Components.Renderer;
using Game.Components.Renderer.Animations;
using Game.Entity;
using Game.SceneSystem;
using OpenTK;

namespace Game.GameObjectFactory
{
    [ExcludeFromCodeCoverage]
    public static class ObjectFactory
    {
        public static GameObject BuildSprite(Scene scene, Vector2 position, Vector2 size, string texture)
        {
            GameObject sprite = new GameObject(scene);
            sprite.Transform.Position = position;

            sprite.AddComponent<CRender>();
            sprite.GetComponent<CRender>().Layer = 9;
            sprite.GetComponent<CRender>().SetSize(size.X, size.Y);
            sprite.GetComponent<CRender>().LoadAndSetTexture($"Content.{texture}");

            return sprite;
        }

        public static GameObject BuildBackground(Scene scene, CTransform target)
        {
            GameObject background = new GameObject(scene);
            background.Transform.Position = Vector2.Zero;

            // background layers
            background.AddComponent<CRender>();
            background.GetComponent<CRender>().LoadAndSetTexture("Content.Environment.background_0.png");
            background.GetComponent<CRender>().Layer = 1;
            background.GetComponent<CRender>().SetSize(16 * 1.5f, 9 * 1.5f);

            background.AddComponent<CParallax>();
            background.GetComponent<CParallax>().Depth = 3;
            background.GetComponent<CParallax>().Target = target;
            background.GetComponent<CParallax>().Render = background.GetComponent<CRender>();

            background.AddComponent<CRender>();
            background.GetComponents<CRender>()[1].LoadAndSetTexture("Content.Environment.background_1.png");
            background.GetComponents<CRender>()[1].Layer = 2;
            background.GetComponents<CRender>()[1].SetSize(16 * 1.5f, 9 * 1.5f);

            background.AddComponent<CParallax>();
            background.GetComponents<CParallax>()[1].Depth = 2;
            background.GetComponents<CParallax>()[1].Target = target;
            background.GetComponents<CParallax>()[1].Render = background.GetComponents<CRender>()[1];

            background.AddComponent<CRender>();
            background.GetComponents<CRender>()[2].LoadAndSetTexture("Content.Environment.background_2.png");
            background.GetComponents<CRender>()[2].Layer = 3;
            background.GetComponents<CRender>()[2].SetSize(16 * 1.5f, 9 * 1.5f);

            background.AddComponent<CParallax>();
            background.GetComponents<CParallax>()[2].Depth = 1;
            background.GetComponents<CParallax>()[2].Target = target;
            background.GetComponents<CParallax>()[2].Render = background.GetComponents<CRender>()[2];

            // ambient Particles
            /*
            background.AddComponent<CParticleSystem>();
            CParticleSystem particleSystem = background.GetComponent<CParticleSystem>();
            particleSystem.Actice = true;
            particleSystem.PositionYRandomness = 32f;
            particleSystem.PositionXRandomness = 18f;
            particleSystem.Offset = new Vector2(0, 9f);
            particleSystem.FadeOut = true;
            particleSystem.SystemColor = Color.Gray;
            particleSystem.MaxParticles = 300;
            particleSystem.MaxParticleLifetime = 1.5f;
            particleSystem.ParticleSpawnRate = 200f;
            */

            return background;
        }

        public static GameObject BuildCamera(Scene scene, Vector2 position)
        {
            GameObject camera = new GameObject(scene, "Camera");
            camera.Transform.Position = position;

            camera.AddComponent<CCamera>();

            return camera;
        }

        public static GameObject BuildWorldText(Scene scene, Vector2 position, string text)
        {
            GameObject textField = new GameObject(scene, "TextField");
            textField.Transform.Position = position;

            textField.AddComponent<CTextRender>();
            textField.GetComponent<CTextRender>().Text = text;

            return textField;
        }

        public static GameObject BuildPlayer(Scene scene, Vector2 position)
        {
            GameObject player = new GameObject(scene, "Player");
            player.Transform.Position = position;
            player.AddComponent<CRender>();
            CRender render = player.GetComponent<CRender>();
            render.SetSize(2.7f, 2.0f);
            render.LoadAndSetTexture("Content.adventurer.png");
            render.SetTexCoords(new SimpleGeometry.Rect(0.0f, 0.0f, 1f / 15f, 1.0f));
            render.SetOffset(0.0f, 0.2f);
            render.Layer = 21;

            player.AddComponent<CBoxCollider>();
            player.GetComponent<CBoxCollider>().Geometry.Size = new Vector2(0.85f, 1.6f);
            player.AddComponent<CRigidBody>();
            player.AddComponent<CPlayerController>();
            player.GetComponent<CPlayerController>().RigidBody = player.GetComponent<CRigidBody>();
            player.GetComponent<CPlayerController>().Render = player.GetComponent<CRender>();

            // add ground trigger for playercontroller
            player.AddComponent<CBoxTrigger>();
            CBoxTrigger trigger = player.GetComponent<CBoxTrigger>();
            trigger.Offset = new Vector2(0f, -0.8f);
            trigger.Geometry.Size = new Vector2(0.84f, 0.2f);
            player.GetComponent<CPlayerController>().SetUpGroundTrigger(trigger);

            // add all animations
            player.AddComponent<CAnimationSystem>();
            CAnimationSystem controll = player.GetComponent<CAnimationSystem>();
            controll.Renderer = player.GetComponent<CRender>();
            controll.DefaultTexture = controll.Renderer.Texture;
            controll.SetDefaultColumnsAndRows(14, 13);
            Animation idle = new Animation("Idle", 4, 0, true);
            idle.TimeBetweenTwoFrames = 1 / 6f;
            controll.AddAnimation(idle);
            controll.SetStartAnimation(idle);
            Animation run = new Animation("Run", 6, 8, true);
            controll.AddAnimation(run);
            Animation jump = new Animation("Jump", 2, 16, false);
            jump.TimeBetweenTwoFrames = 1 / 2.5f;
            controll.AddAnimation(jump);
            Animation fall = new Animation("Fall", 2, 22, true);
            controll.AddAnimation(fall);
            Animation attack1 = new Animation("Attack1", 3, 50, false);
            controll.AddAnimation(attack1);
            Animation attack2 = new Animation("Attack2", 4, 55, false);
            controll.AddAnimation(attack2);
            Animation attack3 = new Animation("Attack3", 3, 44, false);
            controll.AddAnimation(attack3);
            Animation hurt = new Animation("Hurt", 1, 136, false);
            hurt.TimeBetweenTwoFrames = 1 / 3f;
            controll.AddAnimation(hurt);
            Animation death = new Animation("Death", 6, 136, false);
            death.TimeBetweenTwoFrames = 1 / 6f;
            controll.AddAnimation(death);
            Animation roll = new Animation("Roll", 4, 18, false);
            roll.TimeBetweenTwoFrames = 1 / 20f;
            controll.AddAnimation(roll);
            player.GetComponent<CPlayerController>().AnimationSystem = controll;

            // add Combat Components
            player.AddComponent<CCombat>();
            player.GetComponent<CCombat>().AnimationSystem = controll;
            player.GetComponent<CCombat>().CurrentHealth = player.Scene.GameManager.PlayerHealth;
            player.AddComponent<CPlayerCombatController>();
            CPlayerCombatController combatController = player.GetComponent<CPlayerCombatController>();
            combatController.AnimationSystem = controll;
            combatController.Combat = player.GetComponent<CCombat>();
            combatController.Controller = player.GetComponent<CPlayerController>();

            // display player damage
            GameObject damageUi = BuildDisplayPlayerDamage(scene, player, new Vector2(0f, 1.4f));
            player.GetComponent<CCombat>().DamageDisplay = damageUi.GetComponent<CDamageDisplay>();

            // displays pickups like coins and keys
            GameObject pickupDisplay = BuildPickupDisplay(scene, player, new Vector2(0f, 2f));

            // add hitbox to combatcontroller
            player.AddComponent<CBoxTrigger>();
            CBoxTrigger attackHitboxLeft = player.GetComponents<CBoxTrigger>()[1];
            attackHitboxLeft.Geometry.Size = new Vector2(1.3f, 1.7f);
            attackHitboxLeft.Offset = new Vector2(-0.8f, 0.1f);
            combatController.LeftHitbox = attackHitboxLeft;

            player.AddComponent<CBoxTrigger>();
            CBoxTrigger attackHitboxRight = player.GetComponents<CBoxTrigger>()[2];
            attackHitboxRight.Geometry.Size = new Vector2(1.3f, 1.7f);
            attackHitboxRight.Offset = new Vector2(0.8f, 0.1f);
            combatController.RightHitbox = attackHitboxRight;

            // add EffectSystem
            player.AddComponent<CEffectSystem>();
            player.GetComponent<CEffectSystem>().PlayerController = player.GetComponent<CPlayerController>();
            player.GetComponent<CEffectSystem>().Combat = player.GetComponent<CCombat>();
            player.GetComponent<CEffectSystem>().CombatController = player.GetComponent<CPlayerCombatController>();

            // add Particle System
            player.AddComponent<CParticleSystem>();
            CParticleSystem particleSystem = player.GetComponent<CParticleSystem>();
            particleSystem.DirectionRandomness = 20f;
            particleSystem.ForceFieldDirection = new Vector2(0f, -10f);
            particleSystem.UseForceField = true;
            particleSystem.SystemColor = Color.Red;
            particleSystem.FadeColor = Color.DarkRed;
            particleSystem.FadesIntoColor = true;
            particleSystem.MaxParticleLifetime = 0.3f;
            particleSystem.Direction = new Vector2(0, 5);
            particleSystem.ParticleSpawnRate = 200f;
            particleSystem.MaxParticles = 50;
            particleSystem.Actice = false;
            particleSystem.Offset = new Vector2(0, 0.3f);
            particleSystem.Layer = 30;
            particleSystem.PositionXRandomness = 0.1f;
            particleSystem.PositionYRandomness = 1.1f;

            player.GetComponent<CCombat>().BloodParticles = particleSystem;

            return player;
        }

        public static GameObject BuildPowerDown(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "PowerDown");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CRender>();
            powerDown.GetComponent<CRender>().LoadAndSetTexture("Content.default.png");
            powerDown.GetComponent<CRender>().Layer = 19;
            powerDown.AddComponent<CCircleTrigger>();
            CCircleTrigger trigger = powerDown.GetComponent<CCircleTrigger>();
            powerDown.AddComponent<CPowerDownScript>();
            powerDown.GetComponent<CPowerDownScript>().Trigger = trigger;

            return powerDown;
        }

        public static GameObject BuildCoin(Scene scene, Vector2 position)
        {
            GameObject coin = new GameObject(scene, "Coin");
            coin.Transform.Position = position;

            coin.AddComponent<CRender>();
            coin.GetComponent<CRender>().LoadAndSetTexture("Content.AnimatedCoin.png");
            coin.GetComponent<CRender>().SetTexCoords(new SimpleGeometry.Rect(0, 0, 1 / 8f, 1f));
            coin.GetComponent<CRender>().Layer = 20;
            coin.GetComponent<CRender>().SetSize(0.5f, 0.5f);

            coin.AddComponent<CAnimationSystem>();
            CAnimationSystem animationSystem = coin.GetComponent<CAnimationSystem>();
            animationSystem.Renderer = coin.GetComponent<CRender>();
            animationSystem.SetDefaultColumnsAndRows(8, 1);
            Animation spin = new Animation("Spin", 8, 0, true);
            animationSystem.AddAnimation(spin);
            animationSystem.SetStartAnimation(spin);

            coin.AddComponent<CBoxTrigger>();
            CBoxTrigger trigger = coin.GetComponent<CBoxTrigger>();
            trigger.Geometry.Size = new Vector2(1, 1);
            coin.AddComponent<CCollectible>();
            CCollectible collectible = coin.GetComponent<CCollectible>();
            collectible.SetupTrigger(trigger);

            coin.AddComponent<CParticleSystem>();
            CParticleSystem pSystem = coin.GetComponent<CParticleSystem>();
            pSystem.Actice = true;
            pSystem.FadeOut = true;
            pSystem.ParticleSize = 0.1f;
            pSystem.SizeMode = ParticleSizeMode.Shrinking;
            pSystem.SizeChangeSpeed = 0.005f;
            pSystem.Layer = 19;
            pSystem.DirectionRandomness = 10f;
            pSystem.UseForceField = true;
            pSystem.MaxParticleLifetime = 1f;
            pSystem.SystemColor = Color.Wheat;

            return coin;
        }

        public static GameObject BuildKey(Scene scene, Vector2 position)
        {
            GameObject key = new GameObject(scene, "Key");
            key.Transform.Position = position;

            key.AddComponent<CRender>();
            CRender render = key.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.Key.png");
            render.Layer = 20;

            key.AddComponent<CBoxTrigger>();
            CBoxTrigger trigger = key.GetComponent<CBoxTrigger>();
            trigger.Geometry.Size = new Vector2(1, 1);
            key.AddComponent<CCollectible>();
            CCollectible collectible = key.GetComponent<CCollectible>();
            collectible.SetupTrigger(trigger);

            key.AddComponent<CParticleSystem>();
            CParticleSystem pSystem = key.GetComponent<CParticleSystem>();
            pSystem.Actice = true;
            pSystem.FadeOut = true;
            pSystem.ParticleSize = 0.1f;
            pSystem.SizeMode = ParticleSizeMode.Shrinking;
            pSystem.SizeChangeSpeed = 0.005f;
            pSystem.Layer = 19;
            pSystem.DirectionRandomness = 10f;
            pSystem.UseForceField = true;
            pSystem.MaxParticleLifetime = 1.5f;
            pSystem.ParticleSpawnRate = 40f;
            pSystem.MaxParticles = 50;
            pSystem.SystemColor = Color.Aquamarine;

            return key;
        }

        public static GameObject BuildMovingPlatform(Scene scene, Vector2 position1, Vector2 position2, int length)
        {
            GameObject floor = new GameObject(scene, "Floor");
            Vector2 size = new Vector2(length, 1f);

            floor.AddComponent<CPeriodicMovement>();
            CPeriodicMovement periodicMovement = floor.GetComponent<CPeriodicMovement>();
            periodicMovement.Start = position1;
            periodicMovement.End = position2;
            periodicMovement.MoveSpeed = 3f;

            floor.Transform.Position = position1;

            floor.AddComponent<CTileRenderer>();
            floor.GetComponent<CTileRenderer>().Height = 1;
            floor.GetComponent<CTileRenderer>().Width = length;
            floor.AddComponent<CTileRenderer>();
            floor.GetComponents<CTileRenderer>()[1].Height = 2;
            floor.GetComponents<CTileRenderer>()[1].Width = length + 1;
            floor.GetComponents<CTileRenderer>()[1].LoadAndSetTexture("Content.GrassSpritesheet.png");
            floor.GetComponents<CTileRenderer>()[1].Layer = 11;

            floor.AddComponent<CBoxCollider>();
            floor.GetComponent<CBoxCollider>().Geometry.Size = 0.9f * size;

            floor.AddComponent<CRigidBody>();
            CRigidBody rb = floor.GetComponent<CRigidBody>();
            rb.Static = true;

            return floor;
        }

        public static GameObject BuildMovingPlatform(Scene scene, Vector2 position1, Vector2 position2, int length, string tilemapPath)
        {
            GameObject floor = new GameObject(scene, "Floor");
            Vector2 size = new Vector2(length, 1f);

            floor.AddComponent<CPeriodicMovement>();
            CPeriodicMovement periodicMovement = floor.GetComponent<CPeriodicMovement>();
            periodicMovement.Start = position1;
            periodicMovement.End = position2;
            periodicMovement.MoveSpeed = 3f;

            floor.Transform.Position = position1;

            floor.AddComponent<CTileRenderer>();
            floor.GetComponent<CTileRenderer>().Height = 1;
            floor.GetComponent<CTileRenderer>().Width = length;
            floor.GetComponent<CTileRenderer>().LoadAndSetTexture(tilemapPath);

            floor.AddComponent<CBoxCollider>();
            floor.GetComponent<CBoxCollider>().Geometry.Size = 0.9f * size;

            floor.AddComponent<CRigidBody>();
            CRigidBody rb = floor.GetComponent<CRigidBody>();
            rb.Static = true;

            return floor;
        }

        public static GameObject BuildMovingSpikes(Scene scene, Vector2 position, Vector2 goal, int length)
        {
            GameObject spikes = new GameObject(scene, "Spikes");
            position.X = position.X - (length / 2f);
            goal.X = goal.X - (length / 2f);
            spikes.Transform.Position = position;

            spikes.AddComponent<CPeriodicMovement>();
            CPeriodicMovement periodicMovement = spikes.GetComponent<CPeriodicMovement>();
            periodicMovement.Start = position;
            periodicMovement.End = goal;
            periodicMovement.MoveSpeed = 3f;

            for (int i = 0; i < length; i++)
            {
                spikes.AddComponent<CRender>();
                spikes.GetComponents<CRender>()[i].LoadAndSetTexture("Content.Environment.spike.png");
                spikes.GetComponents<CRender>()[i].SetOffset(i * 1f, 0f);
            }

            return spikes;
        }

        public static GameObject BuildMovingDeadlyArea(Scene scene, Vector2 position, Vector2 goal, Vector2 size, Vector2 lastPosition, float dmg)
        {
            GameObject deadlyArea = new GameObject(scene, "DeadlyArea");
            deadlyArea.Transform.Position = position;
            deadlyArea.AddComponent<CPeriodicMovement>();
            CPeriodicMovement periodicMovement = deadlyArea.GetComponent<CPeriodicMovement>();
            periodicMovement.Start = position;
            periodicMovement.End = goal;
            periodicMovement.MoveSpeed = 3f;

            deadlyArea.AddComponent<CBoxTrigger>();
            CBoxTrigger trigger = deadlyArea.GetComponent<CBoxTrigger>();
            trigger.Geometry.Size = size;
            deadlyArea.AddComponent<CResetController>();
            CResetController controller = deadlyArea.GetComponent<CResetController>();
            controller.PreviousPos = lastPosition;
            controller.SetupTrigger(trigger);
            controller.Damage = dmg;

            return deadlyArea;
        }

        private static GameObject BuildDisplayPlayerDamage(Scene scene, GameObject parent, Vector2 position)
        {
            GameObject damageDisplay = new GameObject(scene, "DisplayPlayerDamage", parent);
            damageDisplay.Transform.Position = position;

            damageDisplay.AddComponent<CDamageDisplay>();
            damageDisplay.AddComponent<CTextRender>();
            damageDisplay.GetComponent<CTextRender>().Size = 0.27f;
            damageDisplay.GetComponent<CTextRender>().Centered = true;
            damageDisplay.GetComponent<CDamageDisplay>().DamageText = damageDisplay.GetComponent<CTextRender>();

            return damageDisplay;
        }

        private static GameObject BuildPickupDisplay(Scene scene, GameObject parent, Vector2 position)
        {
            GameObject pickupDisplay = new GameObject(scene, "PickupDisplay", parent);
            pickupDisplay.Transform.Position = position;
            pickupDisplay.AddComponent<CPickupDisplay>();

            GameObject coins = new GameObject(scene, "PickupCoins", pickupDisplay);
            coins.Transform.Position = Vector2.Zero;
            coins.AddComponent<CTextRender>();
            coins.AddComponent<CRender>();
            coins.GetComponent<CTextRender>().Size = 0.23f;
            coins.GetComponent<CTextRender>().Offset = new Vector2(-0.6f, 0f);
            coins.GetComponent<CRender>().SetSize(0.35f, 0.35f);
            coins.GetComponent<CRender>().SetOffset(0.2f, 0.12f);
            coins.GetComponent<CRender>().Layer = 21;

            GameObject keysAndEffects = new GameObject(scene, "PickupKeysAndEffects", pickupDisplay);
            keysAndEffects.Transform.Position = new Vector2(0f, 0.4f);
            keysAndEffects.AddComponent<CTextRender>();
            keysAndEffects.AddComponent<CRender>();
            keysAndEffects.GetComponent<CTextRender>().Size = 0.25f;
            keysAndEffects.GetComponent<CTextRender>().Offset = new Vector2(-0.7f, 0f);
            keysAndEffects.GetComponent<CRender>().SetSize(0.5f, 0.5f);
            keysAndEffects.GetComponent<CRender>().SetOffset(0.2f, 0.12f);
            keysAndEffects.GetComponent<CRender>().Layer = 21;

            pickupDisplay.GetComponent<CPickupDisplay>().TextRenderCoin = coins.GetComponent<CTextRender>();
            pickupDisplay.GetComponent<CPickupDisplay>().RenderCoin = coins.GetComponent<CRender>();
            pickupDisplay.GetComponent<CPickupDisplay>().TextRenderKey = keysAndEffects.GetComponent<CTextRender>();
            pickupDisplay.GetComponent<CPickupDisplay>().RenderKey = keysAndEffects.GetComponent<CRender>();
            return pickupDisplay;
        }
    }
}