using Game.Interfaces;

namespace Game.SceneSystem
{
    public class GameManager : IUpdateAble
    {
        public GameManager(SceneManager sceneManager)
        {
            SceneManager = sceneManager;
        }

        public int Coins { get; set; } = 0;

        public SceneManager SceneManager { get; }

        // TODO: Add Scene.LoadScene event Listener

        public void Update(float deltaTime)
        {
        }
    }
}