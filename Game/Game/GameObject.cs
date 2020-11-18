using System;
using System.Collections;
using System.Collections.Generic;
using Game.Component;

namespace Game
{
    public class GameObject
    {
        public Transform transform = new Transform();

        private Hashtable components = new Hashtable()
        {
            { "EXAMPLE1", "First component object" },
        };

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
            AddMeToChildren();
        }

        public GameObject(string name, GameObject parent)
        {
            Name = name;
            Parent = parent;
            AddMeToChildren();
        }

        public string Name { get; set; } = "GameObject";

        public GameObject Parent { get; set; } = null;

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

            // TODO: add component to hashtable
        }

        public T GetComponent<T>()
            where T : class, IComponent
        {
            T component = null; // TODO: get component from hashtable
            return component;
        }

        private void AddMeToChildren()
        {
            if (!Parent.Children.Contains(this))
            {
                Parent.Children.Add(this);
            }
        }
    }
}