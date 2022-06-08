using SFML.Graphics;
using SFML.System;
using System;
using TCEngine;

namespace TCGame
{
    public class TextHUDComponent : RenderComponent
    {
        private Font m_Font;
        private Text m_Text;

        private string m_Label;
        private bool m_IsVisible = false;

        public TextHUDComponent()
        {
            m_RenderLayer = ERenderLayer.HUD;

            m_Label = "";

            m_Font = TecnoCampusEngine.Get.Resources.GetFont("Fonts/neuro");
            m_Text = new Text(m_Label, m_Font);

            SetupTextProperties();
        }

        public TextHUDComponent(Font _font)
        {
            m_RenderLayer = ERenderLayer.HUD;

            m_Label = "";

            m_Font = _font;
            m_Text = new Text(m_Label, m_Font);
            
            SetupTextProperties();
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);
            
            if (m_IsVisible)
            {
                float s = (float)Math.Sin(TecnoCampusEngine.Get.Time * 2.0f) * 5.0f + 5.0f;
                byte alpha = (byte)(MathUtil.Lerp(0.2f, 1.0f, s) * 255.0f);
                m_Text.FillColor = new Color(alpha, alpha, alpha, alpha);
            }
        }

        public override void Draw(RenderTarget _target, RenderStates _states)
        {
            if(m_IsVisible)
            {
                base.Draw(_target, _states);

                _states.Transform *= Owner.GetWorldTransform();
                _target.Draw(m_Text, _states);
            }
        }

        private void SetupTextProperties()
        {
            const uint characterSize = 70u;
            m_Text.CharacterSize = characterSize;
            UpdateText();
        }

        private void UpdateText()
        {
            m_Text.DisplayedString = m_Label;

            FloatRect textSize = m_Text.GetLocalBounds();
            m_Text.Origin = new Vector2f(textSize.Width * 0.5f, textSize.Height * 0.5f);
        }

        public void ShowText(string _textToShow)
        {
            m_Label = _textToShow;
            m_IsVisible = true;
            UpdateText();
        }

        public void HideText()
        {
            m_IsVisible = false;
        }


        public override object Clone()
        {
            TextHUDComponent clonedComponent = new TextHUDComponent(m_Font);
            return clonedComponent;
        }
    }
}
