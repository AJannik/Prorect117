using System;
using Game.Components.Combat;
using Game.Components.Player;
using Game.Components.Renderer;
using Game.Components.Renderer.Animations;
using Game.Entity;
using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;
using OpenTK;

namespace Game.Components.Actor.Player
{
    public class PlayerStateBehavior : IActorStateBehavior
    {
        public CAnimationSystem AnimationSystem { get; set; }

        public CRigidBody RigidBody { get; set; }

        public CParticleSystem BloodParticles { get; set; }

        public CDamageDisplay DamageDisplay { get; set; }

        public CTextRender HpText { get; set; }

        public IActor Actor { get; set; }

        public bool Jumping { get; set; } = false;

        private CPickupDisplay PickupDisplay { get; set; }

        private bool Died { get; set; } = false;

        public void Idle()
        {
            if (RigidBody.Velocity.X <= 0.001f && RigidBody.Velocity.X >= -0.001f && !Jumping)
            {
                AnimationSystem.PlayAnimation("Idle", false, !Actor.FacingRight);
            }
        }

        public void Running(Vector2 moveSpeed)
        {
            RigidBody.Velocity = new Vector2(moveSpeed.X, RigidBody.Velocity.Y);

            // updating facingRight and animations
            if (RigidBody.Velocity.X > 0.1f)
            {
                Actor.FacingRight = true;
                if (!Jumping)
                {
                    AnimationSystem.PlayAnimation("Run", false, !Actor.FacingRight);
                }
            }
            else if (RigidBody.Velocity.X < -0.1f)
            {
                Actor.FacingRight = false;
                if (!Jumping)
                {
                    AnimationSystem.PlayAnimation("Run", false, !Actor.FacingRight);
                }
            }
        }

        public void Jump(int onGround)
        {
            if (!Jumping && onGround >= 1 && Actor.ActorStats.JumpCooldown <= 0f)
            {
                RigidBody.Velocity = new Vector2(RigidBody.Velocity.X, 0f);
                RigidBody.AddForce(new Vector2(0f, Actor.ActorStats.JumpForce));
                Jumping = true;
                Actor.ActorStats.JumpCooldown = 0.1f;
                AnimationSystem.PlayAnimation("Jump", false, !Actor.FacingRight);
            }

            if (Jumping && RigidBody.Velocity.Y >= 0f)
            {
                RigidBody.GravityScale = 4f;
                AnimationSystem.UpdateFlipped(!Actor.FacingRight);
            }
            else
            {
                RigidBody.GravityScale = 4f;
                Jumping = false;
            }
        }

        public void Fall(int onGround)
        {
            if (RigidBody.Velocity.Y < 0f && onGround < 1)
            {
                AnimationSystem.PlayAnimation("Fall", false, !Actor.FacingRight);
            }
        }

        public void SetXVelocity(float vX)
        {
            RigidBody.Velocity = new OpenTK.Vector2(vX, RigidBody.Velocity.Y);
        }

        public void Roll()
        {
            Actor.CombatController.MakeInvincible(0.2f);
            RigidBody.Velocity = new OpenTK.Vector2(Actor.FacingRight ? 20f : -20f, RigidBody.Velocity.Y);
            AnimationSystem.PlayAnimation("Roll", true);
        }

        public void Attacking(ITrigger attackTrigger)
        {
            Actor.CombatController.Attack(attackTrigger, 1f, false);
            AnimationSystem.PlayAnimation("Attack1", true, !Actor.FacingRight);
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

        public void TakeDamage(float dmgAmount, bool ignoreArmor)
        {
            float dmg = Actor.CombatController.CalculateDamage(dmgAmount, ignoreArmor, Actor.ActorStats.Armor);
            Actor.ActorStats.CurrentHealth -= dmg;
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