using Game.Components.Collision;
using Game.Components.Renderer;
using Game.Interfaces;
using OpenTK.Input;

namespace Game.Components
{
    public class CPlayerCombatController : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; } = null;

        public CAnimationSystem AnimationSystem { get; set; }

        public CCombat Combat { get; set; }

        public CBoxTrigger LeftHitbox { get; set; }

        public CBoxTrigger RightHitbox { get; set; }

        public CTextRender TextRender { get; set; }

        public CPlayerController Controller { get; set; }

        private float ComboTime { get; set; } = 0f;

        private int ComboCount { get; set; } = 0;

        private float LockTime { get; set; } = 0f;

        public void Update(float deltaTime)
        {
            var mouse = Mouse.GetState();
            if (mouse.IsButtonDown(MouseButton.Left) && AnimationSystem.Renderer.Flipped)
            {
                ComboAttack(true);
            }
            else if (mouse.IsButtonDown(MouseButton.Left) && !AnimationSystem.Renderer.Flipped)
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

            if (LockTime > 0f)
            {
                LockTime -= deltaTime;
            }
            else
            {
                Controller.State = PlayerState.Free;
            }

            TextRender.Text = ((int)Combat.CurrentHealth).ToString() + "/" + ((int)Combat.MaxHealth).ToString();
        }

        private void ComboAttack(bool leftSide)
        {
            bool successful;
            CBoxTrigger trigger = RightHitbox;
            if (leftSide)
            {
                trigger = LeftHitbox;
            }

            switch (ComboCount)
            {
                case 0:
                    successful = Attack1(trigger, leftSide);
                    break;
                case 1:
                    successful = Attack2(trigger, leftSide);
                    break;
                case 2:
                    successful = Attack3(trigger, leftSide);
                    break;
                default:
                    ComboCount = 0;
                    successful = Attack1(trigger, leftSide);
                    break;
            }

            if (successful)
            {
                ComboCount++;
                ComboTime = 2f;
                Controller.State = PlayerState.Blocked;
                LockTime = 0.5f / Combat.AttackSpeed;
            }
        }

        private bool Attack1(ITrigger hitbox, bool leftSide)
        {
            if (Combat.Attack(hitbox, 1f, false))
            {
                AnimationSystem.PlayAnimation("Attack1", true, leftSide);
                return true;
            }

            return false;
        }

        private bool Attack2(ITrigger hitbox, bool leftSide)
        {
            if (Combat.Attack(hitbox, 1.5f, false))
            {
                AnimationSystem.PlayAnimation("Attack2", true, leftSide);
                return true;
            }

            return false;
        }

        private bool Attack3(ITrigger hitbox, bool leftSide)
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
