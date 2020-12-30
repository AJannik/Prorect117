using Game.Components;
using Game.Components.Colision;
using Game.Components.Renderer;
using Game.SceneSystem;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.GameObjectFactory
{
    public static class ObjectFactory
    {
        public static GameObject BuildSprite(Scene scene, Vector2 position)
        {
            GameObject sprite = new GameObject(scene);
            sprite.Transform.Position = position;

            sprite.AddComponent<CRender>();

            return sprite;
        }

        public static GameObject BuildCamera(Scene scene, Vector2 position)
        {
            GameObject camera = new GameObject(scene, "Camera");
            camera.Transform.Position = position;

            camera.AddComponent<CCamera>();

            return camera;
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
            render.SetOffset(0.0f, 0.1f);
            render.Layer = 12;

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

            // add text
            player.AddComponent<CTextRender>();
            player.GetComponent<CTextRender>().Text = "Player";
            player.GetComponent<CTextRender>().Offset = new Vector2(-1f, 2f);

            return player;
        }

        public static GameObject BuildPowerDown(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "PowerDown");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CRender>();
            powerDown.GetComponent<CRender>().LoadAndSetTexture("Content.default.png");
            powerDown.AddComponent<CCircleTrigger>();
            CCircleTrigger trigger = powerDown.GetComponent<CCircleTrigger>();
            powerDown.AddComponent<CPowerDownScript>();
            powerDown.GetComponent<CPowerDownScript>().Trigger = trigger;

            return powerDown;
        }

        public static GameObject BuildLevelEnd(Scene scene, Vector2 position, Vector2 size)
        {
            GameObject levelEnd = new GameObject(scene, "LevelEnd");

            // TODO: Add door sprite, hitbox, trigger

            return levelEnd;
        }

        public static GameObject BuildCoin(Scene scene, Vector2 position)
        {
            GameObject coin = new GameObject(scene, "Coin");

            // TODO: Add sprite and hitbox

            return coin;
        }

        public static GameObject BuildKey(Scene scene, Vector2 position)
        {
            GameObject key = new GameObject(scene, "Key");

            // TODO: Add sprite and hitbox

            return key;
        }
    }
}