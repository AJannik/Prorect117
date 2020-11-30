using System;
using System.Collections.Generic;
using System.Text;
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
            foreach (CRender cRender in renderers)
            {
                cRender.Update(deltaTime);
            }

            foreach (CCamera cCamera in cameras)
            {
                cCamera.Update(deltaTime);
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

        private void AddComponentsToLists(GameObject gameObject)
        {
            if (gameObject.GetComponent<CRender>() != null)
            {
                foreach (CRender cRender in gameObject.GetComponents<CRender>())
                {
                    if (!renderers.Contains(cRender))
                    {
                        renderers.Add(cRender);
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

        private void RemoveComponentsFromLists(GameObject gameObject)
        {
            if (gameObject.GetComponent<CRender>() != null)
            {
                foreach (CRender cRender in gameObject.GetComponents<CRender>())
                {
                    if (!renderers.Contains(cRender))
                    {
                        renderers.Remove(cRender);
                    }
                }
            }

            if (gameObject.GetComponent<CCamera>() != null)
            {
                foreach (CCamera cCamera in gameObject.GetComponents<CCamera>())
                {
                    if (!cameras.Contains(cCamera))
                    {
                        cameras.Remove(cCamera);
                    }
                }
            }
        }
    }
}