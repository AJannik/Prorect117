using Game.Components;
using Game.Components.Renderer.Animations;
using Game.Entity;

namespace Game.Interfaces.ActorInterfaces
{
    public interface IActorStateHandler
    {
        public CAnimationSystem AnimationSystem { get; set; }

        public CRigidBody RigidBody { get; set; }

        public IActor Actor { get; set; }

        public void Idle();

        public void Running(float moveSpeed);

        public void Attacking(ITrigger attackTrigger);

        public void Dying();

        public void Dead();

        public void SetupPickupDisplay(GameObject myGameObject);
    }
}