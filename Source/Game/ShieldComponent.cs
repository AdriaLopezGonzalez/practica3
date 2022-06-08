using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class ShieldComponent : BaseComponent
    {

        private const float LOADING_TIME = 0.2f;
        private const float LOADED_TIME = 5.0f;
        private const float UNLOADING_TIME = 0.2f;

        private const float MIN_SCALE = 0.1f;
        private const float MAX_SCALE = 1.0f;

        private Actor m_ShieldActor;

        private enum EState
        {
            Inactive,
            Loading,
            Loaded,
            Unloading
        }

        private EState m_CurrentState;
        private float m_CurrentStateTime = 0.0f;

        public ShieldComponent()
        {
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();
            m_CurrentState = EState.Inactive;
        }

        public void ActivateShield()
        {
            if (m_CurrentState == EState.Inactive)
            {
                ChangeState(EState.Loading);
            }
        }

        public bool IsActive()
        {
            return m_CurrentState != EState.Inactive;
        }



        public override void Update(float _dt)
        {
            base.Update(_dt);

            // TODO (1): You need to implement the code that update the different states of the ShieldComponent in the Update
            switch (m_CurrentState)
            {
                case EState.Inactive:
                    UpdateInactive();
                    break;
                case EState.Loading:
                    UpdateLoading(_dt);
                    break;
                case EState.Loaded:
                    UpdateLoaded(_dt);
                    break;
                case EState.Unloading:
                    UpdateUnloading(_dt);
                    break;
                default:
                    break;
            }
        }

        private void UpdateInactive()
        {
        }

        private void UpdateLoading(float _dt)
        {
            m_CurrentStateTime += _dt;

            float actualScale = MathUtil.Lerp(MIN_SCALE, MAX_SCALE, m_CurrentStateTime / 0.2f);

            m_ShieldActor.GetComponent<TransformComponent>().Transform.Scale = new Vector2f(actualScale, actualScale);

            if (m_CurrentStateTime >= LOADING_TIME)
            {
                ChangeState(EState.Loaded);
                m_CurrentStateTime = 0;
            }
        }

        private void UpdateLoaded(float _dt)
        {
            m_CurrentStateTime += _dt;

            if(m_CurrentStateTime >= LOADED_TIME)
            {
                ChangeState(EState.Unloading);
                m_CurrentStateTime = 0;
            }
        }

        private void UpdateUnloading(float _dt)
        {
            m_CurrentStateTime += _dt;

            float actualScale = MathUtil.Lerp(MIN_SCALE, MAX_SCALE, (UNLOADING_TIME - m_CurrentStateTime) / 0.2f);

            m_ShieldActor.GetComponent<TransformComponent>().Transform.Scale = new Vector2f(actualScale, actualScale);

            if (m_CurrentStateTime >= UNLOADING_TIME)
            {
                ChangeState(EState.Inactive);
                m_CurrentStateTime = 0;
            }
        }

        private void CreateShieldActor()
        {
            m_ShieldActor = new Actor("Shield Actor");

            m_ShieldActor.AddComponent<AnimatedSpriteComponent>("Textures/Shield", 3u, 2u);
            m_ShieldActor.AddComponent<TransformComponent>().Transform.Scale = new Vector2f(0, 0);
            m_ShieldActor.AddComponent<ParentActorComponent>(Owner, new Vector2f(0.0f, 20.0f));

            TecnoCampusEngine.Get.Scene.CreateActor(m_ShieldActor);
        }

        private void ChangeState(EState _newState)
        {
            OnLeaveState(m_CurrentState);
            OnEnterState(_newState);

            m_CurrentState = _newState;
        }

        private void OnEnterState(EState _nextState)
        {
            switch (_nextState)
            {
                case EState.Loading:
                    CreateShieldActor();
                    break;
                default:
                    break;
            }
        }
        private void OnLeaveState(EState _previousState)
        {
            switch (_previousState)
            {
                case EState.Unloading:
                    m_ShieldActor.Destroy();
                    m_ShieldActor = null;
                    break;
                default:
                    break;
            }
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            ShieldComponent clonedComponent = new ShieldComponent();
            return clonedComponent;
        }
    }
}
