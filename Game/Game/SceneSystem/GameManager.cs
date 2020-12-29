using Game.Interfaces;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateAble
    {
        public int Coins { get; set; } = 0;

        // TODO: add shop scene
        public void Update(float deltaTime)
        {
        }
    }
}