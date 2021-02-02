using Game.Entity;
using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;
using OpenTK;

namespace Game.Components.Actor
{
    public class CResetController : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public Vector2 PreviousPos { get; set; }

        public float Damage { get; set; }

        public void SetupTrigger(ITrigger trigger)
        {
            trigger.TriggerEntered += Triggered;
        }

        private void Triggered(object sender, IComponent e)
        {
            if (e.MyGameObject.Name == "Player")
            {
                e.MyGameObject.Transform.Position = PreviousPos;
                e.MyGameObject.GetComponent<IActor>().ActorStateBehavior.TakeDamage(Damage, true);
            }
        }
    }
}