using Game.Components;
using Game.SimpleGeometry;

namespace Game.RaycastSystem
{
    public static class Raycast
    {
        public static bool Cast(Scene scene, bool querryTrigger, Ray ray, out RaycastHit hit)
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