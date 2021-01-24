﻿using System.Diagnostics.CodeAnalysis;
using Game.Components;
using Game.Components.Collision;
using Game.Components.Combat;
using Game.Components.Renderer;
using Game.SceneSystem;
using Game.Tools;
using OpenTK;

namespace Game.GameObjectFactory
{
    [ExcludeFromCodeCoverage]
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
            render.SetSize(2.4f, 2f);
            render.SetOffset(0.5f, 0.22f);
            render.Layer = 20;

            // hitboxes and triggers
            enemy.AddComponent<CBoxCollider>();
            CBoxCollider hitbox = enemy.GetComponent<CBoxCollider>();
            hitbox.Geometry.Size = new Vector2(1.2f, 1.6f);
            enemy.AddComponent<CBoxTrigger>();
            CBoxTrigger left = enemy.GetComponent<CBoxTrigger>();
            enemy.AddComponent<CBoxTrigger>();
            CBoxTrigger right = enemy.GetComponents<CBoxTrigger>()[1];
            left.Geometry.Size = new Vector2(1.3f, 1.8f);
            right.Geometry.Size = new Vector2(1.3f, 1.8f);
            left.Offset = new Vector2(-0.7f, 0f);
            right.Offset = new Vector2(0.7f, 0f);

            enemy.AddComponent<CRigidBody>();

            enemy.AddComponent<CEnemyAI>();
            CEnemyAI ai = enemy.GetComponent<CEnemyAI>();
            ai.SetupLeftTrigger(left);
            ai.SetupRightTrigger(right);
            ai.RigidBody = enemy.GetComponent<CRigidBody>();
            ai.HitTime = 0.8f;
            ai.InAttackTime = 1.8f;

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
            attack.TimeBetweenTwoFrames = 1f / 10f;
            animationSystem.AddAnimation(attack);
            Animation death = new Animation("Death", 15, 0, false, true, "Content.Skeleton.SkeletonDead.png", 15, 1);
            death.TimeBetweenTwoFrames = 1f / 15f;
            animationSystem.AddAnimation(death);
            ai.AnimationSystem = animationSystem;

            // combat
            enemy.AddComponent<CCombat>();
            CCombat combat = enemy.GetComponent<CCombat>();
            combat.AnimationSystem = animationSystem;
            combat.Armor = 66f;
            combat.AttackDamage = 80f;
            combat.MaxHealth = 40f;
            ai.Combat = combat;

            // Hp Text
            GameObject damageUi = BuildEnemyHpText(scene, enemy, new Vector2(0f, 1.1f));
            combat.DamageDisplay = damageUi.GetComponent<CDamageDisplay>();

            // add Particle System
            enemy.AddComponent<CParticleSystem>();
            CParticleSystem particleSystem = enemy.GetComponent<CParticleSystem>();
            particleSystem.DirectionRandomness = 20f;
            particleSystem.ForceFieldDirection = new Vector2(0f, -10f);
            particleSystem.UseForceField = true;
            particleSystem.SystemColor = Color.White;
            particleSystem.FadeColor = Color.LightGray;
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

            enemy.GetComponent<CCombat>().BloodParticles = particleSystem;

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
            animationSystem.FlippedAnimations = true;
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
            combat.Armor = 33f;
            combat.AttackDamage = 30f;
            combat.MaxHealth = 30f;
            ai.Combat = combat;

            // Hp Text
            GameObject damageUi = BuildEnemyHpText(scene, enemy, new Vector2(0f, 1.1f));
            combat.DamageDisplay = damageUi.GetComponent<CDamageDisplay>();

            // add Particle System
            enemy.AddComponent<CParticleSystem>();
            CParticleSystem particleSystem = enemy.GetComponent<CParticleSystem>();
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

            enemy.GetComponent<CCombat>().BloodParticles = particleSystem;

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

            enemyHpText.AddComponent<CDamageDisplay>();
            enemyHpText.AddComponent<CTextRender>();
            enemyHpText.GetComponents<CTextRender>()[1].Size = 0.2f;
            enemyHpText.GetComponents<CTextRender>()[1].Centered = true;
            enemyHpText.GetComponents<CTextRender>()[1].Offset = new Vector2(0f, 0.3f);
            enemyHpText.GetComponent<CDamageDisplay>().DamageText = enemyHpText.GetComponents<CTextRender>()[1];

            return enemyHpText;
        }
    }
}