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

        private Hashtable components = new Hashtable()
        {
            { "EXAMPLE1", "First component object" },
        };

        public void AddComponent(IComponent component)
        {
            // TODO: add to hashtable
            component.GameObject = this;
        }
    }
}