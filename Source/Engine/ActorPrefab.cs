using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TCEngine
{
    public class ActorPrefab
    {
        private string m_PrefabName;
        private List<BaseComponent> m_Components = new List<BaseComponent>();

        public string PrefabName
        {
            get => m_PrefabName;
        }

        public List<BaseComponent> Components
        {
            get => m_Components;
        }

        public ActorPrefab(string _prefabName)
        {
            m_PrefabName = _prefabName;
        }

        public T AddComponent<T>() where T : BaseComponent, new()
        {
            Debug.Assert(GetComponent<T>() == null, "This prebab (name:" + m_PrefabName + ") already has a component of type " + typeof(T).ToString());
            T component = new T();
            m_Components.Add(component);
            return component;
        }

        public T AddComponent<T>(params object[] _componentArguments) where T : BaseComponent
        {
            Debug.Assert(GetComponent<T>() == null, "This prebab (name:" + m_PrefabName + ") already has a component of type " + typeof(T).ToString());
            T component = (T)Activator.CreateInstance(typeof(T), _componentArguments);
            m_Components.Add(component);
            return component;
        }

        public T GetComponent<T>() where T : BaseComponent
        {
            return m_Components.Find(component => component is T) as T;
        }
    }
}
