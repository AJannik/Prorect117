using System;
using Game.Components;
using Game.Tools;
using OpenTK;

namespace Game
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            var window = new GameWindow(512, 512);
            window.VSync = VSyncMode.On;
            SceneManager sceneManager = new SceneManager(2);
            float counter = 7f;
            int skipedFrames = 0;

            sceneManager.SetScene(0, BuildScene1());
            sceneManager.SetScene(1, BuildScene2());
            GameObject quad = sceneManager.GetScene(0).GetGameObjects()[0];
            GameObject parentQuad = sceneManager.GetScene(1).GetGameObjects()[0];

            void Update(float deltaTime)
            {
                // Skip frame if deltaTime is to big because an untypically big deltaTime can cause physic bugs
                if (deltaTime > 0.03f)
                {
                    skipedFrames++;
                    return;
                }

                sceneManager.Update(deltaTime);

                // TODO: Remove this, it's only for testing
                if (sceneManager.CurrentScene == 0)
                {
                    counter -= deltaTime;
                    if (counter <= 0f)
                    {
                        sceneManager.LoadNextScene();
                        counter = 10f;
                    }
                }
                else
                {
                    counter -= deltaTime;
                    if (counter <= 0f)
                    {
                        Console.WriteLine($"A total of {skipedFrames} Update-Frames were skiped.");
                        window.Exit();
                        return;
                    }
                }

                // TODO: Remove this, it's only for testing
                if (sceneManager.CurrentScene == 0)
                {
                }
                else if (sceneManager.CurrentScene == 1)
                {
                    parentQuad.Transform.Rotation += deltaTime * 0.5f;
                }
            }

            void Draw()
            {
                sceneManager.Draw();
                window.SwapBuffers(); // buffer swap needed for double buffering
            }

            window.UpdateFrame += (_, args) => Update((float)args.Time);
            window.RenderFrame += (s, a) => Draw();
            window.Resize += (_, args) => sceneManager.Resize(window.Width, window.Height);
            window.Run();
        }

        private static Scene BuildScene1()
        {
            Scene scene = new Scene();
            GameObject quad = new GameObject(scene);
            GameObject floor = new GameObject(scene);

            quad.AddComponent<CCamera>();
            quad.AddComponent<CRender>();
            quad.AddComponent<CBoxCollider>();
            quad.AddComponent<CRigidBody>();
            quad.AddComponent<CBoxCollider>();
            quad.AddComponent<CTriggerEventTest>();
            quad.Transform.Position = new Vector2(0f, 1f);

            CBoxCollider trigger = quad.GetComponents<CBoxCollider>()[1];
            trigger.IsTrigger = true;
            trigger.Offset = new Vector2(0f, -0.05f);
            trigger.Geometry.Size = new Vector2(0.4f, 0.1f);

            CRigidBody rb = quad.GetComponent<CRigidBody>();
            rb.UseGravity = true;
            rb.Mass = 0.1f;
            rb.AddForce(Vector2.UnitX);

            floor.AddComponent<CRender>();
            floor.AddComponent<CBoxCollider>();
            floor.AddComponent<CRigidBody>();
            floor.GetComponent<CRigidBody>().UseGravity = false;
            floor.GetComponent<CRigidBody>().Static = true;
            floor.Transform.Position = new Vector2(0f, -0.8f);

            // floor.GetComponent<CBoxCollider>().Geometry.Size = new Vector2(2f, 0.2f);
            return scene;
        }

        private static Scene BuildScene2()
        {
            Scene scene = new Scene();
            GameObject parentQuad = new GameObject(scene);
            GameObject childQuad = new GameObject(scene, parentQuad);

            parentQuad.AddComponent<CRender>();
            childQuad.AddComponent<CRender>();
            childQuad.Transform.Position = new Vector2(0.5f, 0f);

            return scene;
        }
    }
}