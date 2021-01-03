using Game.Components;
using Game.Components.Collision;
using Game.Components.Renderer;
using Game.Components.UI;
using Game.SceneSystem;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.GameObjectFactory
{
    public static class ObjectFactory
    {
        public static GameObject BuildSprite(Scene scene, Vector2 position, string texture)
        {
            GameObject sprite = new GameObject(scene);
            sprite.Transform.Position = position;

            sprite.AddComponent<CRender>();
            sprite.GetComponent<CRender>().LoadAndSetTexture($"Content.{texture}");

            return sprite;
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
            trigger.Offset = new Vector2(0f, -0.77f);
            trigger.Geometry.Size = new Vector2(0.84f, 0.1f);
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
            jump.TimeBetweenTwoFrames = 1 / 3.3f;
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
            player.GetComponent<CPlayerController>().AnimationSystem = controll;

            // add Combat Components
            player.AddComponent<CCombat>();
            player.GetComponent<CCombat>().AnimationSystem = controll;
            player.GetComponent<CCombat>().CurrentHealth = player.Scene.GameManager.PlayerHealth;
            player.AddComponent<CPlayerCombatController>();
            CPlayerCombatController combatController = player.GetComponent<CPlayerCombatController>();
            combatController.AnimationSystem = controll;
            combatController.Combat = player.GetComponent<CCombat>();

            // add hitbox to combatcontroller
            player.AddComponent<CBoxTrigger>();
            CBoxTrigger attackHitboxLeft = player.GetComponents<CBoxTrigger>()[1];
            attackHitboxLeft.Geometry.Size = new Vector2(1f, 1.5f);
            attackHitboxLeft.Offset = new Vector2(-0.9f, 0f);
            combatController.LeftHitbox = attackHitboxLeft;

            player.AddComponent<CBoxTrigger>();
            CBoxTrigger attackHitboxRight = player.GetComponents<CBoxTrigger>()[2];
            attackHitboxRight.Geometry.Size = new Vector2(1f, 1.5f);
            attackHitboxRight.Offset = new Vector2(0.9f, 0f);
            combatController.RightHitbox = attackHitboxRight;

            // add EffectSystem
            player.AddComponent<CEffectSystem>();
            player.GetComponent<CEffectSystem>().PlayerController = player.GetComponent<CPlayerController>();
            player.GetComponent<CEffectSystem>().Combat = player.GetComponent<CCombat>();

            // add text
            player.AddComponent<CTextRender>();
            player.GetComponent<CTextRender>().Offset = new Vector2(0f, 1f);
            player.GetComponent<CTextRender>().Size = 0.3f;
            player.GetComponent<CTextRender>().Centered = true;
            combatController.TextRender = player.GetComponent<CTextRender>();

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

        public static GameObject BuildLevelEnd(Scene scene, Vector2 position, Vector2 size)
        {
            GameObject levelEnd = new GameObject(scene, "LevelEnd");
            levelEnd.Transform.Position = position;

            levelEnd.AddComponent<CRender>();
            CRender render = levelEnd.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.DoorSprite.png");
            render.SetSize(1f, 2f);
            render.SetTexCoords(new SimpleGeometry.Rect(0f, 0f, 0.5f, 1.0f));

            // TODO: Add door hitbox, trigger

            return levelEnd;
        }

        public static GameObject BuildCoin(Scene scene, Vector2 position)
        {
            GameObject coin = new GameObject(scene, "Coin");
            coin.Transform.Position = position;

            coin.AddComponent<CRender>();
            coin.GetComponent<CRender>().LoadAndSetTexture("Content.goldcoin1.png");
            coin.GetComponent<CRender>().Layer = 11;

            // TODO: Add hitbox and script
            return coin;
        }

        public static GameObject BuildKey(Scene scene, Vector2 position)
        {
            GameObject key = new GameObject(scene, "Key");
            key.Transform.Position = position;

            key.AddComponent<CRender>();
            CRender render = key.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.Key.png");
            render.Layer = 11;

            // TODO: Add hitbox and script
            return key;
        }
    }
}