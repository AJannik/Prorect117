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
                if ((collider as IComponent).MyGameObject.Active && (querryTrigger || (!querryTrigger && !collider.IsTrigger)))
                {
                    if (RaycastCollisionCheck.HandleRaycastCollision((IReadonlySimpleGeometry)collider.Geometry, ray, hit))
                    {
                        hit.HitObject = collider;
                        return true;
                    }
                }
            }

            hit = null;
            return false;
        }
    }
}