using Game.Components;
using Game.Components.Collision;
using Game.Components.Renderer;
using Game.SceneSystem;
using OpenTK;

namespace Game.GameObjectFactory
{
    public static class StaticRigidbodyFactory
    {
        public static GameObject BuildPlatform(Scene scene, Vector2 position, int length)
        {
            GameObject floor = new GameObject(scene, "Floor");
            Vector2 size = new Vector2(length, 1f);
            floor.Transform.Position = position;

            floor.AddComponent<CTileRenderer>();
            floor.GetComponent<CTileRenderer>().Height = 1;
            floor.GetComponent<CTileRenderer>().Width = length;
            floor.AddComponent<CBoxCollider>();
            floor.GetComponent<CBoxCollider>().Geometry.Size = size;

            floor.AddComponent<CRigidBody>();
            CRigidBody rb = floor.GetComponent<CRigidBody>();
            rb.Static = true;

            return floor;
        }

        public static GameObject BuildWall(Scene scene, Vector2 position, int height)
        {
            GameObject wall = new GameObject(scene, "Wall");
            Vector2 size = new Vector2(1f, height);
            wall.Transform.Position = position;

            wall.AddComponent<CTileRenderer>();
            wall.GetComponent<CTileRenderer>().Height = height;
            wall.GetComponent<CTileRenderer>().Width = 1;
            wall.AddComponent<CBoxCollider>();
            wall.GetComponent<CBoxCollider>().Geometry.Size = size;

            wall.AddComponent<CRigidBody>();
            CRigidBody rb = wall.GetComponent<CRigidBody>();
            rb.Static = true;

            return wall;
        }
    }
}