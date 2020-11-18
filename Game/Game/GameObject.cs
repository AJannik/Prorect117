using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Game.Component;

namespace Game
{
    public class GameObject
    {
        public Transform transform = new Transform();

        public string Name { get; set; }

        public GameObject Parent { get; set; }

        private Hashtable components = new Hashtable()
        {
            { "EXAMPLE1", "First component object" },
        };

        public GameObject()
        {
            this.Name = "GameObject";
            this.Parent = null;
        }

        public GameObject(string name)
        {
            this.Name = name;
            this.Parent = null;
        }

        public GameObject(GameObject parent)
        {
            this.Name = "GameObject";
            this.Parent = parent;
        }

        public GameObject(string name, GameObject parent)
        {
            this.Name = name;
            this.Parent = parent;
        }

        public void AddComponent(IComponent component)
        {
            // TODO: add to hashtable
            component.GameObject = this;
        }
    }
}