using System;
using Game.Components.Player;
using Game.Components.Renderer.Animations;
using Game.Entity;
using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;
using OpenTK;

namespace Game.Components.Actor.Bandit
{
    public class BanditStateHandler : IActorStateHandler
    {
        public CAnimationSystem AnimationSystem { get; set; }

        public CRigidBody RigidBody { get; set; }

        public IActor Actor { get; set; }

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
            if (!Attacked && Math.Abs(Actor.ActorCombatBehavior.AttackTime - Actor.ActorCombatBehavior.AttackSpeed) < 0.05f)
            {
                AnimationSystem.PlayAnimation("Attack", false, !Actor.FacingRight);
                RigidBody.Velocity = new Vector2(0, RigidBody.Velocity.Y);
                Attacked = true;
            }

            // compute attack hit
            if (Attacked && Actor.ActorCombatBehavior.AttackTime <= Actor.ActorCombatBehavior.AttackSpeed - Actor.ActorCombatBehavior.TimeToHit)
            {
                Actor.Combat.Attack(attackTrigger, 1, false);
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
    }
}