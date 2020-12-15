using Game.Interfaces;

namespace Game.Components
{
    public class CTriggerEventTest : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        private CBoxCollider Trigger { get; set; } = null;

        public void Update(float deltaTime)
        {
            if (Trigger == null)
            {
                Trigger = MyGameObject.GetComponents<CBoxCollider>()[1];
                Trigger.TriggerEntered += OnTriggerEntered;
                Trigger.TriggerExited += OnTriggerExited;
            }
        }

        private void OnTriggerExited(object sender, ICollider e)
        {
            MyGameObject.GetComponent<CRigidBody>().UseGravity = true;
        }

        private void OnTriggerEntered(object sender, ICollider e)
        {
            MyGameObject.GetComponent<CRigidBody>().UseGravity = false;
        }
    }
}