using System;
using TCEngine;

namespace TCGame
{
    public class TimerComponent : BaseComponent
    {
        private const int DEFAULT_DURATION = 5;

        private float m_Duration;
        private float m_CurrentTime = 0;

        private bool m_Loop = false;
        private bool m_HasFinished = false;


        public Action OnTime;

        public float Duration
        {
            get => m_Duration;
            set => m_Duration = value;
        }

        public bool Loop
        {
            get => m_Loop;
            set => m_Loop = value;
        }


        public TimerComponent()
        {
            m_Duration = DEFAULT_DURATION;
        }

        public TimerComponent(float _duration)
        {
            m_Duration = _duration;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            if( !m_HasFinished )
            {
                m_CurrentTime += _dt;

                if( m_CurrentTime >= m_Duration)
                {
                    if( OnTime != null )
                    {
                        OnTime();
                    }

                    if (m_Loop)
                    {
                        ResetTimer();
                    }
                    else
                    {
                        m_HasFinished = true;
                    }
                }
            }
        }

        public void ResetTimer()
        {
            m_HasFinished = false;
            m_CurrentTime = 0.0f;
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            TimerComponent clonedComponent = new TimerComponent(m_Duration);
            return clonedComponent;
        }
    }
}
