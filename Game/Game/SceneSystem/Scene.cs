using System;
using System.Collections.Generic;
using Game.Entity;
using Game.Interfaces;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game.SceneSystem
{
    public class Scene
    {
        private readonly List<GameObject> gameObjects = new List<GameObject>();
        private readonly List<IDrawable> drawables = new List<IDrawable>();
        private readonly List<IUpdateable> updatables = new List<IUpdateable>();
        private readonly List<IPhysicsComponent> physicsComponents = new List<IPhysicsComponent>();
        private readonly List<IDebugDrawable> debugDrawables = new List<IDebugDrawable>();
        private readonly List<IResizeable> resizables = new List<IResizeable>();
        private readonly List<ICollider> colliders = new List<ICollider>();
        private readonly List<ITrigger> triggers = new List<ITrigger>();
        private readonly List<IMouseListener> mouseListeners = new List<IMouseListener>();
        private readonly List<IOnStart> onStarts = new List<IOnStart>();

        private readonly List<GameObject> deleteList = new List<GameObject>();

        public Scene(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        public event EventHandler<int> LoadLevelNumber;

        public event EventHandler<int> ExitEvent;

        public GameManager GameManager { get; }

        private Debug Debug { get; } = new Debug();

        public void InvokeLoadLevelEvent(int num)
        {
            LoadLevelNumber?.Invoke(this, num);
            GameManager.Key = false;
        }

        public void Start()
        {
            foreach (IOnStart start in onStarts)
            {
                start.Start();
            }
        }

        public void Update(float deltaTime)
        {
            SortRenderers();
            foreach (IUpdateable updateAble in updatables)
            {
                if (((IComponent)updateAble).MyGameObject.Active)
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
                if (((IComponent)physicsComponent).MyGameObject.Active)
                {
                    physicsComponent.FixedUpdate(deltaTime);
                }
            }
        }

        public void Resize(int width, int height)
        {
            foreach (IResizeable resizeable in resizables)
            {
                if (((IComponent)resizeable).MyGameObject.Active)
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
                if (((IComponent)drawable).MyGameObject.Active)
                {
                    drawable.Draw(alpha);
                }
            }

            if (debugMode)
            {
                DebugDraw();
            }
        }

        public void MouseEvent(MouseButtonEventArgs args)
        {
            foreach (IMouseListener mouseListener in mouseListeners)
            {
                if (((IComponent)mouseListener).MyGameObject.Active)
                {
                    mouseListener.MouseEvent(args);
                }
            }
        }

        public void ExitGame()
        {
            ExitEvent?.Invoke(this, 0);
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

        public IReadOnlyList<IUpdateable> GetUpdatables()
        {
            return updatables;
        }

        public IReadOnlyList<ITrigger> GetTriggers()
        {
            return triggers;
        }

        public void AddComponent(IComponent component)
        {
            if (component is IPhysicsComponent physicsComponent)
            {
                if (!physicsComponents.Contains(physicsComponent))
                {
                    physicsComponents.Add(physicsComponent);
                }
            }

            if (component is IDrawable drawable1)
            {
                if (!drawables.Contains(drawable1))
                {
                    int index = GetIndex(drawable1);
                    drawables.Insert(index, drawable1);
                }
            }

            if (component is IUpdateable updateable)
            {
                if (!updatables.Contains(updateable))
                {
                    updatables.Add(updateable);
                }
            }

            if (component is IResizeable resizeable)
            {
                if (!resizables.Contains(resizeable))
                {
                    resizables.Add(resizeable);
                }
            }

            if (component is IDebugDrawable drawable)
            {
                if (!debugDrawables.Contains(drawable))
                {
                    debugDrawables.Add(drawable);
                }
            }

            if (component is ICollider collider)
            {
                if (!colliders.Contains(collider))
                {
                    colliders.Add(collider);
                }
            }

            if (component is ITrigger trigger)
            {
                if (!triggers.Contains(trigger))
                {
                    triggers.Add(trigger);
                }
            }

            if (component is IMouseListener listener)
            {
                if (!mouseListeners.Contains(listener))
                {
                    mouseListeners.Add(listener);
                }
            }

            if (component is IOnStart start)
            {
                if (!onStarts.Contains(start))
                {
                    onStarts.Add(start);
                }
            }
        }

        public void RemoveComponent(IComponent component)
        {
            if (component is IPhysicsComponent physicsComponent)
            {
                physicsComponents.Remove(physicsComponent);
            }

            if (component is IDrawable drawable)
            {
                drawables.Remove(drawable);
            }

            if (component is IUpdateable updateable)
            {
                updatables.Remove(updateable);
            }

            if (component is IResizeable resizeable)
            {
                resizables.Remove(resizeable);
            }

            if (component is IDebugDrawable debugDrawable)
            {
                debugDrawables.Remove(debugDrawable);
            }

            if (component is ICollider collider)
            {
                colliders.Remove(collider);
            }

            if (component is ITrigger trigger)
            {
                triggers.Remove(trigger);
            }

            if (component is IMouseListener listener)
            {
                mouseListeners.Remove(listener);
            }

            if (component is IOnStart start)
            {
                onStarts.Remove(start);
            }
        }

        private void DebugDraw()
        {
            foreach (IDebugDrawable item in debugDrawables)
            {
                if (((IComponent)item).MyGameObject.Active)
                {
                    item.DebugDraw();
                }
            }

            Debug.DebugDraw();
        }

        private void DeleteGameObjects()
        {
            foreach (GameObject gameObject in deleteList)
            {
                gameObjects.Remove(gameObject);
                DeleteChildren(gameObject);

                RemoveComponentsFromLists(gameObject);
            }

            deleteList.Clear();
        }

        private void DeleteChildren(GameObject gameObject)
        {
            if (gameObject.GetAllChildren() != null)
            {
                foreach (GameObject child in gameObject.GetAllChildren())
                {
                    DeleteChildren(child);
                }
            }

            RemoveComponentsFromLists(gameObject);
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