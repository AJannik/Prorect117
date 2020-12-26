using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;
using Game.Tools;
using OpenTK.Input;

namespace Game.Components
{
    public class CPlayerCombatController : IComponent
    {
        public CPlayerCombatController()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public CAnimationSystem AnimationSystem { get; set; }

        public CCombat Combat { get; set; }

        public CBoxCollider Hitbox { get; set; }

        public void Update(float deltaTime)
        {
            var mouse = Mouse.GetState();
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                // attack
            }
        }

        private void Attack1()
        {
            if (Combat.Attack(Hitbox, 1f, false))
            {
                AnimationSystem.PlayAnimation("Attack1", true);
            }
        }

        private void Attack2()
        {
            if (Combat.Attack(Hitbox, 1.5f, false))
            {
                AnimationSystem.PlayAnimation("Attack2", true);
            }
        }

        private void Attack3()
        {
            if (Combat.Attack(Hitbox, 3f, false))
            {
                AnimationSystem.PlayAnimation("Attack3", true);
            }
        }
    }
}
