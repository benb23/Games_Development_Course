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
        public event EventHandler<EventArgs> VolumeChanged;

        private Game m_Game;
        private string m_NumbersAsset;
        //private Texture2D m_VolumeTexture2;
        private ToggleOption m_VolumeTexture;
        private ISoundSettingsManager m_SoundSettingsMngr;
        private float m_Volume;
        private float m_VolumeToRevert;


        public float Volume
        {
            get { return m_Volume; }
            set
            {
                m_Volume = value;
                //if (VolumeChanged != null)
                //{
                //    VolumeChanged.Invoke(this, null);
                //}
            }
        }

        public VolumeItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber, string i_NumbersAsset) : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_NumbersAsset = i_NumbersAsset;
            this.m_Game = i_GameScreen.Game;
            this.m_SoundSettingsMngr = this.m_Game.Services.GetService(typeof(ISoundSettingsManager)) as ISoundSettingsManager;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.m_VolumeTexture.Texture = Game.Content.Load<Texture2D>(this.m_NumbersAsset);
            initVolumePosition();
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
            //this.VolumeChanged += VolumeItem_VolumeChanged;

            this.m_SoundSettingsMngr.ToggleGameSoundChanched += volumeItem_ToggleGameSoundChanched;
        }

        protected override void InitSourceRectangle()
        {
            this.m_VolumeTexture.SourceRectangle = new Rectangle(0, 0, (int)this.m_WidthBeforeScale / 10, (int)this.m_HeightBeforeScale);
        }



        private void volumeItem_ToggleGameSoundChanched(object sender, EventArgs e)
        {
            if (m_SoundSettingsMngr.IsGameSoundOn)
            {
                this.Volume = m_VolumeToRevert;
            }
            else
            {
                m_VolumeToRevert = m_Volume;
                this.Volume = 0f;
            }
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            initVolumePosition();
        }

        // TODO: implement
        private void initVolumePosition()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (m_Volume == 1f)
            {
                
            }
            else
            {
                

            }

            //this.GameScreen.SpriteBatch.Draw(m_SeperatorTexture, m_SeperatorPosition, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}