using Game.Components;
using Game.SceneSystem;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.GameObjectFactory
{
    public static class ObjectFactory
    {
        public static GameObject BuildWall2(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(0.2f, 2f);
            return BuildWall(scene, position, size);
        }

        public static GameObject BuildWall3(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(0.2f, 3f);
            return BuildWall(scene, position, size);
        }

        public static GameObject BuildWall4(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(0.2f, 4f);
            return BuildWall(scene, position, size);
        }

        public static GameObject BuildLevelWall(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(0.2f, 32f);
            return BuildWall(scene, position, size);
        }

        public static GameObject BuildPlatform2(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(2f, 0.2f);
            return BuildPlatform(scene, position, size);
        }

        public static GameObject BuildPlatform3(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(3f, 0.2f);
            return BuildPlatform(scene, position, size);
        }

        public static GameObject BuildPlatform4(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(4f, 0.2f);
            return BuildPlatform(scene, position, size);
        }

        public static GameObject BuildGround(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(32f, 0.2f);
            return BuildPlatform(scene, position, size);
        }

        public static GameObject BuildBall(Scene scene, Vector2 position)
        {
            GameObject ball = new GameObject(scene, "Ball");
            ball.Transform.Position = position;

            ball.AddComponent<CRender>();
            ball.GetComponent<CRender>().SetSize(0.2f, 0.2f);
            ball.AddComponent<CCircleCollider>();
            ball.GetComponent<CCircleCollider>().Geometry.Size = new Vector2(0.2f, 0f);
            ball.AddComponent<CTriggerEventTest>();
            ball.AddComponent<CRigidBody>();

            ball.AddComponent<CBoxCollider>();
            CBoxCollider trigger = ball.GetComponent<CBoxCollider>();
            trigger.IsTrigger = true;
            trigger.Offset = new Vector2(0f, -0.2f);
            trigger.Geometry.Size = new Vector2(0.2f, 0.1f);

            return ball;
        }

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
            trigger.Offset = new Vector2(0f, -0.8f);
            trigger.Geometry.Size = new Vector2(0.75f, 0.2f);
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
            controll.AddAnimation(jump);
            Animation fall = new Animation("Fall", 2, 23, true);
            controll.AddAnimation(fall);
            player.GetComponent<CPlayerController>().AnimationSystem = controll;

            return player;
        }

        public static GameObject BuildSkeletonEnemy(Scene scene, Vector2 position)
        {
            GameObject enemy = new GameObject(scene, "Enemy");
            enemy.Transform.Position = position;

            // render
            enemy.AddComponent<CRender>();
            CRender render = enemy.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.Skeleton.SkeletonIdle.png");
            render.SetTexCoords(new SimpleGeometry.Rect(0f, 0f, 1f / 11f, 1f));
            render.SetSize(2f, 2f);
            render.SetOffset(0.3f, 0f);

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
            left.Geometry.Size = new Vector2(0.7f, 1.8f);
            right.Geometry.Size = new Vector2(0.7f, 1.8f);
            left.Offset = new Vector2(-0.6f, 0f);
            right.Offset = new Vector2(0.6f, 0f);

            enemy.AddComponent<CRigidBody>();

            enemy.AddComponent<CEnemyAI>();
            CEnemyAI ai = enemy.GetComponent<CEnemyAI>();
            ai.LeftTrigger = left;
            ai.RightTrigger = right;
            ai.RigidBody = enemy.GetComponent<CRigidBody>();

            // ai.RigidBody.Velocity = new Vector2(ai.MoveSpeed, 0f);

            // animations
            enemy.AddComponent<CAnimationSystem>();
            CAnimationSystem animationSystem = enemy.GetComponent<CAnimationSystem>();
            Animation idle = new Animation("Idle", 11, 0, true);
            animationSystem.Renderer = render;
            animationSystem.AddAnimation(idle);
            animationSystem.SetDefaultColumnsAndRows(11, 1);
            animationSystem.SetStartAnimation(idle);
            Animation walk = new Animation("Walk", 13, 0, true, true, "Content.Skeleton.SkeletonWalk.png", 13, 1);
            animationSystem.AddAnimation(walk);

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

        private static GameObject BuildPlatform(Scene scene, Vector2 position, Vector2 size)
        {
            GameObject floor = new GameObject(scene, "Floor");
            floor.Transform.Position = position;
            floor.Transform.Scale = size;

            floor.AddComponent<CRender>();
            floor.AddComponent<CBoxCollider>();
            floor.GetComponent<CBoxCollider>().Geometry.Size = size;

            floor.AddComponent<CRigidBody>();
            CRigidBody rb = floor.GetComponent<CRigidBody>();
            rb.Static = true;

            return floor;
        }

        private static GameObject BuildWall(Scene scene, Vector2 position, Vector2 size)
        {
            GameObject wall = new GameObject(scene, "Wall");
            wall.Transform.Position = position;
            wall.Transform.Scale = size;

            wall.AddComponent<CRender>();
            wall.AddComponent<CBoxCollider>();
            wall.GetComponent<CBoxCollider>().Geometry.Size = size;

            wall.AddComponent<CRigidBody>();
            CRigidBody rb = wall.GetComponent<CRigidBody>();
            rb.Static = true;

            return wall;
        }
    }
}