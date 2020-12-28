using Game.Components;
using Game.SceneSystem;
using Game.Tools;
using OpenTK;

namespace Game.GameObjectFactory
{
    public static class EnemyFactory
    {
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
            Animation idle = new Animation("Idle", 11, 0, true);
            animationSystem.Renderer = render;
            animationSystem.AddAnimation(idle);
            animationSystem.SetDefaultColumnsAndRows(11, 1);
            animationSystem.SetStartAnimation(idle);
            Animation walk = new Animation("Walk", 13, 0, true, true, "Content.Skeleton.SkeletonWalk.png", 13, 1);
            animationSystem.AddAnimation(walk);
            Animation hurt = new Animation("Hurt", 8, 0, false, true, "Content.Skeleton.SkeletonHit.png", 8, 1);
            animationSystem.AddAnimation(hurt);
            ai.AnimationSystem = animationSystem;

            // combat
            enemy.AddComponent<CCombat>();
            CCombat combat = enemy.GetComponent<CCombat>();
            combat.AnimationSystem = animationSystem;
            combat.MaxHealth = 30;

            return enemy;
        }
    }
}