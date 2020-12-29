using System;
using System.Collections.Generic;
using Game.Interfaces;
using OpenTK.Graphics.OpenGL;

namespace Game.SceneSystem
{
    public class Scene
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        private List<IDrawable> drawables = new List<IDrawable>();
        private List<IUpdateable> updateables = new List<IUpdateable>();
        private List<IPhysicsComponent> physicsComponents = new List<IPhysicsComponent>();
        private List<IDebugDrawable> debugDrawables = new List<IDebugDrawable>();
        private List<IResizeable> resizeables = new List<IResizeable>();
        private List<ICollider> colliders = new List<ICollider>();

        private List<GameObject> deleteList = new List<GameObject>();

        public event EventHandler<int> LoadLevelNumber;

        public Debug Debug { get; } = new Debug();

        public void LoadLevelEvent(int num)
        {
            LoadLevelNumber?.Invoke(this, num);
        }

        public void Update(float deltaTime)
        {
            SortRenderers();
            foreach (IUpdateable updateAble in updateables)
            {
                if ((updateAble as IComponent).MyGameObject.Active)
                {
                    updateAble.Update(deltaTime);
                }
                else
                {
                    updateAble.Update(deltaTime);
                }
            }

            DeleteGameObjects();
        }

        public void FixedUpdate(float deltaTime)
        {
            foreach (IPhysicsComponent physicsComponent in physicsComponents)
            {
                if ((physicsComponent as IComponent).MyGameObject.Active)
                {
                    physicsComponent.FixedUpdate(deltaTime);
                }
            }
        }

        public void Resize(int width, int height)
        {
            foreach (IResizeable resizeable in resizeables)
            {
                if ((resizeable as IComponent).MyGameObject.Active)
                {
                    resizeable.Resize(width, height);
                }
            }
        }

        public void Draw(float alpha, bool debugMode)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            foreach (IDrawable drawable in drawables)
            {
                if ((drawable as IComponent).MyGameObject.Active)
                {
                    drawable.Draw(alpha);
                }
            }

            if (debugMode)
            {
                DebugDraw(alpha);
            }
        }

        public void DebugDraw(float alpha)
        {
            foreach (IDebugDrawable item in debugDrawables)
            {
                if ((item as IComponent).MyGameObject.Active)
                {
                    item.DebugDraw();
                }
            }

            Debug.DebugDraw();
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
            deleteList.Add(gameObject);
        }

        public IReadOnlyList<GameObject> GetGameObjects()
        {
            return gameObjects;
        }

        public IReadOnlyList<ICollider> GetColliders()
        {
            return colliders;
        }

        public IReadOnlyList<IUpdateable> GetUpdateables()
        {
            return updateables;
        }

        public void AddComponent(IComponent component)
        {
            if (component is IPhysicsComponent)
            {
                if (!physicsComponents.Contains((IPhysicsComponent)component))
                {
                    physicsComponents.Add((IPhysicsComponent)component);
                }
            }

            if (component is IDrawable)
            {
                if (!drawables.Contains((IDrawable)component))
                {
                    int index = GetIndex((IDrawable)component);
                    drawables.Insert(index, (IDrawable)component);
                }
            }

            if (component is IUpdateable)
            {
                if (!updateables.Contains((IUpdateable)component))
                {
                    updateables.Add((IUpdateable)component);
                }
            }

            if (component is IResizeable)
            {
                if (!resizeables.Contains((IResizeable)component))
                {
                    resizeables.Add((IResizeable)component);
                }
            }

            if (component is IDebugDrawable)
            {
                if (!debugDrawables.Contains((IDebugDrawable)component))
                {
                    debugDrawables.Add((IDebugDrawable)component);
                }
            }

            if (component is ICollider)
            {
                if (!colliders.Contains((ICollider)component))
                {
                    colliders.Add((ICollider)component);
                }
            }
        }

        public void RemoveComponent(IComponent component)
        {
            if (component is IPhysicsComponent)
            {
                physicsComponents.Remove((IPhysicsComponent)component);
            }

            if (component is IDrawable)
            {
                drawables.Remove((IDrawable)component);
            }

            if (component is IUpdateable)
            {
                updateables.Remove((IUpdateable)component);
            }

            if (component is IResizeable)
            {
                resizeables.Remove((IResizeable)component);
            }

            if (component is IDebugDrawable)
            {
                debugDrawables.Remove((IDebugDrawable)component);
            }

            if (component is ICollider)
            {
                colliders.Remove((ICollider)component);
            }
        }

        private void DeleteGameObjects()
        {
            foreach (GameObject gameObject in deleteList)
            {
                gameObjects.Remove(gameObject);
                RemoveComponentsFromLists(gameObject);
            }

            deleteList.Clear();
        }

        private void SortRenderers()
        {
            drawables.Sort((x, y) => x.Layer.CompareTo(y.Layer));
        }

        private void AddComponentsToLists(GameObject gameObject)
        {
            IComponent[] components = gameObject.GetAllComponents();

            foreach (IComponent component in components)
            {
                AddComponent(component);
            }
        }

        private int GetIndex(IDrawable drawable)
        {
            if (drawables.Count == 0)
            {
                return 0;
            }

            for (int i = 1; i < drawables.Count; i++)
            {
                if (drawables[i].Layer >= drawable.Layer)
                {
                    return i - 1;
                }
            }

            return drawables.Count;
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