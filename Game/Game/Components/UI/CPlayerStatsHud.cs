using Game.Components.Combat;
using Game.Components.Player;
using Game.Components.UI.BaseComponents;
using Game.Interfaces;

namespace Game.Components.UI
{
    public class CPlayerStatsHud : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; }

        public CGuiTextRender HpText { get; set; }

        public CGuiTextRender AttackText { get; set; }

        public CGuiTextRender ArmorText { get; set; }

        public CGuiTextRender SpeedText { get; set; }

        public CCombat Combat { get; set; }

        public CPlayerController PlayerController { get; set; }

        public void Update(float deltaTime)
        {
            HpText.Text = $"{(int)Combat.CurrentHealth}/{(int)Combat.MaxHealth}";
            AttackText.Text = $"Damage: {(int)Combat.AttackDamage}";
            ArmorText.Text = $"Armor: {(int)Combat.Armor}";
            SpeedText.Text = $"Speed: {(int)PlayerController.PlayerSpeed}";
        }
    }
}