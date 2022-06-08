using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class RocketComponent : BaseComponent
    {
        private static Vector2f UP_VECTOR = new Vector2f(0.0f, -1.0f);

        private const float DEFAULT_ANGULAR_SPEED = 90.0f;
        private const float DEFAULT_LINEAR_SPEED = 500.0f;

        private float m_AngularSpeed;
        private float m_LinearSpeed;
        private Vector2f m_Forward;

        public RocketComponent(Vector2f _forward)
        {
            m_AngularSpeed = DEFAULT_ANGULAR_SPEED;
            m_LinearSpeed = DEFAULT_LINEAR_SPEED;
            m_Forward = _forward;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            // TODO (4): Implement the update of the rocket!
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            RocketComponent clonedComponent = new RocketComponent(m_Forward);
            return clonedComponent;
        }
    }
}
