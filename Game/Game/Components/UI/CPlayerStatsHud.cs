using System;
using Game.Components.Actor;
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

        public CGuiTextRender SilencedText { get; set; }

        public CPlayer Player { get; set; }

        private float Timer { get; set; } = 0f;

        private float LastHealth { get; set; }

        private float LastAttackDamage { get; set; }

        private float LastArmor { get; set; }

        private float LastSpeed { get; set; }

        public void Start()
        {
            LastHealth = Player.ActorStats.CurrentHealth;
            LastAttackDamage = Player.ActorStats.AttackDamage;
            LastArmor = Player.ActorStats.Armor;
            LastSpeed = Player.ActorStats.MoveSpeed;
            SilencedText.Text = "CAN ROLL";
            HpText.Text = $"{MathF.Ceiling(Player.ActorStats.CurrentHealth)}/{MathF.Ceiling(Player.ActorStats.MaxHealth)}";
            AttackText.Text = $"Damage: {MathF.Ceiling(Player.ActorStats.AttackDamage)}";
            ArmorText.Text = $"Armor: {MathF.Ceiling(Player.ActorStats.Armor)}";
            SpeedText.Text = $"Speed: {MathF.Ceiling(Player.ActorStats.MoveSpeed)}";
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
                SilencedText.FontColor = Color.White;
            }

            if (Math.Abs(Player.ActorStats.CurrentHealth - LastHealth) > 0.01f)
            {
                ChangeHpText();
            }

            if (Math.Abs(Player.ActorStats.AttackDamage - LastAttackDamage) > 0.01f)
            {
                ChangeAttackText();
            }

            if (Math.Abs(Player.ActorStats.Armor - LastArmor) > 0.01f)
            {
                ChangeArmorText();
            }

            if (Math.Abs(Player.ActorStats.MoveSpeed - LastSpeed) > 0.01f)
            {
                ChangeSpeedText();
            }

            if (!Player.ActorStats.RollEnabled)
            {
                ChangeSilencedText();
            }
        }

        private void ChangeHpText()
        {
            HpText.FontColor = Color.Red;
            HpText.Text = $"{MathF.Ceiling(Player.ActorStats.CurrentHealth)}/{MathF.Ceiling(Player.ActorStats.MaxHealth)}";
            LastHealth = Player.ActorStats.CurrentHealth;
            Timer = 1f;
        }

        private void ChangeAttackText()
        {
            AttackText.FontColor = Color.Red;
            AttackText.Text = $"Damage: {MathF.Ceiling(Player.ActorStats.AttackDamage)}";
            LastAttackDamage = Player.ActorStats.AttackDamage;
            Timer = 1f;
        }

        private void ChangeArmorText()
        {
            ArmorText.FontColor = Color.Red;
            ArmorText.Text = $"Armor: {MathF.Ceiling(Player.ActorStats.Armor)}";
            LastArmor = Player.ActorStats.Armor;
            Timer = 1f;
        }

        private void ChangeSpeedText()
        {
            SpeedText.FontColor = Color.Red;
            SpeedText.Text = $"Speed: {MathF.Ceiling(Player.ActorStats.MoveSpeed)}";
            LastSpeed = Player.ActorStats.MoveSpeed;
            Timer = 1f;
        }

        private void ChangeSilencedText()
        {
            SilencedText.FontColor = Color.Red;
            SilencedText.Text = "CAN NOT ROLL";
            Timer = 1f;
        }
    }
}