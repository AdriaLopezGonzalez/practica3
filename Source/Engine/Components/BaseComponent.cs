using System;
using System.Diagnostics;

namespace TCEngine
{
    public abstract class BaseComponent : ICloneable
    {
        public enum EComponentUpdateCategory
        {
            PreUpdate,
            Update,
            PostUpdate
        }

        private Actor m_Owner;
        private bool m_DebugDraw;

        public Actor Owner
        {
            get => m_Owner;
            set
            {
                Debug.Assert(m_Owner == null, "The owner of a component cannot be changed");
                m_Owner = value;
            }
        }

        protected BaseComponent()
        {
            m_DebugDraw = false;
        }

        public virtual void Update(float _dt) 
        {
            if (IsDebugDrawEnabled())
            {
                DebugDraw();
            }
        }

        public virtual void OnActorCreated()
        {
            TecnoCampusEngine.Get.Scene.RegisterComponentForUpdate(this);
        }

        public virtual void OnActorDestroyed()
        {
            TecnoCampusEngine.Get.Scene.UnregisterComponentFromUpdate(this);
        }

        public abstract EComponentUpdateCategory GetUpdateCategory();

        public void EnableDebugDraw()
        {
            m_DebugDraw = true;
        }

        public void DisableDebugDraw()
        {
            m_DebugDraw = false;
        }

        public bool IsDebugDrawEnabled()
        {
            return m_DebugDraw;
        }

        public virtual void DebugDraw()
        {
        }

        public virtual object Clone()
        {
            Debug.Assert(false, "Trying to clone a component which Clone method has not been implemented");
            return null;
        }
    }
}
