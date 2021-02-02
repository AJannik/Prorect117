using System.Diagnostics.CodeAnalysis;
using Game.Components;
using Game.Components.Actor;
using Game.Components.Collision;
using Game.Components.Renderer;
using Game.Entity;
using Game.SceneSystem;
using OpenTK;

namespace Game.GameObjectFactory
{
    [ExcludeFromCodeCoverage]
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

        public static GameObject BuildPlatform(Scene scene, Vector2 position, int length, string tilemapPath)
        {
            GameObject floor = new GameObject(scene, "Floor");
            Vector2 size = new Vector2(length, 1f);
            floor.Transform.Position = position;

            floor.AddComponent<CTileRenderer>();
            floor.GetComponent<CTileRenderer>().Height = 1;
            floor.GetComponent<CTileRenderer>().Width = length;
            floor.GetComponent<CTileRenderer>().LoadAndSetTexture(tilemapPath);
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

        public static GameObject BuildWall(Scene scene, Vector2 position, int height, string tilemapPath)
        {
            GameObject wall = new GameObject(scene, "Wall");
            Vector2 size = new Vector2(1f, height);
            wall.Transform.Position = position;

            wall.AddComponent<CTileRenderer>();
            wall.GetComponent<CTileRenderer>().Height = height;
            wall.GetComponent<CTileRenderer>().Width = 1;
            wall.GetComponent<CTileRenderer>().LoadAndSetTexture(tilemapPath);
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
            CTextRender textHasKey = levelEnd.GetComponent<CTextRender>();
            textHasKey.Text = "E";
            textHasKey.Offset = new Vector2(0f, 1.5f);
            textHasKey.Visible = false;
            textHasKey.Centered = true;
            door.TextRender = levelEnd.GetComponent<CTextRender>();

            return levelEnd;
        }

        public static GameObject BuildDeadlyArea(Scene scene, Vector2 position, Vector2 size, Vector2 lastPosition, float dmg)
        {
            GameObject deadlyArea = new GameObject(scene, "DeadlyArea");
            deadlyArea.Transform.Position = position;

            deadlyArea.AddComponent<CBoxTrigger>();
            CBoxTrigger trigger = deadlyArea.GetComponent<CBoxTrigger>();
            trigger.Geometry.Size = size;
            deadlyArea.AddComponent<CResetController>();
            CResetController controller = deadlyArea.GetComponent<CResetController>();
            controller.PreviousPos = lastPosition;
            controller.SetupTrigger(trigger);
            controller.Damage = dmg;

            return deadlyArea;
        }

        public static GameObject BuildSpikes(Scene scene, Vector2 position, int length)
        {
            GameObject spikes = new GameObject(scene, "Spikes");
            position.X = position.X - (length / 2f);
            spikes.Transform.Position = position;

            for (int i = 0; i < length; i++)
            {
                spikes.AddComponent<CRender>();
                spikes.GetComponents<CRender>()[i].LoadAndSetTexture("Content.Environment.spike.png");
                spikes.GetComponents<CRender>()[i].SetOffset(i * 1f, 0f);
            }

            return spikes;
        }
    }
}