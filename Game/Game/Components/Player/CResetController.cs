using System;
using Game.Interfaces;
using OpenTK;

namespace Game.Components.Player
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
            Console.WriteLine("Triggered by " + e.MyGameObject.Name);
            if (e.MyGameObject.Name == "Player")
            {
                e.MyGameObject.Transform.Position = PreviousPos;
                e.MyGameObject.GetComponent<CCombat>().TakeDamage(Damage, true, e.MyGameObject.GetComponent<CCombat>().HurtAnimationName);
            }
        }
    }
}