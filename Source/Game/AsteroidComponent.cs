using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class AsteroidComponent : BaseComponent
    {

        private static float[] LINEAR_SPEEDS = { 50.0f, 20.0f, 80.0f };
        private static float[] ANGULAR_SPEEDS = { 20.0f, -50.0f, 30.0f };
        private static float[] SCALES = { 0.5f, 0.7f, 0.8f };

        private static Vector2f RIGHT_VECTOR = new Vector2f(1.0f, 0.0f);
        private static Vector2f DOWN_VECTOR = new Vector2f(0.0f, 1.0f);
        
        private float m_AngularSpeed;
        private float m_LinearSpeed;
        private Vector2f m_Forward;

        public AsteroidComponent()
        {
            m_Forward = new Vector2f(1.0f, 0.0f);
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();

            Random randomGenerator = new Random();
            Vector2f windowSize = TecnoCampusEngine.Get.ViewportSize;

            int linearSpeedIndex = randomGenerator.Next(LINEAR_SPEEDS.Length);
            int angularSpeedIndex = randomGenerator.Next(ANGULAR_SPEEDS.Length);
            int scaleIndex = randomGenerator.Next(SCALES.Length);

            m_LinearSpeed = LINEAR_SPEEDS[linearSpeedIndex];
            m_AngularSpeed = ANGULAR_SPEEDS[angularSpeedIndex];
            float scale = SCALES[scaleIndex];

            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            Debug.Assert(transformComponent != null);

            transformComponent.Transform.Scale = new Vector2f(scale, scale);


            switch (randomGenerator.Next(2))
            {
                case 0:
                    transformComponent.Transform.Position = new Vector2f(-200.0f, windowSize.Y / 2.0f);
                    m_Forward = RIGHT_VECTOR.Rotate(randomGenerator.Next(-20, +20));
                    break;

                case 1:
                    transformComponent.Transform.Position = new Vector2f(windowSize.X / 2.0f, -200.0f);
                    m_Forward = DOWN_VECTOR.Rotate(randomGenerator.Next(-20, +20));
                    break;
            }

            LifeComponent lifeComponent = Owner.GetComponent<LifeComponent>();
            if (lifeComponent != null)
            {
                lifeComponent.OnLifeDecreased += ChangeAsteroidState;
            }
        }

        private void ChangeAsteroidState()
        {
            LifeComponent lifeComponent = Owner.GetComponent<LifeComponent>();
            Debug.Assert(lifeComponent != null);

            SpriteComponent spriteComponent = Owner.GetComponent<SpriteComponent>();
            Debug.Assert(spriteComponent != null);


            switch (lifeComponent.NumLifes)
            {
                case 2:
                    spriteComponent.ChangeSprite("Textures/Asteroid01");
                    break;
                case 1:
                    spriteComponent.ChangeSprite("Textures/Asteroid02");
                    break;
                default:
                    break;
            }
        }


        public override void Update(float _dt)
        {
            base.Update(_dt);

            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            Debug.Assert(transformComponent != null);

            Vector2f velocity = m_Forward * m_LinearSpeed;
            transformComponent.Transform.Position += velocity * _dt;

            transformComponent.Transform.Rotation += m_AngularSpeed * _dt;
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            AsteroidComponent clonedComponent = new AsteroidComponent();
            return clonedComponent;
        }
    }
}
