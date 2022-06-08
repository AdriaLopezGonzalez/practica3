using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class AlwaysInWindowComponent : BaseComponent
    {
        public AlwaysInWindowComponent()
        {
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            Vector2f viewportSize = TecnoCampusEngine.Get.ViewportSize;
            FloatRect ownerAABB = Owner.GetGlobalBounds();

            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            Vector2f currentPosition = transformComponent.Transform.Position;

            // Top Bounds
            if (ownerAABB.Top + ownerAABB.Height < 0.0f)
            {
                transformComponent.Transform.Position = new Vector2f(currentPosition.X, viewportSize.Y + ownerAABB.Height * 0.5f);
            }

            // Bottom Bounds
            if (ownerAABB.Top > viewportSize.Y)
            {
                transformComponent.Transform.Position = new Vector2f(currentPosition.X, -ownerAABB.Height * 0.5f);
            }

            // Left Bounds
            if (ownerAABB.Left + ownerAABB.Width < 0.0f)
            {
                transformComponent.Transform.Position = new Vector2f(viewportSize.X + ownerAABB.Width * 0.5f, currentPosition.Y);
            }

            // Right Bounds
            if (ownerAABB.Left > viewportSize.X)
            {
                transformComponent.Transform.Position = new Vector2f(-ownerAABB.Width * 0.5f, currentPosition.Y);
            }
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            AlwaysInWindowComponent clonedComponent = new AlwaysInWindowComponent();
            return clonedComponent;
        }
    }
}
