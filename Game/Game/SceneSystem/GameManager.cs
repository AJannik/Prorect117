using System;
using System.Collections.Generic;
using Game.Interfaces;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateable
    {
        // Stay at level-change
        public int Coins { get; set; }

        // If the player has collected a key in this level; Needs to be reset at level-change
        public bool Key { get; set; } = false;

        // TODO: Add list of power-downs that the player has collected

        public void Update(float deltaTime)
        {
        }

        public void GameOver()
        {
            Console.WriteLine("Game Over!");
        }
    }
}