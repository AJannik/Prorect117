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
            floor.AddComponent<CTileRenderer>();
            floor.GetComponents<CTileRenderer>()[1].Height = 2;
            floor.GetComponents<CTileRenderer>()[1].Width = length + 1;
            floor.GetComponents<CTileRenderer>()[1].LoadAndSetTexture("Content.GrassSpritesheet.png");
            floor.GetComponents<CTileRenderer>()[1].Layer = 11;
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

        public static GameObject BuildLevelEnd(Scene scene, Vector2 position, Vector2 size)
        {
            GameObject levelEnd = new GameObject(scene, "LevelEnd");
            levelEnd.Transform.Position = position;

            levelEnd.AddComponent<CRender>();
            CRender render = levelEnd.GetComponent<CRender>();
            render.LoadAndSetTexture("Content.DoorSprite.png");
            render.SetSize(1f, 2f);
            render.SetTexCoords(new SimpleGeometry.Rect(0f, 0f, 0.5f, 1.0f));

            levelEnd.AddComponent<CBoxTrigger>();
            CBoxTrigger trigger = levelEnd.GetComponent<CBoxTrigger>();
            trigger.Geometry.Size = size;
            levelEnd.AddComponent<CDoor>();
            CDoor door = levelEnd.GetComponent<CDoor>();
            door.SetupTrigger(trigger);

            levelEnd.AddComponent<CTextRender>();
            levelEnd.GetComponent<CTextRender>().Text = "E";
            levelEnd.GetComponent<CTextRender>().Offset = new Vector2(0f, 1.5f);
            levelEnd.GetComponent<CTextRender>().Visible = false;
            levelEnd.GetComponent<CTextRender>().Centered = true;
            door.TextRender = levelEnd.GetComponent<CTextRender>();

            return levelEnd;
        }
    }
}