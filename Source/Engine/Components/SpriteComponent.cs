using SFML.Graphics;
using SFML.System;
using System.Diagnostics;

namespace TCEngine
{
    public class SpriteComponent : RenderComponent
    {
        private Sprite m_Sprite;

        public SpriteComponent(string _textureName)
        {
            m_Sprite = new Sprite(TecnoCampusEngine.Get.Resources.GetTexture(_textureName));
        }

        public SpriteComponent(Texture _texture)
        {
            m_Sprite = new Sprite(_texture);
        }

        public Sprite Sprite
        {
            get => m_Sprite;
            set => m_Sprite = value;
        }

        public override void Draw(RenderTarget _target, RenderStates _states)
        {
            Debug.Assert(m_Sprite != null);

            _states.Transform *= Owner.GetWorldTransform();

            base.Draw(_target, _states);
            _target.Draw(m_Sprite, _states);
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();

            Owner.GetGlobalBoundsEvent += () =>
            {
                Debug.Assert(m_Sprite != null);
                Transform worldTransform = Owner.GetWorldTransform();
                return worldTransform.TransformRect(m_Sprite.GetGlobalBounds());
            };

            Owner.GetLocalBoundsEvent += () =>
            {
                Debug.Assert(m_Sprite != null);
                Transform worldTransform = Owner.GetWorldTransform();
                return worldTransform.TransformRect(m_Sprite.GetLocalBounds());
            };

            Center();
        }

        public void ChangeSprite(string _textureName)
        {
            Sprite newSprite = new Sprite(TecnoCampusEngine.Get.Resources.GetTexture(_textureName));
            Debug.Assert(m_Sprite.GetLocalBounds() == newSprite.GetLocalBounds());

            m_Sprite = newSprite;
        }


        public void Center()
        {
            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            if(transformComponent != null)
            {
                FloatRect localBounds = m_Sprite.GetLocalBounds();
                transformComponent.Transform.Origin = new Vector2f(localBounds.Width * 0.5f, localBounds.Height * 0.5f);
            }
        }

        public override void DebugDraw()
        {
            Debug.Assert(m_Sprite != null);

            base.DebugDraw();
            Transform worldTransform = Owner.GetWorldTransform();
            TecnoCampusEngine.Get.DebugManager.Box(worldTransform.TransformRect(m_Sprite.GetLocalBounds()), Color.Blue);
            TecnoCampusEngine.Get.DebugManager.Box(worldTransform.TransformRect(m_Sprite.GetGlobalBounds()), Color.Red);
        }

        public override object Clone()
        {
            SpriteComponent clonedComponent = new SpriteComponent(m_Sprite.Texture);
            clonedComponent.m_RenderLayer = m_RenderLayer;

            return clonedComponent;
        }
    }
}