using SFML.Graphics;

namespace TCEngine
{
    public class RenderComponent : BaseComponent, Drawable
    {
        public enum ERenderLayer
        {
            Background,
            Back,
            Middle,
            Front,
            HUD
        }
        public ERenderLayer m_RenderLayer = ERenderLayer.Back;

        public virtual void Draw(RenderTarget _target, RenderStates _states)
        {

        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }
    }
}
