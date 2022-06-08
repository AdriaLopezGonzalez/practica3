using SFML.Graphics;

namespace TCEngine
{
    public class TransformComponent : BaseComponent
    {

        private Transformable m_Transform = new Transformable();

        public Transformable Transform
        {
            get => m_Transform;
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.PreUpdate;
        }

        public override void DebugDraw()
        {
            base.DebugDraw();
            TecnoCampusEngine.Get.DebugManager.Circle(m_Transform.Origin, 2.0f, Color.Blue);
            TecnoCampusEngine.Get.DebugManager.Circle(m_Transform.Position, 2.0f, Color.Green);

            TecnoCampusEngine.Get.DebugManager.Label(m_Transform.Position, "Position (" + m_Transform.Position.X + ", " + m_Transform.Position.Y + ")", Color.Green);
        }

        public override object Clone()
        {
            TransformComponent clonedComponent = new TransformComponent();
            clonedComponent.m_Transform = new Transformable(m_Transform);
            return clonedComponent;
        }
    }
}
