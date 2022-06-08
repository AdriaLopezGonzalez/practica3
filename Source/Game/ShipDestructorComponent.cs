using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class ShipDestructorComponent : BaseComponent
    {
        public ShipDestructorComponent()
        {
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            List<ShipControllerComponent> ships = TecnoCampusEngine.Get.Scene.GetAllComponents<ShipControllerComponent>();
            if(ships.Count > 0)
            {
                TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
                Debug.Assert(transformComponent != null);

                foreach (ShipControllerComponent shipController in ships)
                {
                    TransformComponent shipTransformComponent = shipController.Owner.GetComponent<TransformComponent>();
                    Debug.Assert(shipTransformComponent != null);

                    float squaredDistance = (shipTransformComponent.Transform.Position - transformComponent.Transform.Position).SizeSquared();
                    if (squaredDistance < 70.0f * 70.0f)
                    {
                        LifeComponent shipLifeComponent = shipController.Owner.GetComponent<LifeComponent>();
                        if (shipLifeComponent != null)
                        {
                            shipLifeComponent.DecreaseLife();
                        }
                        else
                        {
                            shipController.Owner.Destroy();
                        }
                    }
                }
            }
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            ShipDestructorComponent clonedComponent = new ShipDestructorComponent();
            return clonedComponent;
        }
    }
}
