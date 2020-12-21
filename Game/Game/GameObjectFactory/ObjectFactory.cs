using Game.Components;
using Game.SceneSystem;
using Game.Tools;
using OpenTK;

namespace Game.GameObjectFactory
{
    public static class ObjectFactory
    {
        public static GameObject BuildFloor(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(4f, 0.2f);

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

        public static GameObject BuildGround(Scene scene, Vector2 position)
        {
            Vector2 size = new Vector2(30f, 0.2f);

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
            render.SetSize(3.0f, 3.0f);
            render.LoadAndSetTexture("Content.KnightIdle.png");
            render.SetTexCoords(new SimpleGeometry.Rect(0.0f, 0.0f, 1f / 15f, 1.0f));
            render.SetOffset(0.0f, -0.1f);

            player.AddComponent<CBoxCollider>();
            player.GetComponent<CBoxCollider>().Geometry.Size = new Vector2(0.75f, 1.5f);
            player.AddComponent<CRigidBody>();
            player.GetComponent<CRigidBody>().Mass = 3f;
            player.AddComponent<CPlayerController>();
            player.GetComponent<CPlayerController>().RigidBody = player.GetComponent<CRigidBody>();
            player.GetComponent<CPlayerController>().Render = player.GetComponent<CRender>();

            // add ground trigger for playercontroller
            player.AddComponent<CBoxCollider>();
            CBoxCollider trigger = player.GetComponents<CBoxCollider>()[1];
            trigger.IsTrigger = true;
            trigger.Offset = new Vector2(0f, -0.7f);
            trigger.Geometry.Size = new Vector2(0.75f, 0.2f);
            player.GetComponent<CPlayerController>().SetUpGroundTrigger(trigger);

            // add all animations
            player.AddComponent<CAnimationSystem>();
            CAnimationSystem controll = player.GetComponent<CAnimationSystem>();
            controll.Renderer = player.GetComponent<CRender>();
            controll.DefaultTexture = controll.Renderer.Texture;
            controll.SetDefaultColumnsAndRows(15, 1);
            Animation idle = new Animation("Idle", 15, 0, true);
            controll.AddAnimation(idle);
            controll.SetStartAnimation(idle);
            Animation run = new Animation("Run", 8, 0, true, true, "Content.KnightRun.png", 8, 1);
            controll.AddAnimation(run);
            controll.LinkAnimation(run, idle);
            player.GetComponent<CPlayerController>().AnimationSystem = controll;

            return player;
        }
    }
}