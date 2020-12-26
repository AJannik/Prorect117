using Game.Interfaces;
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

        public CBoxCollider LeftHitbox { get; set; }

        public CBoxCollider RightHitbox { get; set; }

        private float ComboTime { get; set; } = 0f;

        private int ComboCount { get; set; } = 0;

        public void Update(float deltaTime)
        {
            var mouse = Mouse.GetState();
            if (mouse.IsButtonDown(MouseButton.Left) && mouse.X < 0.5f)
            {
                ComboAttack(true);
            }
            else if (mouse.IsButtonDown(MouseButton.Left) && mouse.X >= 0.5f)
            {
                ComboAttack(false);
            }

            if (ComboTime >= 0f)
            {
                ComboTime -= deltaTime;
            }
            else
            {
                ComboCount = 0;
            }
        }

        private void ComboAttack(bool leftSide)
        {
            bool successful;
            CBoxCollider collider = RightHitbox;
            if (leftSide)
            {
                collider = LeftHitbox;
            }

            switch (ComboCount)
            {
                case 0:
                    successful = Attack1(collider, leftSide);
                    break;
                case 1:
                    successful = Attack2(collider, leftSide);
                    break;
                case 2:
                    successful = Attack3(collider, leftSide);
                    break;
                default:
                    ComboCount = 0;
                    successful = Attack1(collider, leftSide);
                    break;
            }

            if (successful)
            {
                ComboCount++;
                ComboTime = 2f;
            }
        }

        private bool Attack1(ICollider hitbox, bool leftSide)
        {
            if (Combat.Attack(hitbox, 1f, false))
            {
                AnimationSystem.PlayAnimation("Attack1", true, leftSide);
                return true;
            }

            return false;
        }

        private bool Attack2(ICollider hitbox, bool leftSide)
        {
            if (Combat.Attack(hitbox, 1.5f, false))
            {
                AnimationSystem.PlayAnimation("Attack2", true, leftSide);
                return true;
            }

            return false;
        }

        private bool Attack3(ICollider hitbox, bool leftSide)
        {
            if (Combat.Attack(hitbox, 3f, false))
            {
                AnimationSystem.PlayAnimation("Attack3", true, leftSide);
                return true;
            }

            return false;
        }
    }
}
