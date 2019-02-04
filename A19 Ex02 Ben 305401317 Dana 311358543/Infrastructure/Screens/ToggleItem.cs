using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure
{
    public class ToggleOption : Sprite
    {
        private Rectangle m_TextureRectangle;
        private Color m_SelectedColor = Color.Yellow;

        public Color SelectedColor
        {
            get { return m_SelectedColor; }
            set { m_SelectedColor = value; }
        }


        public ToggleOption(GameScreen i_GameScreen,string i_AssetName, Rectangle i_Rec, Vector2 i_Position) 
            : base(i_AssetName, i_GameScreen)
        {
            this.Position = i_Position;
            this.m_GameScreen = i_GameScreen;
            this.m_TextureRectangle = i_Rec;
        }

        public override void Draw(GameTime gameTime)
        {
            GameScreen.SpriteBatch.Draw(this.Texture , this.Position, this.m_TextureRectangle, this.m_TintColor);
        }
    }

    public class ToggleItem : MenuItem
    {
        private int m_CurrToggleValue;
        private const int k_numOfOptions = 2;
        private Texture2D m_SeperatorTexture;
        private Vector2 m_SeperatorPosition;
        private string m_SeperatorAsset = @"Screens\MainMenu\OptionsSeperator";
        private string m_OptionsAssetName;
        private List<ToggleOption> m_Options;
        private Texture2D m_OptionsTexture;
        private Game m_Game;

        public int ToggleValue
        {
            get { return m_CurrToggleValue; }
        }

        public event EventHandler<EventArgs> ToggleValueChanched;

        protected virtual void OnToggeleValueChanged(object sender, EventArgs args)
        {
            if (ToggleValueChanched != null)
            {
                ToggleValueChanched.Invoke(sender, args);
            }
        }

        public ToggleItem(string i_AssetName,string i_OptionsAssetName, GameScreen i_GameScreen, int i_ItemNumber) 
            : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_OptionsAssetName = i_OptionsAssetName;
            this.m_Game = i_GameScreen.Game;
        }

        public ToggleItem(string i_AssetName, string i_OptionsAssetName, GameScreen i_GameScreen, int i_ItemNumber, int i_DefualtVal)
            : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            m_CurrToggleValue = i_DefualtVal;
            this.m_OptionsAssetName = i_OptionsAssetName;
            this.m_Game = i_GameScreen.Game;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.m_SeperatorTexture = Game.Content.Load<Texture2D>(this.m_SeperatorAsset);
            this.m_OptionsTexture = Game.Content.Load<Texture2D>(this.m_OptionsAssetName);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Game.Window.ClientSizeChanged += updatePositionsAfterWindowSizeChanged;
        }

        private void updatePositionsAfterWindowSizeChanged(object sender, EventArgs e)
        {
            initOptionsPositions();
        }

        private void initOptionsPositions()
        {
            for (int i = 0; i < k_numOfOptions; i++)
            {
                m_Options[i].Position = new Vector2(this.Position.X + this.Texture.Width + 5 + i *( m_OptionsTexture.Width + m_SeperatorTexture.Width), this.Position.Y);
            }
            m_SeperatorPosition = new Vector2(m_Options[0].Position.X + m_OptionsTexture.Width, m_Options[0].Position.Y);
        }

        private void initOptions()
        {
            m_Options = new List<ToggleOption>(k_numOfOptions);

            for (int i = 0; i < k_numOfOptions; i++)
            {
                m_Options.Add(new ToggleOption(this.GameScreen, m_OptionsAssetName,
                    new Rectangle(0, i * m_OptionsTexture.Height / 2, m_OptionsTexture.Width, m_OptionsTexture.Height / 2),
                    new Vector2(this.Position.X + this.Texture.Width + 5 + i * (m_OptionsTexture.Width + m_SeperatorTexture.Width), this.Position.Y)));
            }
            m_SeperatorPosition = new Vector2(m_Options[0].Position.X + m_OptionsTexture.Width, m_Options[0].Position.Y);
            m_Options[m_CurrToggleValue].TintColor = m_Options[m_CurrToggleValue].SelectedColor;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.GameScreen.SpriteBatch.Draw(m_SeperatorTexture,m_SeperatorPosition ,Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if(!m_Initialize)
            {
                initOptions();
                m_Initialize = true;
            }

            if (IsActive)
            {
                if (this.GameScreen.InputManager.KeyPressed(Keys.PageDown) || this.GameScreen.InputManager.KeyPressed(Keys.PageUp))
                {
                    UpdateToggleValue();
                }
            }
            base.Update(gameTime);  
        }

        private void UpdateToggleValue()
        {
            m_Options[m_CurrToggleValue].TintColor = Color.White;
            m_CurrToggleValue = (1 - m_CurrToggleValue) % k_numOfOptions;
            m_Options[m_CurrToggleValue].TintColor = m_Options[m_CurrToggleValue].SelectedColor;
            OnToggeleValueChanged(this, EventArgs.Empty);
        }

        
    }
}
