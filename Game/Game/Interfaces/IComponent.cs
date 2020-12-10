namespace Game.Interfaces
{
    public interface IComponent
    {
        public GameObject MyGameObject { get; set; }

        void Update(float deltaTime);
    }
}