using Game.Interfaces;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateable
    {
        public int Coins { get; set; } = 0;

        // TODO: add shop scene
        public void Update(float deltaTime)
        {
        }
    }
}