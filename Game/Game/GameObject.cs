using System;
using System.Collections;
using System.Collections.Generic;
using Game.Component;

namespace Game
{
    public class GameObject
    {
        private List<IComponent> components = new List<IComponent>();

        public GameObject()
        {
        }

        public GameObject(string name)
        {
            Name = name;
        }

        public GameObject(GameObject parent)
        {
            Parent = parent;
            Parent.AddChild(this);
        }

        public GameObject(string name, GameObject parent)
        {
            Name = name;
            Parent = parent;
            Parent.AddChild(this);
        }

        public CTransform Transform { get; set; } = new CTransform();

        public string Name { get; set; } = "GameObject";

        public GameObject Parent { get; set; } = null;

        public int ChildCount
        {
            get { return Children.Count; }
        }

        private List<GameObject> Children { get; } = new List<GameObject>();

        // Old code, will remove if the new method using generics has been tested
        /*
        public void AddComponent(IComponent component)
        {
            // TODO: add to hashtable
            component.GameObject = this;
        }
        */

        public void AddComponent<T>()
            where T : IComponent, new()
        {
            T component = new T();
            component.MyGameObject = this;
            components.Add(component);
        }

        public T GetComponent<T>()
            where T : class, IComponent
        {
            foreach (IComponent component in components)
            {
                if (component.GetType() == typeof(T))
                {
                    return (T)component;
                }
            }

            return null;
        }

        private void AddChild(GameObject child)
        {
            if (!Children.Contains(child))
            {
                Children.Add(child);
            }
        }

        // TODO: T[] GetComponents()

        // TODO: T GetComponentInChildren()

        // TODO: T[] GetComponentsInChildren()

        // TODO: T GetComponentInParent()

        // TODO: T[] GetComponentsInChildren()

        // TODO: RemoveComponent()

        // TODO: RemoveChild()

        // TODO: GameObject[] GetChildren

        // TODO: GameObject GetChild()

        // TODO: RemoveAllChildren()

        // TODO: bool IsChildOf()
    }
}