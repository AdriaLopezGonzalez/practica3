using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TCEngine
{
    public class ActorSpawnerComponent<T> : BaseComponent where T : ActorPrefab
    {
        public float m_MinTime = 2.0f;
        public float m_MaxTime = 3.0f;
        public Vector2f m_MinPosition;
        public Vector2f m_MaxPosition;
        public bool m_UseRandomPosition;

        private List<T> m_ActorsToSpawn;
        private uint m_SpawnedActors;
        private float m_CoolDown;

        public ActorSpawnerComponent()
        {
            m_ActorsToSpawn = new List<T>();
            m_SpawnedActors = 0;
            m_CoolDown = 0.0f;
            m_UseRandomPosition = true;

            m_MinPosition = new Vector2f(0.0f, 0.0f);
            m_MaxPosition = new Vector2f(TecnoCampusEngine.WINDOW_WIDTH, TecnoCampusEngine.WINDOW_HEIGHT);
        }

        public void AddActorPrefab(T _actorToSpawn)
        {
            m_ActorsToSpawn.Add(_actorToSpawn);
        }

        public void Reset()
        {
            Random randomGenerator = new Random();
            float timeRate = (float)randomGenerator.NextDouble();

            m_CoolDown = m_MaxTime * timeRate + (1.0f - timeRate) * m_MinTime;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            m_CoolDown -= _dt;
            if (m_CoolDown < 0.0f)
            {
                SpawnActor();
                Reset();
            }
        }

        public void SpawnActor()
        {
            Debug.Assert(m_ActorsToSpawn.Count > 0, "There are no prefabs assigned. Nothing can be spawned!");

            Random randomGenerator = new Random();
            int actorIndexToSpawn = randomGenerator.Next(m_ActorsToSpawn.Count);
            ActorPrefab actorToSpawn = m_ActorsToSpawn[actorIndexToSpawn];

            Actor actor = new Actor(actorToSpawn.PrefabName + "_" + m_SpawnedActors);
            foreach (BaseComponent component in actorToSpawn.Components)
            {
                Debug.Assert(component.Owner == null, "Trying to spawn an actor from prefab which components already have an actor owner");
                BaseComponent clonedComponent = component.Clone() as BaseComponent;
                actor.AddComponentUnsafe(clonedComponent);
            }

            TecnoCampusEngine.Get.Scene.CreateActor(actor);
            ++m_SpawnedActors;

            if (m_UseRandomPosition)
            {
                TransformComponent transformComponent = actor.GetComponent<TransformComponent>();
                if (transformComponent != null)
                {
                    float xPositionRatio = (float)randomGenerator.NextDouble();
                    float yPositionRatio = (float)randomGenerator.NextDouble();

                    float positionX = m_MaxPosition.X * xPositionRatio + (1.0f - xPositionRatio) * m_MinPosition.X;
                    float positionY = m_MaxPosition.Y * yPositionRatio + (1.0f - yPositionRatio) * m_MinPosition.Y;
                    transformComponent.Transform.Position = new Vector2f(positionX, positionY);
                }
            }
        }

        public override void DebugDraw()
        {
            base.DebugDraw();
            TecnoCampusEngine.Get.DebugManager.Label(new Vector2f(50f, 50f), "CoolDown: " + m_CoolDown.ToString("F2"), Color.Red);
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }
    }
}
