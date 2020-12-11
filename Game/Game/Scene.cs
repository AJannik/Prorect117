using System;
using System.Collections.Generic;
using Game.Components;
using Game.Interfaces;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    public class Scene
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        private List<CRender> renderers = new List<CRender>();
        private List<CCamera> cameras = new List<CCamera>();
        private List<CBoxCollider> boxColliders = new List<CBoxCollider>();
        private List<IComponent> genericComponents = new List<IComponent>();

        public void Update(float deltaTime)
        {
            foreach (CBoxCollider boxCollider in boxColliders)
            {
                boxCollider.Update(deltaTime);
            }

            foreach (CCamera cCamera in cameras)
            {
                cCamera.Update(deltaTime);
            }

            SortRenderers();
            foreach (CRender cRender in renderers)
            {
                cRender.Update(deltaTime);
            }

            foreach (IComponent component in genericComponents)
            {
                component.Update(deltaTime);
            }
        }

        public void Draw(bool debugMode)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            foreach (CRender render in renderers)
            {
                render.Draw();
            }

            if (debugMode)
            {
                foreach (CBoxCollider boxCollider in boxColliders)
                {
                    boxCollider.DebugDraw();
                }
            }
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (!gameObjects.Contains(gameObject))
            {
                gameObjects.Add(gameObject);
                gameObject.Scene = this;
                AddComponentsToLists(gameObject);
            }
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
            RemoveComponentsFromLists(gameObject);
        }

        public List<GameObject> GetGameObjects()
        {
            return gameObjects;
        }

        public List<CRender> GetCRenders()
        {
            return renderers;
        }

        public List<CBoxCollider> GetCBoxColliders()
        {
            return boxColliders;
        }

        public List<IComponent> GetGenericComponents()
        {
            return genericComponents;
        }

        public void AddComponent(IComponent component)
        {
            if (component.GetType() == typeof(CRender))
            {
                if (!renderers.Contains((CRender)component))
                {
                    int index = GetIndex((CRender)component);
                    renderers.Insert(index, (CRender)component);
                }
            }
            else if (component.GetType() == typeof(CCamera))
            {
                if (!cameras.Contains((CCamera)component))
                {
                    cameras.Add((CCamera)component);
                }
            }
            else if (component.GetType() == typeof(CBoxCollider))
            {
                if (!boxColliders.Contains((CBoxCollider)component))
                {
                    boxColliders.Add((CBoxCollider)component);
                }
            }
            else
            {
                if (!genericComponents.Contains(component))
                {
                    genericComponents.Add(component);
                }
            }
        }

        public void RemoveComponent(IComponent component)
        {
            if (component.GetType() == typeof(CRender))
            {
                renderers.Remove((CRender)component);
            }
            else if (component.GetType() == typeof(CCamera))
            {
                cameras.Remove((CCamera)component);
            }
            else if (component.GetType() == typeof(CBoxCollider))
            {
                boxColliders.Remove((CBoxCollider)component);
            }
            else
            {
                genericComponents.Remove(component);
            }
        }

        private void SortRenderers()
        {
            renderers.Sort((x, y) => x.Layer.CompareTo(y.Layer));
        }

        private void AddComponentsToLists(GameObject gameObject)
        {
            IComponent[] components = gameObject.GetAllComponents();

            foreach (IComponent component in components)
            {
                AddComponent(component);
            }
        }

        private int GetIndex(CRender cRender)
        {
            if (renderers.Count == 0)
            {
                return 0;
            }

            for (int i = 1; i < renderers.Count; i++)
            {
                if (renderers[i].Layer >= cRender.Layer)
                {
                    return i - 1;
                }
            }

            return renderers.Count;
        }

        private void RemoveComponentsFromLists(GameObject gameObject)
        {
            IComponent[] components = gameObject.GetAllComponents();

            foreach (IComponent component in components)
            {
                RemoveComponent(component);
            }
        }
    }
}