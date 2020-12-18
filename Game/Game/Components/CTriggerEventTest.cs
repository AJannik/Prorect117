using Game.Interfaces;
using Game.Physics.RaycastSystem;
using OpenTK;

namespace Game.Components
{
    public class CTriggerEventTest : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        private CBoxCollider Trigger { get; set; } = null;

        public void Update(float deltaTime)
        {
            if (false && Trigger == null)
            {
                Trigger = MyGameObject.GetComponents<CBoxCollider>()[1];
                Trigger.TriggerEntered += OnTriggerEntered;
                Trigger.TriggerExited += OnTriggerExited;
            }

            Ray ray = new Ray(MyGameObject.Transform.WorldPosition - (Vector2.UnitY * 0.3f), -Vector2.UnitY, 0.5f);
            RaycastHit hit;
            if (Raycast.BasicRaycast(MyGameObject.Scene, false, ray, out hit))
            {
                ray.Color = Color.Red;
                ray.Length = (ray.StartPos - hit.HitPoint).Length;
                Ray normal = new Ray(hit.HitPoint, hit.ObjectNormal, 0.2f);
                normal.Color = Color.Blue;
                MyGameObject.Scene.Debug.DrawRay(normal);
            }
            else
            {
                ray.Color = Color.Green;
            }

            MyGameObject.Scene.Debug.DrawRay(ray);
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