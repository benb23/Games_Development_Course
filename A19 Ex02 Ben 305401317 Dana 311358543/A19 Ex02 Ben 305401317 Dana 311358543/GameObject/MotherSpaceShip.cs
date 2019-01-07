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
    public class MotherSpaceShip : RectangleCollidableSprite, IRectangleCollidable
    {
        private const string k_AssteName = @"Sprites\MotherShip_32x120";
        private  float k_MotherShipVelocity = 40;
        private const int k_MaxRandomNumToDrawMotherShip = 70;
        public const int k_MaxRandomNumber = 50000;
        private IGameEngine m_GameEngine;
        private bool m_OnMove = false;

        public MotherSpaceShip(Game i_Game): base(k_AssteName, i_Game)
		{
            this.m_TintColor = Color.Red;
        }

        public override void Initialize()
        {
            base.Initialize();
            initAnimations();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!m_Initialize)
            {
                InitPosition();
                m_Initialize = true;
            }

            // TODO: change random from static to private with getter

            if (!m_OnMove)
            {
                if (SpaceInvaders.k_Random.Next(0, k_MaxRandomNumber) <= k_MaxRandomNumToDrawMotherShip)
                {
                    m_OnMove = true;
                }
            }
            else
            {
                this.m_Position.X += k_MotherShipVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                if (m_Position.X >= GraphicsDevice.Viewport.Width)
                {
                    m_OnMove = false;
                    m_Initialize = false;
                }
            }
        }

        private void InitPosition()
        {
            this.Position = new Vector2(-Texture.Width, Texture.Height);
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (!Animations["DestroyMother"].Enabled)
            {
                if (m_GameEngine == null)
                {
                    m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
                }

                Animations["DestroyMother"].Restart();
                m_GameEngine.HandleHit(this, i_Collidable);
                
            }
        }

        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Width / 2, 0);
            base.InitOrigins();
        }

        private void initAnimations()
        {
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(2.2));
            FadeAnimator fadeAnimator = new FadeAnimator(TimeSpan.FromSeconds(2.2));
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(2.2));

            CompositeAnimator DestroyAnimator2 = new CompositeAnimator("DestroyMother", TimeSpan.FromSeconds(2.2), this, blinkAnimator, fadeAnimator, shrinkAnimator);
            this.Animations.Add(DestroyAnimator2);
            Animations["DestroyMother"].Finished += new EventHandler(this.destroyed_Finished1);   
        }

        private void destroyed_Finished1(object sender, EventArgs e)
        {
            InitPosition();
            m_OnMove = false;
        }


    }
}
