using System;
using System.Collections.Generic;
using Game.Components;

namespace Game
{
    public class Scene
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        private List<CRender> renderers = new List<CRender>();
        private List<CCamera> cameras = new List<CCamera>();

        public void Update(float deltaTime)
        {
            foreach (CCamera cCamera in cameras)
            {
                cCamera.Update(deltaTime);
            }

            SortRenderers();
            foreach (CRender cRender in renderers)
            {
                cRender.Update(deltaTime);
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

            if (component.GetType() == typeof(CCamera))
            {
                if (!cameras.Contains((CCamera)component))
                {
                    cameras.Add((CCamera)component);
                }
            }
        }

        public void RemoveComponent(IComponent component)
        {
            if (component.GetType() == typeof(CRender))
            {
                renderers.Remove((CRender)component);
            }

            if (component.GetType() == typeof(CCamera))
            {
                cameras.Remove((CCamera)component);
            }
        }

        private void SortRenderers()
        {
            renderers.Sort((x, y) => x.Layer.CompareTo(y.Layer));
        }

        private void AddComponentsToLists(GameObject gameObject)
        {
            if (gameObject.GetComponent<CRender>() != null)
            {
                foreach (CRender cRender in gameObject.GetComponents<CRender>())
                {
                    if (!renderers.Contains(cRender))
                    {
                        int index = GetIndex(cRender);
                        renderers.Insert(index, cRender);
                    }
                }
            }

            if (gameObject.GetComponent<CCamera>() != null)
            {
                foreach (CCamera cCamera in gameObject.GetComponents<CCamera>())
                {
                    if (!cameras.Contains(cCamera))
                    {
                        cameras.Add(cCamera);
                    }
                }
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
            if (gameObject.GetComponent<CRender>() != null)
            {
                foreach (CRender cRender in gameObject.GetComponents<CRender>())
                {
                    renderers.Remove(cRender);
                }
            }

            if (gameObject.GetComponent<CCamera>() != null)
            {
                foreach (CCamera cCamera in gameObject.GetComponents<CCamera>())
                {
                    cameras.Remove(cCamera);
                }
            }
        }
    }
}