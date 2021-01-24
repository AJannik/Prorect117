using Game.Components.Renderer;
using Game.Interfaces;
using Game.Tools;

namespace Game.Components.Player
{
    public class CPickupDisplay : IComponent, IOnStart, IUpdateable
    {
        public CTextRender TextRenderCoin { get; set; }

        public CRender RenderCoin { get; set; }

        public CTextRender TextRenderKey { get; set; }

        public CRender RenderKey { get; set; }

        public GameObject MyGameObject { get; set; }

        private float DisplayTimeCoins { get; } = 1f;

        private float CurrentTimeCoins { get; set; } = 0f;

        private float DisplayTimeKeys { get; } = 1f;

        private float CurrentTimeKeys { get; set; } = 0f;

        public void Start()
        {
            RenderCoin.LoadAndSetTexture("Content.goldcoin1.png");
            RenderKey.LoadAndSetTexture("Content.Key.png");
            RenderKey.Visible = false;
            TextRenderKey.Visible = false;
            RenderCoin.Visible = false;
            TextRenderCoin.Visible = false;
        }

        public void Update(float deltaTime)
        {
            if (CurrentTimeCoins > 0f)
            {
                CurrentTimeCoins -= deltaTime;
            }
            else
            {
                RenderCoin.Visible = false;
                TextRenderCoin.Visible = false;
            }

            if (CurrentTimeKeys > 0f)
            {
                CurrentTimeKeys -= deltaTime;
            }
            else
            {
                RenderKey.Visible = false;
                TextRenderKey.Visible = false;
            }
        }

        public void AddCoins(int num)
        {
            TextRenderCoin.Text = $"+{num}";
            RenderCoin.Visible = true;
            TextRenderCoin.Visible = true;
            CurrentTimeCoins = DisplayTimeCoins;
        }

        public void AddKey()
        {
            TextRenderKey.Text = $"+1";
            RenderKey.Visible = true;
            TextRenderKey.Visible = true;
            CurrentTimeKeys = DisplayTimeKeys;
        }

        public void AddPowerDown(EffectType type)
        {
            TextRenderKey.Text = $"+1x {type}";
            TextRenderKey.Visible = true;
            CurrentTimeKeys = DisplayTimeKeys;
        }
    }
}