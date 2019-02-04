using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure
{
    public class VolumeItem : MenuItem
    {
        public event EventHandler<EventArgs> IncreaseVolumeButtonClicked;

        public event EventHandler<EventArgs> DecreaseVolumeButtonClicked;

        private Game m_Game;
        private SpriteFont m_Font;
        private ISoundMananger m_SoundMngr;
        private float m_Volume = 100;
        private const int k_MaxVolume = 100;
        private const int k_VolumeAddition = 10;

        public VolumeItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber) : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_Game = i_GameScreen.Game;
            this.m_SoundMngr = this.m_Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.m_Font = this.Game.Content.Load<SpriteFont>(@"Fonts\ERASDEMI");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Color colorToDraw;
            if (this.IsActive)
            {
                colorToDraw = Color.Yellow;
            }
            else
            {
                colorToDraw = Color.White;
            }

           this.GameScreen.SpriteBatch.DrawString(this.m_Font, this.m_Volume.ToString(), this.Position + new Vector2(this.Width + 5, -3), colorToDraw);            
        }

        private void OnIncreaseVolumeButtonClicked(object sender, EventArgs args)
        {
            if (this.IncreaseVolumeButtonClicked != null)
            {
                this.IncreaseVolumeButtonClicked.Invoke(sender, args);
            }
        }

        private void OnDecreaseVolumeButtonClicked(object sender, EventArgs args)
        {
            if (this.DecreaseVolumeButtonClicked != null)
            {
                this.DecreaseVolumeButtonClicked.Invoke(sender, args);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.m_SoundMngr.IsGameSoundOn && this.IsActive)
            {
                if (this.GameScreen.InputManager.KeyPressed(Keys.PageUp))
                {
                    this.m_Volume = MathHelper.Clamp(this.m_Volume + k_VolumeAddition, 0, k_MaxVolume);

                    this.OnIncreaseVolumeButtonClicked(this, null);
                }
                else if (this.GameScreen.InputManager.KeyPressed(Keys.PageDown))
                {
                    this.m_Volume = MathHelper.Clamp(this.m_Volume - k_VolumeAddition, 0, k_MaxVolume);
                    this.OnIncreaseVolumeButtonClicked(this, null);
                }
            }

            base.Update(gameTime);
        }
    }
}