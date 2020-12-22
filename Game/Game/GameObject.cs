using System;
using System.Collections.Generic;
using Game.Components;
using Game.Interfaces;
using Game.SceneSystem;

namespace Game
{
    public class GameObject
    {
        private bool active;
        private List<IComponent> components = new List<IComponent>();

        public GameObject(Scene scene)
            : this(scene, "GameObject", null)
        {
        }

        public GameObject(Scene scene, string name)
            : this(scene, name, null)
        {
            Name = name;
        }

        public GameObject(Scene scene, GameObject parent)
            : this(scene, "GameObject", parent)
        {
            if (!SetParent(parent))
            {
                Console.WriteLine($"Couldn't set parent for - {parent.Name} - (Cycle in hierarchy detected)");
            }
        }

        public GameObject(Scene scene, string name, GameObject parent)
        {
            if (parent != null && parent.Scene != scene)
            {
                throw new Exception($"GameObject '{name}' and the assigned parent are not in the same scene!");
            }

            Name = name;
            Active = true;
            Transform.MyGameObject = this;
            Scene = scene;
            Scene.AddGameObject(this);
            if (parent != null && !SetParent(parent))
            {
                Console.WriteLine($"Couldn't set parent for - {parent.Name} - (Cycle in hierarchy detected)");
            }
        }

        public CTransform Transform { get; private set; } = new CTransform();

        public Scene Scene { get; set; }

        public string Name { get; set; } = "GameObject";

        public bool Active
        {
            get
            {
                return active;
            }

            set
            {
                active = value;
                foreach (GameObject child in Children)
                {
                    child.Active = value;
                }
            }
        }

        public int ChildCount
        {
            get { return Children.Count; }
        }

        private GameObject Parent { get; set; } = null;

        private List<GameObject> Children { get; } = new List<GameObject>();

        /// <summary>
        /// Instanciate new component of type T and add it to GameObject.
        /// </summary>
        /// <typeparam name="T">Class that implements IComponent.</typeparam>
        public void AddComponent<T>()
            where T : IComponent, new()
        {
            T component = new T();
            component.MyGameObject = this;
            components.Add(component);

            // update scene component lists
            Scene.AddComponent(component);
        }

        /// <summary>
        /// Returns the first component of type T or otherwise null.
        /// </summary>
        /// <typeparam name="T">Class that implements IComponent.</typeparam>
        /// <returns>Component of type T or null.</returns>
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

        /// <summary>
        /// Returns all components as an IComponent array.
        /// </summary>
        /// <returns>All Components as array.</returns>
        public IComponent[] GetAllComponents()
        {
            return components.ToArray();
        }

        /// <summary>
        /// Returns all components of type T or null if non of type T were found.
        /// </summary>
        /// <typeparam name="T">Class that implements IComponent.</typeparam>
        /// <returns>All components of type T or null.</returns>
        public T[] GetComponents<T>()
            where T : class, IComponent
        {
            List<T> list = new List<T>();
            foreach (IComponent component in components)
            {
                if (component.GetType() == typeof(T))
                {
                    list.Add((T)component);
                }
            }

            if (list.Count > 0)
            {
                return list.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Removes the first component of type T.
        /// </summary>
        /// <typeparam name="T">Class that implements IComponent.</typeparam>
        public void RemoveComponent<T>()
            where T : class, IComponent
        {
            foreach (IComponent component in components)
            {
                if (component.GetType() == typeof(T))
                {
                    components.Remove(component);

                    Scene.RemoveComponent(component);
                    return;
                }
            }
        }

        /// <summary>
        /// Removes <c>component</c> of type T from GameObject.
        /// </summary>
        /// <typeparam name="T">Class that implements IComponent.</typeparam>
        /// <param name="component">Component to be removed.</param>
        public void RemoveComponent<T>(T component)
            where T : class, IComponent
        {
            foreach (IComponent comp in components)
            {
                if (comp.GetType() == typeof(T) && comp == component)
                {
                    components.Remove(comp);
                    Scene.RemoveComponent(component);
                    return;
                }
            }
        }

        /// <summary>
        /// Returns the number of components this GameObject owns.
        /// </summary>
        /// <returns>Number of components.</returns>
        public int GetNumberComponents()
        {
            return components.Count;
        }

        /// <summary>
        /// Returns the parent of this GameObject.
        /// </summary>
        /// <returns>null if there is no parent, otherwise returns parent as GameObject.</returns>
        public GameObject GetParent()
        {
            return Parent;
        }

        /// <summary>
        /// Checks for cycles in the GameOject hierarchy. If there is a cycle then it returns <c>false</c>and aborts, otherwise the new parent is set and <c>true</c> returned.
        /// </summary>
        /// <param name="parent">GameObject you want to be the parent of this.</param>
        /// <returns>false if failed and true if succeded.</returns>
        public bool SetParent(GameObject parent)
        {
            // Check for cycle before setting parent
            GameObject tmp = parent;
            while (tmp != null)
            {
                if (tmp == this)
                {
                    return false; // can't set parent because of cycle
                }

                tmp = tmp.GetParent();
            }

            // check if this object had a parent before
            if (Parent != null)
            {
                RemoveParent(); // make sure this object is removed from childs of former parent
            }

            Parent = parent;
            Parent.AddChild(this); // set this as child of parent
            return true; // parent successfully set
        }

        /// <summary>
        /// Sets parent of this GameObject null.
        /// </summary>
        public void RemoveParent()
        {
            Parent.RemoveChild(this);
            Parent = null;
        }

        // TODO: Is this usefull? Index is hard to predict without a way to sort children. Mybe get child by name?
        public GameObject GetChild(int index)
        {
            if (index < ChildCount)
            {
                return Children[index];
            }

            return null;
        }

        /// <summary>
        /// Returns all children of this GameObject.
        /// </summary>
        /// <returns>null if ther are no children, otherwise retuns a Gameobject array.</returns>
        public GameObject[] GetAllChildren()
        {
            if (ChildCount > 0)
            {
                return Children.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Check if <c>gameObject</c> is a child this GameObject.
        /// </summary>
        /// <param name="gameObject">The Object you want to check if it is a child of this GameObject.</param>
        /// <returns>true or false.</returns>
        public bool IsChildOfThis(GameObject gameObject)
        {
            return Children.Contains(gameObject);
        }

        private void AddChild(GameObject child)
        {
            if (!Children.Contains(child))
            {
                Children.Add(child);
            }
        }

        private void RemoveChild(GameObject child)
        {
            foreach (GameObject gameObject in Children.ToArray())
            {
                if (gameObject == child)
                {
                    Children.Remove(gameObject);
                }
            }
        }
    }
}