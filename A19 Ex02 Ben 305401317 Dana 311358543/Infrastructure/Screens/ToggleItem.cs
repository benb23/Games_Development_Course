using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Infrastructure
{
    public class MenuOption : Sprite
    {
        private Rectangle m_TextureRectangle;

        public MenuOption(GameScreen i_GameScreen,string i_AssetName, Rectangle i_Rec , Vector2 i_Position) 
            : base(i_AssetName, i_GameScreen)
        {
            this.Position = i_Position;
            this.m_TextureRectangle = i_Rec;
        }
        public override void Draw(GameTime gameTime)
        {
            GameScreen.SpriteBatch.Draw(this.Texture , this.Position, this.m_TextureRectangle, this.m_TintColor);
        }
    }
    public class ToggleItem : MenuItem
    {
        private int m_numOfOptions;
        private Texture2D m_SeperatorTexture;
        private Vector2 m_SeperatorPosition;
        private string m_SeperatorAsset = @"Screens\MainMenu\OptionsSeperator";
        private string m_OptionsAssetName;
        private List<MenuOption> m_Options; 
        private List<Rectangle> m_OptionsRectangles;
        private Texture2D m_OptionsTexture;
        

        public ToggleItem(string i_AssetName,string i_OptionsAssetName, GameScreen i_GameScreen, int i_ItemNumber, int i_NumOfOptions) 
            : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_OptionsAssetName = i_OptionsAssetName;
            this.m_numOfOptions = i_NumOfOptions;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.m_SeperatorTexture = Game.Content.Load<Texture2D>(this.m_SeperatorAsset);
            this.m_OptionsTexture = Game.Content.Load<Texture2D>(this.m_OptionsAssetName);//??
            initOptions();
        }

        private void initOptions()
        {
            m_Options = new List<MenuOption>(m_numOfOptions);

            for (int i = 0; i < m_numOfOptions; i++)
            {
                m_Options.Add(new MenuOption(this.GameScreen, m_OptionsAssetName,
                    new Rectangle(0, i * m_OptionsTexture.Height / 2, m_OptionsTexture.Width, m_OptionsTexture.Height / 2),
                    new Vector2(this.Position.X + this.Texture.Width + 5 + i *( m_OptionsTexture.Width + m_SeperatorTexture.Width), this.Position.Y)));
            }
            m_SeperatorPosition = new Vector2(m_Options[0].Position.X + m_OptionsTexture.Width, m_Options[0].Position.Y);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.GameScreen.SpriteBatch.Draw(m_SeperatorTexture,m_SeperatorPosition ,Color.White);
        }
        public override void Update(GameTime gameTime)
        {
            if(IsActive)
            {
                if(this.GameScreen.InputManager.KeyPressed(Keys.PageDown))
                {

                }
                else if(this.GameScreen.InputManager.KeyPressed(Keys.PageUp))
                {

                }
            }
            base.Update(gameTime);  
        }
    }
}
