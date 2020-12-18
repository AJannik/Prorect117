using Game.Components;
using Game.SceneLevel;
using Game.SimpleGeometry;

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

            foreach (CBoxCollider boxCollider in scene.GetCBoxColliders())
            {
                if (querryTrigger || (!querryTrigger && !boxCollider.IsTrigger))
                {
                    if (RaycastCollisionCheck.AabbAndLine((Rect)boxCollider.Geometry, ray, hit))
                    {
                        hit.HitObject = boxCollider;
                        return true;
                    }
                }
            }

            foreach (CCircleCollider circleCollider in scene.GetCCircleColliders())
            {
                if (querryTrigger || (!querryTrigger && !circleCollider.IsTrigger))
                {
                    if (RaycastCollisionCheck.CircleAndLine((Circle)circleCollider.Geometry, ray, hit))
                    {
                        hit.HitObject = circleCollider;
                        return true;
                    }
                }
            }

            hit = null;
            return false;
        }
    }
}