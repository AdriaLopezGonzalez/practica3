using SFML.Graphics;
using SFML.System;

namespace TCEngine
{
    public class ParentActorComponent : BaseComponent
    {
        private Actor m_ParentActor;
        private Vector2f m_ParentOffset;

        public Actor ParentActor
        {
            get => m_ParentActor;
            set => m_ParentActor = value;
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public ParentActorComponent()
        {
        }
        
        public ParentActorComponent(Actor _parentActor)
        {
            m_ParentActor = _parentActor;
            m_ParentOffset = new Vector2f(0.0f, 0.0f);
        }

        public ParentActorComponent(Actor _parentActor, Vector2f _parentOffset)
        {
            m_ParentActor = _parentActor;
            m_ParentOffset = _parentOffset;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            if (m_ParentActor != null)
            {
                TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
                TransformComponent parentTransformComponent = m_ParentActor.GetComponent<TransformComponent>();
                if (transformComponent != null)
                {
                    transformComponent.Transform.Position = parentTransformComponent.Transform.Transform.TransformPoint(m_ParentOffset);
                    transformComponent.Transform.Position += parentTransformComponent.Transform.Origin.Rotate(parentTransformComponent.Transform.Rotation);

                    transformComponent.Transform.Rotation = parentTransformComponent.Transform.Rotation;
                }
            }
        }

        public override object Clone()
        {
            ParentActorComponent clonedComponent = new ParentActorComponent(m_ParentActor, m_ParentOffset);
            return clonedComponent;
        }
    }
}
