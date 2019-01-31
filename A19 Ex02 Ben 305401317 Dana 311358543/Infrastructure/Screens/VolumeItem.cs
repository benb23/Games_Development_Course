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
        private string m_NumbersAsset;
        private SpriteFont m_Font;
        private ISoundMananger m_SoundMngr;
        private float m_Volume = 100;
        private float m_VolumeToRevert;


        public VolumeItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber) : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
           // this.m_NumbersAsset = i_NumbersAsset;
            this.m_Game = i_GameScreen.Game;
            this.m_SoundMngr = this.m_Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.m_Font = this.Game.Content.Load<SpriteFont>(@"Fonts\ComicSansMS");
            //initVolumePosition();
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
            //this.m_SoundSettingsMngr.ToggleGameSoundChanched += new EventHandler<EventArgs>(volumeItem_ToggleGameSoundChanched);
        }


        private void volumeItem_ToggleGameSoundChanched(object sender, EventArgs e)
        {
            if (m_SoundMngr.IsGameSoundOn)
            {
                this.m_Volume = m_VolumeToRevert;
            }
            else
            {
                m_VolumeToRevert = m_Volume;
                this.m_Volume = 0f;
            }
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            //initVolumePosition();
        }

        // TODO: implement
        private void initVolumePosition()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

           this.GameScreen.SpriteBatch.DrawString(this.m_Font, m_Volume.ToString(), this.Position + new Vector2(this.Width + 5, 0), Color.Green);

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
                    m_Volume = MathHelper.Clamp(m_Volume + 10, 0, 100);

                    this.OnVolumeIncrease(this, null);
                }
                else if (this.GameScreen.InputManager.KeyPressed(Keys.PageDown))
                {
                    m_Volume = MathHelper.Clamp(m_Volume - 10, 0, 100);
                    this.OnVolumeDecrease(this, null);
                }

                
            }
            base.Update(gameTime);
        }

    }

}