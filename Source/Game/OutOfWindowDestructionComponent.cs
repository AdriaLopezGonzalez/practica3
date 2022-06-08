using SFML.Graphics;
using SFML.System;
using System;
using TCEngine;

namespace TCGame
{
    public class OutOfWindowDestructionComponent : BaseComponent
    {
        public OutOfWindowDestructionComponent()
        {
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.PostUpdate;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            FloatRect actorAABB = Owner.GetGlobalBounds();
            Vector2f viewportSize = TecnoCampusEngine.Get.ViewportSize;

            if ( (actorAABB.Left > viewportSize.X) ||
                 (actorAABB.Left + actorAABB.Width < 0.0f) ||
                 (actorAABB.Top + actorAABB.Width < 0.0f) ||
                 (actorAABB.Top > viewportSize.Y))
            {
                ExplosionComponent explosionComponent = Owner.GetComponent<ExplosionComponent>();
                if (explosionComponent != null)
                {
                    explosionComponent.DisableExplosion = true;
                }
                Owner.Destroy();
            }
        }

        public override object Clone()
        {
            return new OutOfWindowDestructionComponent();
        }
    }
}
