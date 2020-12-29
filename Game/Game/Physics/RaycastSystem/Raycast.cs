using Game.Interfaces;
using Game.SceneSystem;

namespace Game.Physics.RaycastSystem
{
    public static class Raycast
    {
        /// <summary>
        /// Performs a normal raycast. If it hits a collider then true is returned and the hit information is stored inside the RaycastHit object.
        /// </summary>
        /// <returns><see langword="true"/> if there was a hit and <see langword="false"/> if there was no hit.</returns>
        public static bool BasicRaycast(Scene scene, bool querryTrigger, Ray ray, out RaycastHit hit)
        {
            hit = new RaycastHit();

            foreach (ICollider collider in scene.GetColliders())
            {
                if (collider.MyGameObject.Active)
                {
                    if (RaycastCollisionCheck.HandleRaycastCollision((IReadonlySimpleGeometry)collider.Geometry, ray, hit))
                    {
                        hit.HitObject = collider.MyGameObject;
                        return true;
                    }
                }
            }

            if (querryTrigger)
            {
                foreach (ITrigger trigger in scene.GetTriggers())
                {
                    if (trigger.MyGameObject.Active)
                    {
                        if (RaycastCollisionCheck.HandleRaycastCollision((IReadonlySimpleGeometry)trigger.Geometry, ray, hit))
                        {
                            hit.HitObject = trigger.MyGameObject;
                            return true;
                        }
                    }
                }
            }

            hit = null;
            return false;
        }
    }
}