using SFML.Graphics;
using SFML.System;
using SFML.Window;
using TCEngine;

namespace TCGame
{
    public class LaserWeaponComponent : BaseComponent
    {
        private const float DEFAULT_FIRE_RATE = 0.3f;

        private float m_FireRate;
        private float m_TimeToShoot = 0.0f;

        public LaserWeaponComponent()
        {
            m_FireRate = DEFAULT_FIRE_RATE;
            m_TimeToShoot = 0.0f;
        }

        public LaserWeaponComponent(float _fireRate)
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

            // TODO (3): Remember to update the m_TimeToShoot member
            if (m_TimeToShoot > 0.0f)
            {
                m_TimeToShoot -= _dt;
            }
        }

        public void Shoot()
        {
            // TODO (3): Implement the creation of the laser bullet. Remember that the LaserWeaponComponent has a fire rate of 0.3 seconds
            if (m_TimeToShoot <= 0.0f)
            {
                Actor laserActor = new Actor("Laser Actor");

                SpriteComponent spriteComponent = laserActor.AddComponent<SpriteComponent>("Textures/Bullet");
                spriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Middle;

                TransformComponent actorTransform = Owner.GetComponent<TransformComponent>();
                TransformComponent transformComponent = laserActor.AddComponent<TransformComponent>();
                transformComponent.Transform.Position = actorTransform.Transform.Position;
                RenderWindow _Window = TecnoCampusEngine.Get.Window;
                Vector2f _mouseForward = new Vector2f(Mouse.GetPosition(_Window).X, Mouse.GetPosition(_Window).Y);
                Vector2f _bulletForward = _mouseForward - transformComponent.Transform.Position;

                laserActor.AddComponent<ForwardMovementComponent>(_bulletForward.Normal(), 500.0f);

                laserActor.AddComponent<AsteroidDestructorComponent>();
                laserActor.AddComponent<ExplosionComponent>();
                laserActor.AddComponent<OutOfWindowDestructionComponent>();

                TecnoCampusEngine.Get.Scene.CreateActor(laserActor);

                m_TimeToShoot = m_FireRate;
            }
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.PostUpdate;
        }

        public override object Clone()
        {
            LaserWeaponComponent clonedComponent = new LaserWeaponComponent(m_FireRate);
            return clonedComponent;
        }
    }
}
