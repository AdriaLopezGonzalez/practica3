using SFML.System;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class ForwardMovementComponent : BaseComponent
    {
        private static Vector2f UP_VECTOR = new Vector2f(0.0f, -1.0f);

        private const float DEFAULT_SPEED = 100.0f;

        private Vector2f m_Forward;
        private float m_Speed;

        public Vector2f Forward
        {
            get => m_Forward;
            set => m_Forward = value;
        }

        public ForwardMovementComponent(Vector2f _forward)
        {
            m_Forward = _forward;
            m_Speed = DEFAULT_SPEED;
        }

        public ForwardMovementComponent(Vector2f _forward, float _speed)
        {
            m_Forward = _forward;
            m_Speed = _speed;
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();

            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            Debug.Assert(transformComponent != null);

            transformComponent.Transform.Rotation = MathUtil.AngleWithSign(m_Forward, UP_VECTOR);
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            Debug.Assert(transformComponent != null);

            Vector2f velocity = m_Forward * m_Speed;
            transformComponent.Transform.Position += velocity * _dt;
        }

        public override object Clone()
        {
            ForwardMovementComponent clonedComponent = new ForwardMovementComponent(m_Forward, m_Speed);
            return clonedComponent;
        }
    }
}
