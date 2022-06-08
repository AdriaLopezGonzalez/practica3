using SFML.Graphics;
using SFML.System;
using System.Diagnostics;

namespace TCEngine
{
    public class ShapeComponent : RenderComponent
    {
        private Shape m_Shape;
        public Shape Shape
        {
            get => m_Shape;
            set => m_Shape = value;
        }

        public ShapeComponent()
        {
        }

        public ShapeComponent(Shape _shape)
        {
            m_Shape = _shape;
        }

        public override void Draw(RenderTarget _target, RenderStates _states)
        {
            Debug.Assert(m_Shape != null);

            _states.Transform *= Owner.GetWorldTransform();

            base.Draw(_target, _states);
            _target.Draw(m_Shape, _states);
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();

            Owner.GetGlobalBoundsEvent += () => 
            {
                Debug.Assert(m_Shape != null);
                Transform worldTransform = Owner.GetWorldTransform();
                return worldTransform.TransformRect(m_Shape.GetGlobalBounds());
            };

            Owner.GetLocalBoundsEvent += () =>
            {
                Debug.Assert(m_Shape != null);
                Transform worldTransform = Owner.GetWorldTransform();
                return worldTransform.TransformRect(m_Shape.GetLocalBounds());
            };

            Center();
        }

        public void Center()
        {
            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            Debug.Assert(transformComponent != null, "Trying to change the origin of the actor " + Owner.Name + " without a TransformComponent");
            FloatRect localBounds = m_Shape.GetLocalBounds();
            transformComponent.Transform.Origin = new Vector2f(localBounds.Width * 0.5f, localBounds.Height * 0.5f);
        }


        public override void DebugDraw()
        {
            Debug.Assert(m_Shape != null);

            base.DebugDraw();
            Transform worldTransform = Owner.GetWorldTransform();
            TecnoCampusEngine.Get.DebugManager.Box(worldTransform.TransformRect(m_Shape.GetLocalBounds()), Color.Blue);
            TecnoCampusEngine.Get.DebugManager.Box(worldTransform.TransformRect(m_Shape.GetGlobalBounds()), Color.Red);
        }

        public override object Clone()
        {
            ShapeComponent clonedComponent = new ShapeComponent(m_Shape);
            return clonedComponent;
        }
    }
}
