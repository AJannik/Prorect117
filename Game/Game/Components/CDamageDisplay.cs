using Game.Components.Renderer;
using Game.Interfaces;
using OpenTK;

namespace Game.Components
{
    public class CDamageDisplay : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; }

        public CTextRender DamageText { get; set; }

        public float DisplayTime { get; set; } = 0.7f;

        private float CurrentTime { get; set; } = 0f;

        public void Update(float deltaTime)
        {
            if (CurrentTime > 0f)
            {
                CurrentTime -= deltaTime;
            }
            else
            {
                DamageText.Visible = false;
            }
        }

        public void DisplayDamage(int damage)
        {
            if (damage < 0)
            {
                DamageText.FontColor = Color.Red;
                DamageText.Text = $"{damage}";
            }
            else
            {
                DamageText.FontColor = Color.Green;
                DamageText.Text = $"+{damage}";
            }

            DamageText.Visible = true;
            CurrentTime = DisplayTime;
        }
    }
}