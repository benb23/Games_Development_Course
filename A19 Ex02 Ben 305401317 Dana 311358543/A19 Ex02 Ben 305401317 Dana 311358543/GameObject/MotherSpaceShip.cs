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

using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    class MotherSpaceShip : Sprite, ICollidable2D
    {
        private const string k_AssteName = @"Sprites\MotherShip_32x120";
        private const float k_MotherShipVelocity = 40;
        private const int k_MaxRandomNumToDrawMotherShip = 70;
        private int m_CurrRandom = 100;
        public const int k_MaxRandomNumber = 50000;
        private IGameEngine m_GameEngine;

        public MotherSpaceShip(Game i_Game): base(k_AssteName, i_Game)
		{
            this.m_TintColor = Color.Red;
            this.Visible = false;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!m_PositionInit)
            {
                InitPosition();
                m_PositionInit = true;
            }

            if (!this.Visible)
            {
                this.m_CurrRandom = SpaceInvaders.s_RandomNum.Next(0, k_MaxRandomNumber);
            }

            if (this.m_CurrRandom <= k_MaxRandomNumToDrawMotherShip)
            {
                this.Visible = true;
                this.m_Position.X += k_MotherShipVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                if (m_Position.X >= GraphicsDevice.Viewport.Width)
                {
                    Visible = false;
                    InitPosition();
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            initAnimations();
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            m_Animations["Destroy"].Restart();

            if (m_GameEngine == null)
            {
                m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
            }

            m_GameEngine.HandleHit(this, i_Collidable);
        }

        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Width / 2,0);
            base.InitOrigins();
        }

        public void InitPosition()
        {
            this.Position = new Vector2(-Texture.Width, Texture.Height);
        }

        private void initAnimations()
        {
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(2.2));
            FadeAnimator fadeAnimator = new FadeAnimator(TimeSpan.FromSeconds(2.2));
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(2.2));

            CompositeAnimator DestroyAnimator = new CompositeAnimator("Destroy", TimeSpan.FromSeconds(2.2), this, blinkAnimator, fadeAnimator, shrinkAnimator);
            DestroyAnimator.ResetAfterFinish = false;
            DestroyAnimator.Finished += new EventHandler(destroyed_Finished);

            this.Animations.Add(DestroyAnimator);
            this.Animations.Enabled = true;
        }

        private void destroyed_Finished(object sender, EventArgs e)
        {
            //Enabled = false;
            Visible = false;
            InitPosition();
        }
    }
}
