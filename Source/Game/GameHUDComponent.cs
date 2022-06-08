using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using TCEngine;

namespace TCGame
{
    public class GameHUDComponent : RenderComponent
    {
        private Text m_PointsText;
        private List<Sprite> m_LifeSprites;
        private Sprite m_HUDSprite;
        private bool m_IsVisible = false;

        private int m_Lifes;
        private int m_Points;

        public int Lifes
        {
            get => m_Lifes;
            set 
            {
                m_Lifes = value;
                UpdateLifeSprites();
            }
        }

        public int Points
        {
            get => m_Points;
            set
            {
                m_Points = value;
                UpdatePoints();
            }
        }

        public GameHUDComponent(int _lifes)
        {
            m_Points = 0;
            m_Lifes = _lifes;

            m_RenderLayer = ERenderLayer.HUD;

            m_HUDSprite = new Sprite(TecnoCampusEngine.Get.Resources.GetTexture("Textures/HUD"));
            m_PointsText = new Text(m_Points.ToString(), TecnoCampusEngine.Get.Resources.GetFont("Fonts/neuro"));
            m_PointsText.CharacterSize = 50;
            m_PointsText.Position = new Vector2f(100.0f, 50.0f);
            m_LifeSprites = new List<Sprite>();

            UpdatePoints();
            UpdateLifeSprites();
        }

        public override void Draw(RenderTarget _target, RenderStates _states)
        {
            if(m_IsVisible)
            {
                base.Draw(_target, _states);

                _states.Transform *= Owner.GetWorldTransform();
                _target.Draw(m_HUDSprite, _states);
                _target.Draw(m_PointsText, _states);
                for (int i = 0; i < m_Lifes; ++i)
                {
                    _target.Draw(m_LifeSprites[i], _states);
                }
            }
        }

        private void UpdatePoints()
        {
            m_PointsText.DisplayedString = m_Points.ToString("00000");
        }

        private void UpdateLifeSprites()
        {
            m_LifeSprites.Clear();

            for (int i = 0; i < 3; ++i)
            {
                var lifeSprite = new Sprite(TecnoCampusEngine.Get.Resources.GetTexture("Textures/Life"));
                lifeSprite.Position = new Vector2f(190.0f + i * 20.0f, 15.0f);
                m_LifeSprites.Add(lifeSprite);
            }
        }

        public void Show()
        {
            m_IsVisible = true;
        }

        public void Hide()
        {
            m_IsVisible = false;
        }


        public override object Clone()
        {
            GameHUDComponent clonedComponent = new GameHUDComponent(m_Lifes);
            return clonedComponent;
        }
    }
}
