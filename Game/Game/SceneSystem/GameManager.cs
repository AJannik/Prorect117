using System;
using System.Collections.Generic;
using System.Linq;
using Game.Interfaces;
using Game.Tools;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateable
    {
        public event EventHandler<int> EndGameEvent;

        public event EventHandler<int> RestartGameEvent;

        // Stay at level-change
        public int Coins { get; set; }

        public float PlayerHealth { get; set; } = 100f;

        // If the player has collected a key in this level
        public bool Key { get; set; } = false;

        public bool PlayerWon { get; set; } = false;

        public List<Effect> Effects { get; private set; } = new List<Effect>();

        public void Update(float deltaTime)
        {
        }

        public void EndGame()
        {
            EndGameEvent?.Invoke(this, 0);
        }

        public void Restart()
        {
            Coins = 0;
            PlayerHealth = 100f;
            Key = false;
            PlayerWon = false;
            Effects = new List<Effect>();
            RestartGameEvent?.Invoke(this, 0);
        }

        public int NumEffectTypeInEffects(EffectType type)
        {
            return Effects.Count(effect => effect.Type == type);
        }

        public void RemoveEffectOfType(EffectType type)
        {
            Effect delete = null;
            foreach (Effect effect in Effects)
            {
                if (effect.Type == type)
                {
                    delete = effect;
                    break;
                }
            }

            Effects.Remove(delete);
        }
    }
}