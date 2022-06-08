using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace TCEngine
{
    public class Scene : Drawable
    {
        private List<BaseComponent> m_Components = new List<BaseComponent>();
        private List<BaseComponent> m_ComponentsToAdd = new List<BaseComponent>();
        private List<BaseComponent> m_ComponentsToDestroy = new List<BaseComponent>();

        private List<Actor> m_Actors = new List<Actor>();
        private List<Actor> m_ActorsToDestroy = new List<Actor>();
        private List<Actor> m_ActorsToAdd = new List<Actor>();

        private bool m_ActorsChanged = false;

        public void RegisterComponentForUpdate(BaseComponent _component)
        {
            m_ComponentsToAdd.Add(_component);
            m_ActorsChanged = true;
        }

        public void UnregisterComponentFromUpdate(BaseComponent _component)
        {
            m_ComponentsToAdd.Remove(_component);
            m_ComponentsToDestroy.Add(_component);
            m_ActorsChanged = true;
        }

        public void CreateActor(Actor _actor)
        {
            m_ActorsToAdd.Add(_actor);
            _actor.OnCreated();
            m_ActorsChanged = true;
        }

        public void Destroy(Actor _actor)
        {
            m_ActorsToAdd.Remove(_actor);
            m_ActorsToDestroy.Add(_actor);
            _actor.OnDestroyed();
            m_ActorsChanged = true;
        }

        public void Update(float _dt)
        {
            m_Actors.RemoveAll(m_ActorsToDestroy.Contains);
            m_ActorsToDestroy.Clear();

            m_Actors.AddRange(m_ActorsToAdd);
            m_ActorsToAdd.Clear();

            m_Components.RemoveAll(m_ComponentsToDestroy.Contains);
            m_Components.AddRange(m_ComponentsToAdd);
            m_ComponentsToAdd.Clear();

            if( m_ActorsChanged)
            {
                m_Components.Sort((componentA, componentB) =>
                {
                    BaseComponent.EComponentUpdateCategory updateCategoryA = componentA.GetUpdateCategory();
                    BaseComponent.EComponentUpdateCategory updateCategoryB = componentB.GetUpdateCategory();

                    return updateCategoryA.CompareTo(updateCategoryB);
                });
            }

            foreach (BaseComponent component in m_Components)
            {
                component.Update(_dt);
            }
        }

        public void Draw(RenderTarget _target, RenderStates _states)
        {
            List<RenderComponent> renderComponents = m_Components.FindAll(x => x is RenderComponent).ConvertAll(x => x as RenderComponent);

            int numRenderLayers = Enum.GetValues(typeof(RenderComponent.ERenderLayer)).Length;
            for(int i = 0; i < numRenderLayers; ++i)
            {
                RenderComponent.ERenderLayer renderLayer = (RenderComponent.ERenderLayer)i;
                List<RenderComponent> renderComponentsInLayer = renderComponents.FindAll(x => x.m_RenderLayer == renderLayer);
                renderComponentsInLayer.ForEach(x => x.Draw(_target, _states));
            }
        }

        public List<T> GetAllComponents<T>() where T : BaseComponent
        {
            return m_Components.FindAll(x => x is T).ConvertAll(x => x as T);
        }

        public T GetRandomComponent<T>() where T : BaseComponent
        {
            List<T> components = GetAllComponents<T>();
            Random randomGenerator = new Random();
            return (components.Count > 0) ? components[randomGenerator.Next(components.Count)] : null;
        }

        public List<Actor> GetAllActors()
        {
            return m_Actors;
        }

        public void EndFrame()
        {
            m_ActorsChanged = false;
        }

    }
}
