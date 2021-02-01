using System;
using Game.Components.Combat;
using Game.Components.Player;
using Game.Components.Renderer;
using Game.Components.Renderer.Animations;
using Game.Entity;
using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;
using OpenTK;

namespace Game.Components.Actor.Bandit
{
    public class BanditStateBehavior : IActorStateBehavior
    {
        public CAnimationSystem AnimationSystem { get; set; }

        public CRigidBody RigidBody { get; set; }

        public IActor Actor { get; set; }

        public CParticleSystem BloodParticles { get; set; }

        public CTextRender HpText { get; set; }

        public CDamageDisplay DamageDisplay { get; set; }

        private CPickupDisplay PickupDisplay { get; set; }

        private bool Attacked { get; set; } = false;

        private bool Died { get; set; } = false;

        public void Idle()
        {
            AnimationSystem.PlayAnimation("Idle", false, !Actor.FacingRight);
            RigidBody.Velocity = new Vector2(0, RigidBody.Velocity.Y);
        }

        public void Running(float moveSpeed)
        {
            if (Actor.RightOnGround < 1)
            {
                Actor.FacingRight = false;
            }

            if (Actor.LeftOnGround < 1)
            {
                Actor.FacingRight = true;
            }

            RigidBody.Velocity = new Vector2((Actor.FacingRight ? 1 : -1) * moveSpeed, RigidBody.Velocity.Y);
            AnimationSystem.PlayAnimation("Walk", false, !Actor.FacingRight);
        }

        public void Attacking(ITrigger attackTrigger)
        {
            // Start attack animation
            if (!Attacked && Math.Abs(Actor.ActorStats.AttackTime - Actor.ActorStats.AttackSpeed) < 0.05f)
            {
                AnimationSystem.PlayAnimation("Attack", false, !Actor.FacingRight);
                RigidBody.Velocity = new Vector2(0, RigidBody.Velocity.Y);
                Attacked = true;
            }

            // compute attack hit
            if (Attacked && Actor.ActorStats.AttackTime <= Actor.ActorStats.AttackSpeed - Actor.ActorStats.TimeToHit)
            {
                Actor.CombatController.Attack(attackTrigger, 1, false);
                Attacked = false;
            }
        }

        public void Dying()
        {
            AnimationSystem.PlayAnimation("Death", true);
            if (!Died && Actor.MyGameObject.Name != "Player")
            {
                Actor.MyGameObject.Scene.GameManager.Coins += 2;
                PickupDisplay?.AddCoins(2);
                Died = true;
            }
        }

        public void Dead()
        {
            Actor.MyGameObject.Scene.RemoveGameObject(Actor.MyGameObject);
            if (Actor.MyGameObject.Name == "Player")
            {
                Actor.MyGameObject.Scene.GameManager.EndGame();
                Actor.MyGameObject.Scene.GameManager.PlayerWon = false;
            }
        }

        public void SetupPickupDisplay(GameObject myGameObject)
        {
            foreach (GameObject gameObject in myGameObject.Scene.GetGameObjects())
            {
                if (gameObject.Name != "Player")
                {
                    continue;
                }

                foreach (GameObject child in gameObject.GetAllChildren())
                {
                    if (child.GetComponent<CPickupDisplay>() != null)
                    {
                        PickupDisplay = child.GetComponent<CPickupDisplay>();
                    }
                }
            }
        }

        public void TakeDamage(float dmgAmount, bool ignoreArmor)
        {
            float dmg = Actor.CombatController.CalculateDamage(dmgAmount, ignoreArmor, Actor.ActorStats.Armor);
            Actor.ActorStats.CurrentHealth -= dmg;
            Actor.ActorStats.BleedTime = 0.1f;

            Actor.ActorStats.BleedTime = 0.1f;

            // UI display of damage
            if (Actor.MyGameObject.Name == "Player")
            {
                Actor.MyGameObject.Scene.GameManager.PlayerHealth = Actor.ActorStats.CurrentHealth;
                DamageDisplay?.DisplayDamage($"{MathF.Ceiling(Actor.ActorStats.CurrentHealth)}/{Actor.ActorStats.MaxHealth}", Color.Red);
            }
            else
            {
                DamageDisplay?.DisplayDamage($"-{MathF.Ceiling(dmg)}", Color.Red);
            }
        }

        public void HandleBloodEffect(float deltaTime)
        {
            if (Actor.ActorStats.BleedTime > 0f && BloodParticles != null)
            {
                Actor.ActorStats.BleedTime -= deltaTime;
                BloodParticles.Actice = true;
            }
            else if (BloodParticles != null)
            {
                BloodParticles.Actice = false;
            }
        }
    }
}