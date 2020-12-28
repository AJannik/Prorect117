﻿using Game.Components;
using Game.SceneSystem;
using OpenTK;

namespace Game.GameObjectFactory
{
    public static class StaticRigidbodyFactory
    {
        public static GameObject BuildPlatform(Scene scene, Vector2 position, int length)
        {
            GameObject floor = new GameObject(scene, "Floor");
            Vector2 size = new Vector2(length, 0.2f);
            floor.Transform.Position = position;
            floor.Transform.Scale = size;

            floor.AddComponent<CRender>();
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
            Vector2 size = new Vector2(0.2f, height);
            wall.Transform.Position = position;
            wall.Transform.Scale = size;

            wall.AddComponent<CRender>();
            wall.AddComponent<CBoxCollider>();
            wall.GetComponent<CBoxCollider>().Geometry.Size = size;

            wall.AddComponent<CRigidBody>();
            CRigidBody rb = wall.GetComponent<CRigidBody>();
            rb.Static = true;

            return wall;
        }
    }
}