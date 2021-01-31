using System;
using Game.Components.Player;
using Game.Components.Renderer.Animations;
using Game.Entity;
using Game.Interfaces;
using OpenTK;

namespace Game.Components.Actor
{
    public class BanditStateHandler
    {
        public CAnimationSystem AnimationSystem { get; set; }

        public CRigidBody RigidBody { get; set; }

        public CBandit Bandit { get; set; }

        private CPickupDisplay PickupDisplay { get; set; }

        private bool Attacked { get; set; } = false;

        private bool Died { get; set; } = false;

        public void Idle()
        {
            AnimationSystem.PlayAnimation("Idle", false, !Bandit.FacingRight);
            RigidBody.Velocity = new Vector2(0, RigidBody.Velocity.Y);
        }

        public void Running(float moveSpeed)
        {
            if (Bandit.RightOnGround < 1)
            {
                Bandit.FacingRight = false;
            }

            if (Bandit.LeftOnGround < 1)
            {
                Bandit.FacingRight = true;
            }

            RigidBody.Velocity = new Vector2((Bandit.FacingRight ? 1 : -1) * moveSpeed, RigidBody.Velocity.Y);
            AnimationSystem.PlayAnimation("Walk", false, !Bandit.FacingRight);
        }

        public void Attacking(ITrigger attackTrigger)
        {
            // Start attack animation
            if (!Attacked && Math.Abs(Bandit.BanditCombatBehavior.AttackTime - Bandit.BanditCombatBehavior.AttackSpeed) < 0.05f)
            {
                AnimationSystem.PlayAnimation("Attack", false, !Bandit.FacingRight);
                RigidBody.Velocity = new Vector2(0, RigidBody.Velocity.Y);
                Attacked = true;
            }

            // compute attack hit
            if (Attacked && Bandit.BanditCombatBehavior.AttackTime <= Bandit.BanditCombatBehavior.AttackSpeed - Bandit.BanditCombatBehavior.TimeToHit)
            {
                Bandit.Combat.Attack(attackTrigger, 1, false);
                Attacked = false;
            }
        }

        public void Dying()
        {
            AnimationSystem.PlayAnimation("Death", true);
            if (!Died && Bandit.MyGameObject.Name != "Player")
            {
                Bandit.MyGameObject.Scene.GameManager.Coins += 2;
                PickupDisplay?.AddCoins(2);
                Died = true;
            }
        }

        public void Dead()
        {
            Bandit.MyGameObject.Scene.RemoveGameObject(Bandit.MyGameObject);
            if (Bandit.MyGameObject.Name == "Player")
            {
                Bandit.MyGameObject.Scene.GameManager.EndGame();
                Bandit.MyGameObject.Scene.GameManager.PlayerWon = false;
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