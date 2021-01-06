using System;
using System.Collections;
using System.Collections.Generic;
using Game.Components;
using Game.Interfaces;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateable
    {
        public event EventHandler<int> GameOverEvent;

        // Stay at level-change
        public int Coins { get; set; }

        public float PlayerHealth { get; set; } = 100f;

        // If the player has collected a key in this level
        public bool Key { get; set; } = false;

        public bool PlayerInvulnerable { get; set; } = false;

        public List<CPowerDownScript> PowerDowns { get; set; }

        public void Update(float deltaTime)
        {
        }

        public void GameOver()
        {
            GameOverEvent?.Invoke(this, 0);
        }
    }
}