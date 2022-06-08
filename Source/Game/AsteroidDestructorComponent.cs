using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class AsteroidDestructorComponent : BaseComponent
    {
        public AsteroidDestructorComponent()
        {
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            List<AsteroidComponent> asteroids = TecnoCampusEngine.Get.Scene.GetAllComponents<AsteroidComponent>();
            if( asteroids.Count > 0)
            {
                TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
                Debug.Assert(transformComponent != null);

                foreach (AsteroidComponent asteroid in asteroids)
                {
                    TransformComponent asteroidTransformComponent = asteroid.Owner.GetComponent<TransformComponent>();
                    Debug.Assert(asteroidTransformComponent != null);

                    float squaredDistance = (asteroidTransformComponent.Transform.Position - transformComponent.Transform.Position).SizeSquared();
                    if (squaredDistance < 50.0f * 50.0f)
                    {
                        Owner.Destroy();

                        LifeComponent asteroidLifeComponent = asteroid.Owner.GetComponent<LifeComponent>();
                        if (asteroidLifeComponent != null)
                        {
                            asteroidLifeComponent.DecreaseLife();

                            if(!asteroidLifeComponent.IsAlive())
                            {
                                // TODO (5): Change the number of points
                            }
                        }
                        else
                        {
                            // TODO (5): If there is no LifeComponent, the asteroid is destroyed automatically, and we must also change the number of points
                            asteroid.Owner.Destroy();
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
            AsteroidDestructorComponent clonedComponent = new AsteroidDestructorComponent();
            return clonedComponent;
        }
    }
}
