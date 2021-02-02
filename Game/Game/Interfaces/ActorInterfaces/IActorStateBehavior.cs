using Game.Components;
using Game.Components.Actor;
using Game.Components.Actor.Displays;
using Game.Components.Renderer;
using Game.Components.Renderer.Animations;
using Game.Entity;
using OpenTK;

namespace Game.Interfaces.ActorInterfaces
{
    public interface IActorStateBehavior
    {
        public CAnimationSystem AnimationSystem { get; set; }

        public CRigidBody RigidBody { get; set; }

        public CParticleSystem BloodParticles { get; set; }

        public CDamageDisplay DamageDisplay { get; set; }

        public CTextRender HpText { get; set; }

        public IActor Actor { get; set; }

        public void Idle();

        public void Running(Vector2 moveSpeed);

        public bool Attacking(string animationName, float damageMultiplier);

        public void Dying();

        public void Dead();

        public void TakeDamage(float dmgAmount, bool ignoreArmor);

        public void SetupPickupDisplay(GameObject myGameObject);

        public void HandleBloodEffect(float deltaTime);
    }
}