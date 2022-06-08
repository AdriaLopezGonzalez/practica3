using SFML.Graphics;
using SFML.System;

namespace TCEngine
{
    public class LineDrawer : Drawable
    {
        private Vertex[] m_Vertices;
        private RenderStates m_RenderState;
        private uint m_CurrentLine;

        public LineDrawer(uint _maxLines)
        {
            m_Vertices = new Vertex[_maxLines * 4];
            m_RenderState = new RenderStates(TecnoCampusEngine.Get.Resources.GetTexture("Textures/Pixel"));
            m_CurrentLine = 0;

            for (int i = 0; i < _maxLines * 4; ++i)
            {
                m_Vertices[i] = new Vertex();
            }
        }

        public void AddLine(Vector2f _p0, Vector2f _p1, float _thickness, Color _color)
        {
            Vector2f offset = MathUtil.Rotate((_p0 - _p1), 90.0f).Normal() * _thickness;

            m_Vertices[m_CurrentLine + 0].Position = _p0 - offset;
            m_Vertices[m_CurrentLine + 1].Position = _p0 + offset;
            m_Vertices[m_CurrentLine + 2].Position = _p1 + offset;
            m_Vertices[m_CurrentLine + 3].Position = _p1 - offset;

            m_Vertices[m_CurrentLine + 0].Color = _color;
            m_Vertices[m_CurrentLine + 1].Color = _color;
            m_Vertices[m_CurrentLine + 2].Color = _color;
            m_Vertices[m_CurrentLine + 3].Color = _color;

            m_CurrentLine += 4;
        }

        public void ClearLines()
        {
            m_CurrentLine = 0;
        }

        public void Draw(RenderTarget _target, RenderStates _states)
        {
            m_RenderState.Transform = _states.Transform;
            _target.Draw(m_Vertices, 0, m_CurrentLine, PrimitiveType.Quads, m_RenderState);
        }
    }
}
