using SFML.System;
using SFML.Window;
using TCEngine;

namespace TCGame
{
    public class ShipControllerComponent : BaseComponent
    {
        private static Vector2f UP_VECTOR = new Vector2f(0.0f, -1.0f);

        private const float DEFAULT_TURBO_SPEED = 400.0f;
        private const float DEFAULT_MOVEMENT_SPEED = 200.0f;
        private const float DEFAULT_ANGULAR_SPEED = 100.0f;

        private float m_LinearSpeed;
        private float m_AngularSpeed;
        private Vector2f m_Forward;
        private bool m_IsUsingTurbo;

        public ShipControllerComponent()
        {
            m_LinearSpeed = DEFAULT_MOVEMENT_SPEED;
            m_AngularSpeed = DEFAULT_ANGULAR_SPEED;
            m_Forward = UP_VECTOR;
            m_IsUsingTurbo = false;
        }


        public override void Update(float _dt)
        {
            base.Update(_dt);

            // TODO (2): Change the property of this component that tells us if we are using turbo or not when pressing the Left Shift key
            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                m_IsUsingTurbo = true;
            }
            else
            {
                m_IsUsingTurbo = false;
            }

            // Check if it is using the Turbo
            CalculateSpeed();

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                RotateLeft(_dt);
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                RotateRight(_dt);
            }

            Move(_dt);


            if (Keyboard.IsKeyPressed(Keyboard.Key.C))
            {
                // Shoot Rocket
                RocketLauncherComponent rocketLauncherComponent = Owner.GetComponent<RocketLauncherComponent>();
                if (rocketLauncherComponent != null)
                {
                    rocketLauncherComponent.Shoot(m_Forward);
                }
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                // Shoot laser
                LaserWeaponComponent laserWeaponComponent = Owner.GetComponent<LaserWeaponComponent>();
                if (laserWeaponComponent != null)
                {
                    laserWeaponComponent.Shoot();
                }
            }

            // TODO (1): Press G in order to activate the shield
            if (Keyboard.IsKeyPressed(Keyboard.Key.G))
            {
                // Shoot laser
                ShieldComponent shieldComponent = Owner.GetComponent<ShieldComponent>();
                if (shieldComponent != null)
                {
                    shieldComponent.ActivateShield();
                }
            }
        }

        private void RotateLeft(float _dt)
        {
            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            transformComponent.Transform.Rotation -= m_AngularSpeed * _dt;
            UpdateForward();
        }
        private void RotateRight(float _dt)
        {
            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            transformComponent.Transform.Rotation += m_AngularSpeed * _dt;
            UpdateForward();
        }

        private void UpdateForward()
        {
            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            m_Forward = UP_VECTOR.Rotate(transformComponent.Transform.Rotation);
        }

        private void CalculateSpeed()
        {
            if (m_IsUsingTurbo)
            {
                m_LinearSpeed = DEFAULT_TURBO_SPEED;
                m_AngularSpeed = 0.0f;
            }
            else 
            {
                m_LinearSpeed = DEFAULT_MOVEMENT_SPEED;
                m_AngularSpeed = DEFAULT_ANGULAR_SPEED;
            }
        }

        private void Move(float _dt)
        {
            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            transformComponent.Transform.Position += m_Forward * m_LinearSpeed * _dt;
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.PreUpdate;
        }

        public override object Clone()
        {
            ShipControllerComponent clonedComponent = new ShipControllerComponent();
            return clonedComponent;
        }
    }
}
