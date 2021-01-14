using Game.Components;
using Game.Components.Collision;
using Game.Components.Renderer;
using Game.SceneSystem;
using Game.Tools;
using OpenTK;

namespace Game.GameObjectFactory
{
    public static class EnemyFactory
    {
        // A bit stronger than the bandit
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
            render.Layer = 20;

            // hitboxes and triggers
            enemy.AddComponent<CBoxCollider>();
            CBoxCollider hitbox = enemy.GetComponent<CBoxCollider>();
            hitbox.Geometry.Size = new Vector2(1f, 1.6f);
            enemy.AddComponent<CBoxTrigger>();
            CBoxTrigger left = enemy.GetComponent<CBoxTrigger>();
            enemy.AddComponent<CBoxTrigger>();
            CBoxTrigger right = enemy.GetComponents<CBoxTrigger>()[1];
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
            Animation attack = new Animation("Attack", 18, 0, false, true, "Content.Skeleton.SkeletonAttack.png", 18, 1);
            animationSystem.AddAnimation(attack);
            Animation death = new Animation("Death", 15, 0 false, true, "Content.Skeleton.SkeletonDead", 15, 1);
            ai.AnimationSystem = animationSystem;

            // combat
            enemy.AddComponent<CCombat>();
            CCombat combat = enemy.GetComponent<CCombat>();
            combat.AnimationSystem = animationSystem;
            combat.MaxHealth = 40;
            combat.AttackDamage = 25;
            ai.Combat = combat;

            // Hp Text
            BuildEnemyHpText(scene, enemy, new Vector2(0f, 1.1f));

            return enemy;
        }

        // Weak enemy for tutorial purpose
        public static GameObject BuildBanditEnemy(Scene scene, Vector2 position)
        {
            GameObject enemy = new GameObject(scene, "Enemy");
            enemy.Transform.Position = position;

            // render
            enemy.AddComponent<CRender>();
            CRender render = enemy.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.LightBandit.png");
            render.SetTexCoords(new SimpleGeometry.Rect(0f, 0f, 1f / 8f, 1f / 5f));
            render.SetSize(2.3f, 2.3f);
            render.SetOffset(0.0f, 0.25f);
            render.Layer = 20;

            // hitboxes and triggers
            enemy.AddComponent<CBoxCollider>();
            CBoxCollider hitbox = enemy.GetComponent<CBoxCollider>();
            hitbox.Geometry.Size = new Vector2(1f, 1.6f);
            enemy.AddComponent<CBoxTrigger>();
            CBoxTrigger left = enemy.GetComponent<CBoxTrigger>();
            enemy.AddComponent<CBoxTrigger>();
            CBoxTrigger right = enemy.GetComponents<CBoxTrigger>()[1];
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
            idle.TimeBetweenTwoFrames = 1 / 9f;
            animationSystem.Renderer = render;
            animationSystem.AddAnimation(idle);
            animationSystem.SetDefaultColumnsAndRows(8, 5);
            animationSystem.SetStartAnimation(idle);
            Animation walk = new Animation("Walk", 8, 8, true);
            walk.TimeBetweenTwoFrames = 1 / 10f;
            animationSystem.AddAnimation(walk);
            Animation hurt = new Animation("Hurt", 3, 32, false);
            animationSystem.AddAnimation(hurt);
            Animation attack = new Animation("Attack", 8, 16, false);
            animationSystem.AddAnimation(attack);
            Animation death = new Animation("Death", 1, 36, false);
            death.TimeBetweenTwoFrames = 100f;
            animationSystem.AddAnimation(death);
            ai.AnimationSystem = animationSystem;

            // combat
            enemy.AddComponent<CCombat>();
            CCombat combat = enemy.GetComponent<CCombat>();
            combat.AnimationSystem = animationSystem;
            combat.MaxHealth = 30;
            ai.Combat = combat;

            // Hp Text
            BuildEnemyHpText(scene, enemy, new Vector2(0f, 1.1f));

            return enemy;
        }

        public static GameObject BuildEnemyHpText(Scene scene, GameObject parent, Vector2 position)
        {
            GameObject enemyHpText = new GameObject(scene, $"{parent.Name}HpText", parent);
            enemyHpText.Transform.Position = position;

            enemyHpText.AddComponent<CTextRender>();
            enemyHpText.GetComponent<CTextRender>().Size = 0.2f;
            enemyHpText.GetComponent<CTextRender>().Centered = true;
            parent.GetComponent<CCombat>().HpText = enemyHpText.GetComponent<CTextRender>();

            return enemyHpText;
        }
    }
}