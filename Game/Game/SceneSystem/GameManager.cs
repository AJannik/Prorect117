using System;
using Game.Interfaces;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateable
    {
        public int Coins { get; set; } = 0;

        public void Update(float deltaTime)
        {
        }

        public void GameOver()
        {
            Console.WriteLine("Game Over!");
        }
    }
}