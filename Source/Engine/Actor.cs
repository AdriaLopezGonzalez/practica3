using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TCEngine
{
    public class Actor
    {
        private string m_Name;
        private List<BaseComponent> m_Components = new List<BaseComponent>();
        public delegate FloatRect GetBoundsHandler();

        public string Name
        {
            get => m_Name;
        }

        public event GetBoundsHandler GetLocalBoundsEvent;
        public event GetBoundsHandler GetGlobalBoundsEvent;

        public Action<Actor> OnDestroy;

        public List<BaseComponent> Components
        {
            get => m_Components;
        }

        public Actor(string _name)
        {
            m_Name = _name;
        }

        public T AddComponent<T>() where T : BaseComponent, new()
        {
            Debug.Assert(GetComponent<T>() == null, "This actor (name:" + m_Name + ") already has a component of type " + typeof(T).ToString());
            T component = new T();
            component.Owner = this;
            m_Components.Add(component);
            return component;
        }

        public T AddComponent<T>(params object[] _componentArguments) where T : BaseComponent
        {
            Debug.Assert(GetComponent<T>() == null, "This actor (name:" + m_Name + ") already has a component of type " + typeof(T).ToString());
            T component = (T)Activator.CreateInstance(typeof(T), _componentArguments);
            component.Owner = this;
            m_Components.Add(component);
            return component;
        }

        public void AddComponentUnsafe(BaseComponent _component)
        {
            Type baseComponentType = typeof(BaseComponent);
            Debug.Assert(_component.GetType() != baseComponentType);

            _component.Owner = this;
            m_Components.Add(_component);
        }

        public T GetComponent<T>() where T : BaseComponent
        {
            return m_Components.Find(component => component is T) as T;
        }

        public void Destroy()
        {
            TecnoCampusEngine.Get.Scene.Destroy(this);
        }

        public void OnCreated()
        {
            foreach(BaseComponent component in m_Components)
            {
                component.OnActorCreated();
            }
        }

        public void OnDestroyed()
        {
            if (OnDestroy != null)
            {
                OnDestroy(this);
            }

            foreach (BaseComponent component in m_Components)
            {
                component.OnActorDestroyed();
            }
        }

        public Transform GetWorldTransform()
        {
            TransformComponent transformComponent = GetComponent<TransformComponent>();
            return ( transformComponent != null) ? transformComponent.Transform.Transform : Transform.Identity;
        }

        public Vector2f GetPosition()
        {
            TransformComponent transformComponent = GetComponent<TransformComponent>();
            return (transformComponent != null) ? transformComponent.Transform.Position : new Vector2f(0.0f, 0.0f);
        }

        public float GetRotation()
        {
            TransformComponent transformComponent = GetComponent<TransformComponent>();
            return (transformComponent != null) ? transformComponent.Transform.Rotation : 0.0f;
        }

        public void EnableDrawDebugOnComponents()
        {
            foreach(BaseComponent component in m_Components)
            {
                component.EnableDebugDraw();
            }
        }

        public void DisableDrawDebugOnComponents()
        {
            foreach (BaseComponent component in m_Components)
            {
                component.DisableDebugDraw();
            }
        }

        public FloatRect GetLocalBounds()
        {
            return (GetLocalBoundsEvent != null) ? GetLocalBoundsEvent.Invoke() : new FloatRect();
        }

        public FloatRect GetGlobalBounds()
        {
            return (GetGlobalBoundsEvent != null) ? GetGlobalBoundsEvent.Invoke() : new FloatRect();
        }
    }
}
