using System;
using System.Collections.Generic;
using Game.Interfaces;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateable
    {
        // Stay at level-change
        public int Coins { get; set; }

        public float PlayerHealth { get; set; } = 100f;

        // If the player has collected a key in this level
        public bool Key { get; set; } = false;

        // TODO: Add list of power-downs that the player has collected

        public void Update(float deltaTime)
        {
            if (PlayerHealth <= 0f)
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            Console.WriteLine("Game Over!");
        }
    }
}