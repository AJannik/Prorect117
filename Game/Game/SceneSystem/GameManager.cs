using Game.Interfaces;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateAble
    {
        public int Coins { get; set; } = 0;

        public void Update(float deltaTime)
        {
        }
    }
}