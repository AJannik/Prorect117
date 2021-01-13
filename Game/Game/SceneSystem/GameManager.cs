using System;
using System.Collections;
using System.Collections.Generic;
using Game.Components;
using Game.Interfaces;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateable
    {
        public event EventHandler<int> EndGameEvent;

        // Stay at level-change
        public int Coins { get; set; }

        public float PlayerHealth { get; set; } = 100f;

        // If the player has collected a key in this level
        public bool Key { get; set; } = false;

        public bool PlayerWon { get; set; } = false;

        public List<CPowerDownScript> PowerDowns { get; set; }

        public void Update(float deltaTime)
        {
        }

        public void EndGame()
        {
            EndGameEvent?.Invoke(this, 0);
        }
    }
}