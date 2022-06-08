using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TCEngine
{
    public class AnimatedSpriteComponent : RenderComponent
    {
        private Sprite m_Sprite;
        private List<IntRect> m_Frames = new List<IntRect>();
        private Vertex[] m_Vertices = new Vertex[4];
        private float m_FrameTime;
        private bool m_Loop;
        
        private float m_CurrentTime;
        private int m_CurrentFrame;
        private uint m_NumColumns;
        private uint m_NumRows;

        public bool Loop
        {
            get => m_Loop;
            set => m_Loop = value;
        }

        public float FrameTime
        {
            get => m_FrameTime;
            set => m_FrameTime = value;
        }


        public float AnimationTime
        {
            get => m_FrameTime * m_Frames.Count;
        }

        public AnimatedSpriteComponent(string _textureName, uint _numColumns, uint _numRows) 
        {
            m_Sprite = new Sprite(TecnoCampusEngine.Get.Resources.GetTexture(_textureName));
            m_NumColumns = _numColumns;
            m_NumRows = _numRows;

            Initialize();
        }

        public AnimatedSpriteComponent(Texture _texture, uint _numColumns, uint _numRows)
        {
            m_Sprite = new Sprite(_texture);
            m_NumColumns = _numColumns;
            m_NumRows = _numRows;

            Initialize();
        }

        private void Initialize()
        {
            m_Loop = true;
            m_FrameTime = 0.2f;
            m_CurrentFrame = 0;

            IntRect frame;
            frame.Width = (int)(m_Sprite.Texture.Size.X / m_NumColumns);
            frame.Height = (int)(m_Sprite.Texture.Size.Y / m_NumRows);
            for (int r = 0; r < m_NumRows; ++r)
            {
                for (int c = 0; c < m_NumColumns; ++c)
                {
                    frame.Top = r * frame.Width;
                    frame.Left = c * frame.Width;
                    m_Frames.Add(frame);
                }
            }

            SetFrame(m_CurrentFrame);
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            m_CurrentTime += _dt;
            if (m_CurrentTime >= m_FrameTime)
            {
                m_CurrentTime = m_CurrentTime % m_FrameTime;

                if (m_Loop)
                {
                    m_CurrentFrame = (m_CurrentFrame + 1) % m_Frames.Count;
                }
                else
                {
                    m_CurrentFrame = Math.Min(m_CurrentFrame + 1, m_Frames.Count - 1);
                }

                SetFrame(m_CurrentFrame);
            }
        }

        public override void DebugDraw()
        {
            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            if(transformComponent != null)
            {
                TecnoCampusEngine.Get.DebugManager.Label(transformComponent.Transform.Position, m_CurrentFrame.ToString(), Color.Blue);

                IntRect currentFrame = m_Frames[m_CurrentFrame];
                TecnoCampusEngine.Get.DebugManager.Box(transformComponent.Transform.Transform.TransformRect(new FloatRect(0, 0, currentFrame.Width, currentFrame.Height)), Color.Red);
            }
        }

        public override void Draw(RenderTarget _target, RenderStates _states)
        {
            Debug.Assert(m_Sprite != null);

            _states.Transform *= Owner.GetWorldTransform();
            _states.Texture = m_Sprite.Texture;

            base.Draw(_target, _states);
            _target.Draw(m_Vertices, 0, 4, PrimitiveType.Quads, _states);
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();

            Owner.GetGlobalBoundsEvent += () =>
            {
                Debug.Assert(m_Sprite != null);
                Transform worldTransform = Owner.GetWorldTransform();
                IntRect currentFrame = m_Frames[m_CurrentFrame];
                return worldTransform.TransformRect(new FloatRect(0, 0, currentFrame.Width, currentFrame.Height));
            };

            Owner.GetLocalBoundsEvent += () =>
            {
                Debug.Assert(m_Sprite != null);
                Transform worldTransform = Owner.GetWorldTransform();
                IntRect currentFrame = m_Frames[m_CurrentFrame];
                return worldTransform.TransformRect(new FloatRect(0, 0, currentFrame.Width, currentFrame.Height));
            };

            Center();
        }

        public void Center()
        {
            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            Debug.Assert(transformComponent != null, "Trying to change the origin of the actor " + Owner.Name + " without a TransformComponent");
            FloatRect localBounds = GetLocalBounds();
            transformComponent.Transform.Origin = new Vector2f(localBounds.Width * 0.5f, localBounds.Height * 0.5f);
        }

        public FloatRect GetLocalBounds()
        {
            IntRect frame = m_Frames[m_CurrentFrame];
            return new FloatRect(0.0f, 0.0f, frame.Width, frame.Height);
        }

        private void SetFrame(int _frame)
        {
            if (_frame < m_Frames.Count)
            {
                IntRect frameRect = m_Frames[_frame];

                m_Vertices[0].Position = new Vector2f(0.0f, 0.0f);
                m_Vertices[1].Position = new Vector2f(0.0f, frameRect.Height);
                m_Vertices[2].Position = new Vector2f(frameRect.Width, frameRect.Height);
                m_Vertices[3].Position = new Vector2f(frameRect.Width, 0.0f);

                float left = frameRect.Left;
                float right = left + frameRect.Width;
                float top = frameRect.Top;
                float bottom = top + frameRect.Height;

                m_Vertices[0].TexCoords = new Vector2f(left, top);
                m_Vertices[1].TexCoords = new Vector2f(left, bottom);
                m_Vertices[2].TexCoords = new Vector2f(right, bottom);
                m_Vertices[3].TexCoords = new Vector2f(right, top);

                m_Vertices[0].Color = Color.White;
                m_Vertices[1].Color = Color.White;
                m_Vertices[2].Color = Color.White;
                m_Vertices[3].Color = Color.White;
            }
        }

        public override object Clone()
        {
            AnimatedSpriteComponent clonedComponent = new AnimatedSpriteComponent(m_Sprite.Texture, m_NumColumns, m_NumRows);
            clonedComponent.m_Loop = m_Loop;
            clonedComponent.m_FrameTime = m_FrameTime;
            clonedComponent.m_RenderLayer = m_RenderLayer;

            return clonedComponent;
        }
    }
}
