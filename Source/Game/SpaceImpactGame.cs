using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    class SpaceImpactGame : Game
    {
        private enum EGameState
        {
            None,

            Start,
            Playing,
            Restarting,
            GameOver
        }

        private EGameState m_CurrentState;
        private TextHUDComponent m_TextHUDComponent;
        private Actor m_GameHUDActor;
        private int m_PendingLifesToGameOver;
        private float m_GameOverTimer;
        public void Init()
        {
            CreateBackground();
            CreateTextHUDActor();

            ChangeState(EGameState.Start);
        }

        public void DeInit()
        {
        }

        public void Update(float _dt)
        {
            switch(m_CurrentState)
            {
                case EGameState.GameOver:
                    m_GameOverTimer -= _dt;
                    if(m_GameOverTimer <= 0.0f)
                    {
                        ChangeState(EGameState.Start);
                    }
                    break;
                default:
                    break;
            }
        }

        private void CreateBackground()
        {
            Actor actor = new Actor("Background Actor");
            actor.AddComponent<SpriteComponent>("Textures/Background");
            TecnoCampusEngine.Get.Scene.CreateActor(actor);
        }

        private void CreateTextHUDActor()
        {
            Debug.Assert(m_TextHUDComponent == null);
            
            Actor actor = new Actor("StartGame HUD Actor");

            // Add the transform component and set its position correctly
            TransformComponent transformComponent = actor.AddComponent<TransformComponent>();
            transformComponent.Transform.Position = TecnoCampusEngine.Get.ViewportSize * 0.5f;

            // Add the Text HUD Component
            m_TextHUDComponent = actor.AddComponent<TextHUDComponent>();

            // Add the actor to the scene
            TecnoCampusEngine.Get.Scene.CreateActor(actor);
        }

        private void CreateGameHUD()
        {
            m_GameHUDActor = new Actor("Game HUD");

            TransformComponent transformComponent = m_GameHUDActor.AddComponent<TransformComponent>();
            transformComponent.Transform.Position = new Vector2f(0.0f, 0.0f);

            m_GameHUDActor.AddComponent<GameHUDComponent>(m_PendingLifesToGameOver);
            TecnoCampusEngine.Get.Scene.CreateActor(m_GameHUDActor);
        }

        private void CreateShip()
        {
            Actor actor = new Actor("Ship Actor");

            SpriteComponent spriteComponent = actor.AddComponent<SpriteComponent>("Textures/Ship");
            spriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Front;

            // Add the transform component and set its position correctly
            TransformComponent transformComponent = actor.AddComponent<TransformComponent>();
            transformComponent.Transform.Position = new Vector2f(600.0f, 200.0f);

            // Add the rest of the components to the ship actor
            actor.AddComponent<ShipControllerComponent>();
            actor.AddComponent<ShieldComponent>();
            actor.AddComponent<RocketLauncherComponent>();
            actor.AddComponent<LaserWeaponComponent>();
            actor.AddComponent<AlwaysInWindowComponent>();
            LifeComponent lifeComponent = actor.AddComponent<LifeComponent>(1);
            lifeComponent.OnLifeDecreased += OnShipLifeDecreased;


            // Add the actor to the scene
            TecnoCampusEngine.Get.Scene.CreateActor(actor);


            // Add the flame effects
            Actor rightFlameActor = new Actor("Right Flame");
            AnimatedSpriteComponent rightAnimatedSpriteComponent = rightFlameActor.AddComponent<AnimatedSpriteComponent>("Textures/Flame", 7u, 1u);
            rightAnimatedSpriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Middle;
            rightFlameActor.AddComponent<TransformComponent>();
            rightFlameActor.AddComponent<ParentActorComponent>(actor, new Vector2f(20.0f, 62.0f));

            TecnoCampusEngine.Get.Scene.CreateActor(rightFlameActor);

            Actor leftFlameActor = new Actor("Left Flame");
            AnimatedSpriteComponent leftAnimatedSpriteComponent = leftFlameActor.AddComponent<AnimatedSpriteComponent>("Textures/Flame", 7u, 1u);
            leftAnimatedSpriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Middle;
            leftFlameActor.AddComponent<TransformComponent>();
            leftFlameActor.AddComponent<ParentActorComponent>(actor, new Vector2f(-20.0f, 62.0f));

            TecnoCampusEngine.Get.Scene.CreateActor(leftFlameActor);

        }

        private void CreateAsteroidSpawner()
        {
            // Create the actor
            Actor actor = new Actor("Asteroid Spawner");

            // Create the spawner component to the actor that spawns
            ActorSpawnerComponent<ActorPrefab> spawnerComponent = actor.AddComponent<ActorSpawnerComponent<ActorPrefab>>();

            // Setup the ActorSpawnerComponent with the spawning area and the time to spawn
            spawnerComponent.m_MinTime = 2.0f;
            spawnerComponent.m_MaxTime = 5.0f;
            spawnerComponent.m_UseRandomPosition = false;

            // We need to call spawnerComponent.Reset() after updating the m_MinTime and m_MaxTime
            spawnerComponent.Reset();

            // Create the actor prefab
            ActorPrefab asteroidActorPrefab = new ActorPrefab("Asteroid Prefab");
            asteroidActorPrefab.AddComponent<TransformComponent>();
            asteroidActorPrefab.AddComponent<AsteroidComponent>();
            asteroidActorPrefab.AddComponent<AlwaysInWindowComponent>();
            asteroidActorPrefab.AddComponent<LifeComponent>(3);
            asteroidActorPrefab.AddComponent<ShipDestructorComponent>();
            SpriteComponent spriteComponent = asteroidActorPrefab.AddComponent<SpriteComponent>("Textures/Asteroid00");
            spriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Back;

            // Add the prefab to the spawner component
            spawnerComponent.AddActorPrefab(asteroidActorPrefab);

            // Add the spawner to the scene
            TecnoCampusEngine.Get.Scene.CreateActor(actor);
        }

        private void OnShipLifeDecreased()
        {
            --m_PendingLifesToGameOver;

            if (m_PendingLifesToGameOver <= 0)
            {
                ChangeState(EGameState.GameOver);
            }
            else 
            {
                ChangeState(EGameState.Restarting);
            }
        }

        private void ShowGameHUD()
        {
            GameHUDComponent gameHUDComponent = m_GameHUDActor.GetComponent<GameHUDComponent>();
            Debug.Assert(gameHUDComponent != null);

            gameHUDComponent.Show();
        }

        private void HideGameHUD()
        {
            GameHUDComponent gameHUDComponent = m_GameHUDActor.GetComponent<GameHUDComponent>();
            Debug.Assert(gameHUDComponent != null);

            gameHUDComponent.Hide();
        }

        private void DestroyShipFlames()
        {
            // Destroy flames
            List<ParentActorComponent> flames = TecnoCampusEngine.Get.Scene.GetAllComponents<ParentActorComponent>();
            foreach (ParentActorComponent flame in flames)
            {
                flame.Owner.Destroy();
            }
        }

        private void DestroyAsteroids()
        {
            List<AsteroidComponent> asteroids = TecnoCampusEngine.Get.Scene.GetAllComponents<AsteroidComponent>();
            foreach(AsteroidComponent asteroid in asteroids)
            {
                asteroid.Owner.Destroy();
            }

            var asteroidSpawners = TecnoCampusEngine.Get.Scene.GetAllComponents<ActorSpawnerComponent<ActorPrefab>>();
            foreach(var asteroidSpawner in asteroidSpawners)
            {
                asteroidSpawner.Owner.Destroy();
            }
        }

        private void ChangeState(EGameState _newState)
        {
            OnLeaveState(m_CurrentState);
            OnEnterState(_newState);

            m_CurrentState = _newState;
        }

        private void OnEnterState(EGameState _nextState)
        {
            switch (_nextState)
            {
                case EGameState.Start:
                    TecnoCampusEngine.Get.Window.KeyPressed += HandleKeyPressed;
                    m_TextHUDComponent.ShowText("Press Start");
                    m_PendingLifesToGameOver = 3;
                    break;
                case EGameState.Playing:
                    CreateAsteroidSpawner();
                    CreateShip();
                    ShowGameHUD();
                    break;
                case EGameState.Restarting:
                    TecnoCampusEngine.Get.Window.KeyPressed += HandleKeyPressed;
                    m_TextHUDComponent.ShowText("Restarting");
                    break;
                case EGameState.GameOver:
                    m_TextHUDComponent.ShowText("Game Over");
                    m_GameHUDActor.Destroy();
                    m_GameHUDActor = null;
                    m_GameOverTimer = 5.0f;
                    break;
            }
        }
        private void OnLeaveState(EGameState _previousState)
        {
            switch (_previousState)
            {
                case EGameState.Start:
                    TecnoCampusEngine.Get.Window.KeyPressed -= HandleKeyPressed;
                    m_TextHUDComponent.HideText();
                    CreateGameHUD();
                    break;
                case EGameState.Playing:
                    DestroyAsteroids();
                    DestroyShipFlames();
                    HideGameHUD();
                    break;
                case EGameState.Restarting:
                    TecnoCampusEngine.Get.Window.KeyPressed -= HandleKeyPressed;
                    m_TextHUDComponent.HideText();
                    break;
                case EGameState.GameOver:
                    m_TextHUDComponent.HideText();
                    break;
            }
        }

        private void HandleKeyPressed(object _sender, KeyEventArgs _eventKeyPressedArgs)
        {
            if (_eventKeyPressedArgs.Code == Keyboard.Key.Enter)
            {
                switch(m_CurrentState)
                {
                    case EGameState.Start:
                    case EGameState.Restarting:
                        ChangeState(EGameState.Playing);
                        break;
                }

            }
        }

        private void RestartGame()
        {
            ChangeState(EGameState.Start);
        }
    }
}
