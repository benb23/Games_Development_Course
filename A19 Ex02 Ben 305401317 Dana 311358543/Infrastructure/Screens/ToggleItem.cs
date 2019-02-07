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
            get { return this.m_SelectedColor; }
            set { this.m_SelectedColor = value; }
        }

        public ToggleOption(GameScreen i_GameScreen, string i_AssetName, Rectangle i_Rec, Vector2 i_Position)
            : base(i_AssetName, i_GameScreen)
        {
            this.Position = i_Position;
            this.m_GameScreen = i_GameScreen;
            this.m_TextureRectangle = i_Rec;
        }

        public override void Draw(GameTime gameTime)
        {
            this.GameScreen.SpriteBatch.Draw(this.Texture, this.Position, this.m_TextureRectangle, this.m_TintColor);
        }
    }

    public class ToggleItem : MenuItem
    {
        private const int k_numOfOptions = 2;
        private int m_CurrToggleValue;
        private Texture2D m_SeperatorTexture;
        private Vector2 m_SeperatorPosition;
        private string m_SeperatorAsset = @"Screens\MainMenu\OptionsSeperator";
        private string m_OptionsAssetName;
        private List<ToggleOption> m_Options;
        private Texture2D m_OptionsTexture;
        private Game m_Game;

        public int ToggleValue
        {
            get { return this.m_CurrToggleValue; }
        }

        public event EventHandler<EventArgs> ToggleValueChanched;

        protected virtual void OnToggeleValueChanged(object sender, EventArgs args)
        {
            if (this.ToggleValueChanched != null)
            {
                this.ToggleValueChanched.Invoke(sender, args);
            }
        }

        public ToggleItem(string i_AssetName, string i_OptionsAssetName, GameScreen i_GameScreen, int i_ItemNumber) 
            : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_OptionsAssetName = i_OptionsAssetName;
            this.m_Game = i_GameScreen.Game;
        }

        public ToggleItem(string i_AssetName, string i_OptionsAssetName, GameScreen i_GameScreen, int i_ItemNumber, int i_DefualtVal)
            : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_CurrToggleValue = i_DefualtVal;
            this.m_OptionsAssetName = i_OptionsAssetName;
            this.m_Game = i_GameScreen.Game;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.m_SeperatorTexture = this.Game.Content.Load<Texture2D>(this.m_SeperatorAsset);
            this.m_OptionsTexture = this.Game.Content.Load<Texture2D>(this.m_OptionsAssetName);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Game.Window.ClientSizeChanged += this.updatePositionsAfterWindowSizeChanged;
        }

        private void updatePositionsAfterWindowSizeChanged(object sender, EventArgs e)
        {
            this.initOptionsPositions();
        }

        private void initOptionsPositions()
        {
            for (int i = 0; i < k_numOfOptions; i++)
            {
                this.m_Options[i].Position = new Vector2(this.Position.X + (this.Texture.Width + (5 + (i * (this.m_OptionsTexture.Width + this.m_SeperatorTexture.Width)))), this.Position.Y);
            }

            this.m_SeperatorPosition = new Vector2(this.m_Options[0].Position.X + this.m_OptionsTexture.Width, this.m_Options[0].Position.Y);
        }

        private void initOptions()
        {
            this.m_Options = new List<ToggleOption>(k_numOfOptions);

            for (int i = 0; i < k_numOfOptions; i++)
            {
                this.m_Options.Add(new ToggleOption(
                    this.GameScreen, 
                    this.m_OptionsAssetName,
                    new Rectangle(0, i * this.m_OptionsTexture.Height / 2, this.m_OptionsTexture.Width, this.m_OptionsTexture.Height / 2),
                    new Vector2(this.Position.X + this.Texture.Width + 5 + i * (this.m_OptionsTexture.Width + this.m_SeperatorTexture.Width), 
                    this.Position.Y)));
            }

            this.m_SeperatorPosition = new Vector2(this.m_Options[0].Position.X + this.m_OptionsTexture.Width, this.m_Options[0].Position.Y);
            this.m_Options[this.m_CurrToggleValue].TintColor = this.m_Options[this.m_CurrToggleValue].SelectedColor;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.GameScreen.SpriteBatch.Draw(this.m_SeperatorTexture, this.m_SeperatorPosition, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if(!this.m_Initialize)
            {
                this.initOptions();
                this.m_Initialize = true;
            }

            if (this.IsActive)
            {
                if (this.GameScreen.InputManager.KeyPressed(Keys.PageDown) || this.GameScreen.InputManager.KeyPressed(Keys.PageUp))
                {
                    this.UpdateToggleValue();
                }
            }

            base.Update(gameTime);  
        }

        private void UpdateToggleValue()
        {
            this.m_Options[this.m_CurrToggleValue].TintColor = Color.White;
            this.m_CurrToggleValue = (1 - this.m_CurrToggleValue) % k_numOfOptions;
            this.m_Options[this.m_CurrToggleValue].TintColor = this.m_Options[this.m_CurrToggleValue].SelectedColor;
            this.OnToggeleValueChanged(this, EventArgs.Empty);
        }  
    }
}
