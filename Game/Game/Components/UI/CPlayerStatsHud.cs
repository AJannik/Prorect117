using System;
using Game.Components.Combat;
using Game.Components.Player;
using Game.Components.UI.BaseComponents;
using Game.Entity;
using Game.Interfaces;
using OpenTK;

namespace Game.Components.UI
{
    public class CPlayerStatsHud : IComponent, IUpdateable, IOnStart
    {
        public GameObject MyGameObject { get; set; }

        public CGuiTextRender HpText { get; set; }

        public CGuiTextRender AttackText { get; set; }

        public CGuiTextRender ArmorText { get; set; }

        public CGuiTextRender SpeedText { get; set; }

        public CCombat Combat { get; set; }

        public CPlayerController PlayerController { get; set; }

        private float Timer { get; set; } = 0f;

        private float LastHealth { get; set; }

        private float LastAttackDamage { get; set; }

        private float LastArmor { get; set; }

        private float LastSpeed { get; set; }

        public void Start()
        {
            LastHealth = Combat.CurrentHealth;
            LastAttackDamage = Combat.AttackDamage;
            LastArmor = Combat.Armor;
            LastSpeed = PlayerController.PlayerSpeed;
            HpText.Text = $"{MathF.Ceiling(Combat.CurrentHealth)}/{MathF.Ceiling(Combat.MaxHealth)}";
            AttackText.Text = $"Damage: {MathF.Ceiling(Combat.AttackDamage)}";
            ArmorText.Text = $"Armor: {MathF.Ceiling(Combat.Armor)}";
            SpeedText.Text = $"Speed: {MathF.Ceiling(PlayerController.PlayerSpeed)}";
        }

        public void Update(float deltaTime)
        {
            if (Timer > 0f)
            {
                Timer -= deltaTime;
            }
            else
            {
                HpText.FontColor = Color.White;
                AttackText.FontColor = Color.White;
                ArmorText.FontColor = Color.White;
                SpeedText.FontColor = Color.White;
            }

            if (Combat.CurrentHealth != LastHealth)
            {
                ChangeHpText();
            }

            if (Combat.AttackDamage != LastAttackDamage)
            {
                ChangeAttackText();
            }

            if (Combat.Armor != LastArmor)
            {
                ChangeArmorText();
            }

            if (PlayerController.PlayerSpeed != LastSpeed)
            {
                ChangeSpeedText();
            }
        }

        private void ChangeHpText()
        {
            HpText.FontColor = Color.Red;
            HpText.Text = $"{MathF.Ceiling(Combat.CurrentHealth)}/{MathF.Ceiling(Combat.MaxHealth)}";
            LastHealth = Combat.CurrentHealth;
            Timer = 1f;
        }

        private void ChangeAttackText()
        {
            AttackText.FontColor = Color.Red;
            AttackText.Text = $"Damage: {MathF.Ceiling(Combat.AttackDamage)}";
            LastAttackDamage = Combat.AttackDamage;
            Timer = 1f;
        }

        private void ChangeArmorText()
        {
            ArmorText.FontColor = Color.Red;
            ArmorText.Text = $"Armor: {MathF.Ceiling(Combat.Armor)}";
            LastArmor = Combat.Armor;
            Timer = 1f;
        }

        private void ChangeSpeedText()
        {
            SpeedText.FontColor = Color.Red;
            SpeedText.Text = $"Speed: {MathF.Ceiling(PlayerController.PlayerSpeed)}";
            LastSpeed = PlayerController.PlayerSpeed;
            Timer = 1f;
        }
    }
}