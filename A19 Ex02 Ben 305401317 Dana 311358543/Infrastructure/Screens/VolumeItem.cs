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
    public class VolumeItem : MenuItem
    {

        public event EventHandler<EventArgs> VolumeIncrease;
        public event EventHandler<EventArgs> VolumeDecrease;

        private Game m_Game;
        private SpriteFont m_Font;
        private ISoundMananger m_SoundMngr;
        private float m_Volume = 100;
        private float m_VolumeToRevert;
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
            if (IsActive)
            {
                colorToDraw = Color.Yellow;
            }
            else
            {
                colorToDraw = Color.White;
            }

           this.GameScreen.SpriteBatch.DrawString(this.m_Font, m_Volume.ToString(), this.Position + new Vector2(this.Width + 5, -3), colorToDraw);
            
        }


        private void OnVolumeIncrease(object sender, EventArgs args)
        {
            if (VolumeIncrease != null)
            {
                VolumeIncrease.Invoke(sender, args);
            }
        }

        private void OnVolumeDecrease(object sender, EventArgs args)
        {
            if (VolumeDecrease != null)
            {
                VolumeDecrease.Invoke(sender, args);
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (m_SoundMngr.IsGameSoundOn && this.IsActive)
            {
                if (this.GameScreen.InputManager.KeyPressed(Keys.PageUp))
                {
                    m_Volume = MathHelper.Clamp(m_Volume + k_VolumeAddition, 0, k_MaxVolume);

                    this.OnVolumeIncrease(this, null);
                }
                else if (this.GameScreen.InputManager.KeyPressed(Keys.PageDown))
                {
                    m_Volume = MathHelper.Clamp(m_Volume - k_VolumeAddition, 0, k_MaxVolume);
                    this.OnVolumeDecrease(this, null);
                }

                
            }
            base.Update(gameTime);
        }

    }


}