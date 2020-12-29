using System.Collections.Generic;
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
        public static bool VersusTriggerAndCollider(Scene scene, Ray ray, out RaycastHit hit)
        {
            List<RaycastHit> hits = new List<RaycastHit>();
            foreach (ITrigger trigger in scene.GetTriggers())
            {
                if (trigger.MyGameObject.Active)
                {
                    hit = new RaycastHit();
                    if (RaycastCollisionCheck.HandleRaycastCollision((IReadonlySimpleGeometry)trigger.Geometry, ray, hit))
                    {
                        hit.HitObject = trigger.MyGameObject;
                        hits.Add(hit);
                    }
                }
            }

            foreach (ICollider collider in scene.GetColliders())
            {
                if (collider.MyGameObject.Active)
                {
                    hit = new RaycastHit();
                    if (RaycastCollisionCheck.HandleRaycastCollision((IReadonlySimpleGeometry)collider.Geometry, ray, hit))
                    {
                        hit.HitObject = collider.MyGameObject;
                        hits.Add(hit);
                    }
                }
            }

            if (hits.Count > 0)
            {
                SortHits(hits);
                hit = hits[0];
                return true;
            }

            hit = null;
            return false;
        }

        public static bool VersusTrigger(Scene scene, Ray ray, out RaycastHit hit)
        {
            List<RaycastHit> hits = new List<RaycastHit>();
            foreach (ITrigger trigger in scene.GetTriggers())
            {
                if (trigger.MyGameObject.Active)
                {
                    hit = new RaycastHit();
                    if (RaycastCollisionCheck.HandleRaycastCollision((IReadonlySimpleGeometry)trigger.Geometry, ray, hit))
                    {
                        hit.HitObject = trigger.MyGameObject;
                        hits.Add(hit);
                    }
                }
            }

            if (hits.Count > 0)
            {
                SortHits(hits);
                hit = hits[0];
                return true;
            }

            hit = null;
            return false;
        }

        public static bool VersusCollider(Scene scene, Ray ray, out RaycastHit hit)
        {
            List<RaycastHit> hits = new List<RaycastHit>();
            foreach (ICollider collider in scene.GetColliders())
            {
                if (collider.MyGameObject.Active)
                {
                    hit = new RaycastHit();
                    if (RaycastCollisionCheck.HandleRaycastCollision((IReadonlySimpleGeometry)collider.Geometry, ray, hit))
                    {
                        hit.HitObject = collider.MyGameObject;
                        hits.Add(hit);
                    }
                }
            }

            if (hits.Count > 0)
            {
                SortHits(hits);
                hit = hits[0];
                return true;
            }

            hit = null;
            return false;
        }

        private static void SortHits(List<RaycastHit> hits)
        {
            hits.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }
    }
}