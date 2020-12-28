namespace Game.Interfaces
{
    public interface IPhysicsComponent
    {
        public GameObject MyGameObject { get; set; }

        public void FixedUpdate(float deltaTime);
    }
}