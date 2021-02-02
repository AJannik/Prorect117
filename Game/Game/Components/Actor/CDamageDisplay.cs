using Game.Components.Renderer;
using Game.Entity;
using Game.Interfaces;
using OpenTK;

namespace Game.Components.Actor
{
    public class CDamageDisplay : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; }

        public CTextRender DamageText { get; set; }

        private float DisplayTime { get; set; } = 0.7f;

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

        public void DisplayDamage(string damage, Color color)
        {
            DamageText.FontColor = color;
            DamageText.Text = damage;

            DamageText.Visible = true;
            CurrentTime = DisplayTime;
        }
    }
}