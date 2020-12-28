using Game.Components;
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
            player.AddComponent<CBoxCollider>();
            CBoxCollider trigger = player.GetComponents<CBoxCollider>()[1];
            trigger.IsTrigger = true;
            trigger.Offset = new Vector2(0f, -0.77f);
            trigger.Geometry.Size = new Vector2(0.95f, 0.1f);
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
            player.AddComponent<CBoxCollider>();
            CBoxCollider attackHitboxLeft = player.GetComponents<CBoxCollider>()[2];
            attackHitboxLeft.IsTrigger = true;
            attackHitboxLeft.Geometry.Size = new Vector2(1f, 1.5f);
            attackHitboxLeft.Offset = new Vector2(-0.9f, 0f);
            combatController.LeftHitbox = attackHitboxLeft;

            player.AddComponent<CBoxCollider>();
            CBoxCollider attackHitboxRight = player.GetComponents<CBoxCollider>()[3];
            attackHitboxRight.IsTrigger = true;
            attackHitboxRight.Geometry.Size = new Vector2(1f, 1.5f);
            attackHitboxRight.Offset = new Vector2(0.9f, 0f);
            combatController.RightHitbox = attackHitboxRight;

            return player;
        }

        public static GameObject BuildBanditEnemy(Scene scene, Vector2 position)
        {
            GameObject enemy = new GameObject(scene, "Enemy");
            enemy.Transform.Position = position;

            // render
            enemy.AddComponent<CRender>();
            CRender render = enemy.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.LightBandit.png");
            render.SetTexCoords(new SimpleGeometry.Rect(0f, 0f, 1f / 8f, 1f / 5f));
            render.SetSize(2.5f, 2.5f);
            render.SetOffset(0.0f, 0.3f);
            render.Layer = 11;

            // hitboxes and triggers
            enemy.AddComponent<CBoxCollider>();
            CBoxCollider hitbox = enemy.GetComponent<CBoxCollider>();
            hitbox.Geometry.Size = new Vector2(1f, 1.6f);
            enemy.AddComponent<CBoxCollider>();
            CBoxCollider left = enemy.GetComponents<CBoxCollider>()[1];
            enemy.AddComponent<CBoxCollider>();
            CBoxCollider right = enemy.GetComponents<CBoxCollider>()[2];
            left.IsTrigger = true;
            right.IsTrigger = true;
            left.Geometry.Size = new Vector2(0.8f, 1.8f);
            right.Geometry.Size = new Vector2(0.8f, 1.8f);
            left.Offset = new Vector2(-0.7f, 0f);
            right.Offset = new Vector2(0.7f, 0f);

            enemy.AddComponent<CRigidBody>();

            enemy.AddComponent<CEnemyAI>();
            CEnemyAI ai = enemy.GetComponent<CEnemyAI>();
            ai.SetupLeftTrigger(left);
            ai.SetupRightTrigger(right);
            ai.RigidBody = enemy.GetComponent<CRigidBody>();

            // animations
            enemy.AddComponent<CAnimationSystem>();
            CAnimationSystem animationSystem = enemy.GetComponent<CAnimationSystem>();
            Animation idle = new Animation("Idle", 4, 0, true);
            animationSystem.Renderer = render;
            animationSystem.AddAnimation(idle);
            animationSystem.SetDefaultColumnsAndRows(8, 5);
            animationSystem.SetStartAnimation(idle);
            Animation walk = new Animation("Walk", 8, 8, true);
            animationSystem.AddAnimation(walk);
            Animation hurt = new Animation("Hurt", 3, 32, false);
            animationSystem.AddAnimation(hurt);
            Animation attack = new Animation("Attack", 8, 16, false);
            animationSystem.AddAnimation(attack);
            ai.AnimationSystem = animationSystem;

            // combat
            enemy.AddComponent<CCombat>();
            CCombat combat = enemy.GetComponent<CCombat>();
            combat.AnimationSystem = animationSystem;
            combat.MaxHealth = 30;
            ai.Combat = combat;

            return enemy;
        }

        public static GameObject BuildPowerDown(Scene scene, Vector2 position)
        {
            GameObject powerDown = new GameObject(scene, "PowerDown");
            powerDown.Transform.Position = position;

            powerDown.AddComponent<CRender>();
            powerDown.GetComponent<CRender>().LoadAndSetTexture("Content.default.png");
            powerDown.AddComponent<CCircleCollider>();
            CCircleCollider trigger = powerDown.GetComponent<CCircleCollider>();
            powerDown.AddComponent<CPowerDownScript>();
            powerDown.GetComponent<CPowerDownScript>().Trigger = trigger;

            return powerDown;
        }
    }
}