using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class RocketLauncherComponent : BaseComponent
    {
        private const float DEFAULT_FIRE_RATE = 0.1f;

        private float m_FireRate;
        private float m_TimeToShoot;

        public RocketLauncherComponent()
        {
            m_FireRate = DEFAULT_FIRE_RATE;
            m_TimeToShoot = 0.0f;
        }

        public RocketLauncherComponent(float _fireRate)
        {
            m_FireRate = _fireRate;
            m_TimeToShoot = 0.0f;
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            if(m_TimeToShoot > 0.0f)
            {
                m_TimeToShoot -= _dt;
            }
        }

        public void Shoot(Vector2f _forward)
        {
            if (m_TimeToShoot <= 0.0f)
            {
                Actor rocketActor = new Actor("Rocket Actor");

                SpriteComponent spriteComponent = rocketActor.AddComponent<SpriteComponent>("Textures/Rocket");
                spriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Middle;

                TransformComponent actorTransform = Owner.GetComponent<TransformComponent>();
                TransformComponent transformComponent = rocketActor.AddComponent<TransformComponent>();
                transformComponent.Transform.Position = actorTransform.Transform.Position + _forward * 30.0f;
                transformComponent.Transform.Rotation = actorTransform.Transform.Rotation;

                rocketActor.AddComponent<RocketComponent>(_forward);
                rocketActor.AddComponent<AsteroidDestructorComponent>();
                rocketActor.AddComponent<ExplosionComponent>();
                rocketActor.AddComponent<OutOfWindowDestructionComponent>();

                TecnoCampusEngine.Get.Scene.CreateActor(rocketActor);

                m_TimeToShoot = m_FireRate;
            }
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.PostUpdate;
        }

        public override object Clone()
        {
            RocketLauncherComponent clonedComponent = new RocketLauncherComponent(m_FireRate);
            return clonedComponent;
        }
    }
}
