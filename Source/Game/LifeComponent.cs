using System;
using TCEngine;

namespace TCGame
{
    public class LifeComponent : BaseComponent
    {
        public Action OnLifeDecreased;

        private int m_MaxLifes;
        private int m_NumLifes;

        public int NumLifes
        {
            get => m_NumLifes;
        }

        public LifeComponent(int _numLifes)
        {
            m_MaxLifes = _numLifes;
            m_NumLifes = m_MaxLifes;
        }

        public void ResetLifes()
        {
            m_NumLifes = m_MaxLifes;
        }

        public bool IsAlive()
        {
            return m_NumLifes > 0;
        }

        public void IncreaseLife()
        {
            m_NumLifes = Math.Min(m_NumLifes + 1, m_MaxLifes);
        }


        public void DecreaseLife()
        {
            ShieldComponent shieldComponent = Owner.GetComponent<ShieldComponent>();
            if(shieldComponent == null || !shieldComponent.IsActive())
            {
                m_NumLifes = Math.Max(m_NumLifes - 1, 0);
                if (OnLifeDecreased != null)
                {
                    OnLifeDecreased();
                }

                if (m_NumLifes == 0)
                {
                    Owner.Destroy();
                }
            }
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            LifeComponent clonedComponent = new LifeComponent(m_NumLifes);
            return clonedComponent;
        }
    }
}
