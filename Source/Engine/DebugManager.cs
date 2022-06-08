using SFML.Graphics;
using SFML.System;

namespace TCEngine
{
    public class DebugManager : Drawable
    {
        private LineDrawer m_LineDrawer;
        private Text[] m_Texts;
        private CircleShape[] m_Circles;
        private uint m_CurrentText;
        private uint m_CurrentCircle;
        private Font m_Font;

        public void Init()
        {
            m_CurrentText = 0;
            m_CurrentCircle = 0;

            m_Font = TecnoCampusEngine.Get.Resources.GetFont("Fonts/georgia");
            m_LineDrawer = new LineDrawer(2000);
            m_Texts = new Text[100];
            m_Circles = new CircleShape[5000];

            for (int i = 0; i < m_Texts.Length; ++i)
            {
                m_Texts[i] = new Text(" ", m_Font);
            }

            for (int i = 0; i < m_Circles.Length; ++i)
            {
                m_Circles[i] = new CircleShape();
            }
        }

        public void DeInit()
        {
            for (int i = 0; i < m_Texts.Length; ++i)
            {
                m_Texts[i].Dispose();
            }

            for (int i = 0; i < m_Circles.Length; ++i)
            {
                m_Circles[i].Dispose();
            }

            m_Texts = null;
            m_Circles = null;
        }

        public void Update(float _deltaSeconds)
        {
            m_LineDrawer.ClearLines();
            m_CurrentText = 0;
            m_CurrentCircle = 0;
        }

        public void Circle(Vector2f _position, float _radius, Color _color, float _thickness = 2.0f)
        {
            CircleShape circle = m_Circles[m_CurrentCircle];

            circle.FillColor = Color.Transparent;
            circle.OutlineColor = _color;
            circle.OutlineThickness = _thickness;
            circle.Radius = _radius;
            circle.Position = _position - new Vector2f(_radius, _radius);

            ++m_CurrentCircle;
        }


        public void Line(Vector2f _start, Vector2f _end, Color _color, float _thickness)
        {
            m_LineDrawer.AddLine(_start, _end, _thickness, _color);
        }

        public void Box(FloatRect _rectangle, Color _color, float _thickness = 2.0f)
        {
            Vector2f topLeftCorner = new Vector2f(_rectangle.Left, _rectangle.Top);
            Vector2f topRightCorner = new Vector2f(_rectangle.Left + _rectangle.Width, _rectangle.Top);
            Vector2f bottomLeftCorner = new Vector2f(_rectangle.Left, _rectangle.Top + _rectangle.Height);
            Vector2f bottomRightCorner = new Vector2f(_rectangle.Left + _rectangle.Width, _rectangle.Top + _rectangle.Height);

            m_LineDrawer.AddLine(topLeftCorner, topRightCorner, _thickness, _color);
            m_LineDrawer.AddLine(topLeftCorner, bottomLeftCorner, _thickness, _color);
            m_LineDrawer.AddLine(topRightCorner, bottomRightCorner, _thickness, _color);
            m_LineDrawer.AddLine(bottomLeftCorner, bottomRightCorner, _thickness, _color);
        }

        public void Arrow(Vector2f _start, Vector2f _offset, Color _color, float _thickness = 2.0f)
        {
            Vector2f end = _start + _offset;
            Line(_start, end, _color, _thickness);

            const float arrowAngle = 20.0f;
            const float arrowDist = 20.0f;

            Vector2f p0 = end + (_start - end).Normal().Rotate(arrowAngle) * arrowDist;
            Vector2f p1 = end + (_start - end).Normal().Rotate(-arrowAngle) * arrowDist;

            Line(end, p0, _color, _thickness);
            Line(end, p1, _color, _thickness);
        }

        public void Label(Vector2f _position, string _displayedString, Color _color, uint _size = 25)
        {
            Text text = m_Texts[m_CurrentText];

            text.CharacterSize = _size;
            text.FillColor = _color;
            text.DisplayedString = _displayedString;
            text.Position = _position;

            text.Font = m_Font;
            m_CurrentText = (uint)(m_CurrentText + 1 % m_Texts.Length);
        }

        public void Label(Vector2f _position, Vector2f _offset, string _displayedString, Color _color, uint _size = 25)
        {
            Label(_position + _offset, _displayedString, _color, _size);
        }

        public void Draw(RenderTarget _target, RenderStates _states)
        {
            _target.Draw(m_LineDrawer, _states);

            for (uint i = 0; i < m_CurrentText; ++i)
            {
                _target.Draw(m_Texts[i], _states);
            }

            for (uint i = 0; i < m_CurrentCircle; ++i)
            {
                _target.Draw(m_Circles[i], _states);
            }
        }
    }
}
