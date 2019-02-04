using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class MotherSpaceShip : CollidableSprite, IRectangleCollidable
    {
        private const string k_AssteName = @"Sprites\MotherShip_32x120";
        private const int k_MaxRandomNumToDrawMotherShip = 70;
        public const int k_MaxRandomNumber = 50000;
        private Random m_Random;
        private ISpaceInvadersEngine m_GameEngine;
        private bool m_OnMove = false;

        public MotherSpaceShip(GameScreen i_GameScreen) : base(k_AssteName, i_GameScreen)
		{
            this.m_ScoreValue = (int)SpaceInvadersConfig.eScoreValue.MotherShip;
            this.m_TintColor = Color.Red;
            this.Velocity = new Vector2(40, 0);
            this.Visible = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.initAnimations();
        }

        public void InitMotherShipForNextLevel()
        {
            this.m_Initialize = false;
            this.m_OnMove = false;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!this.m_Initialize)
            {
                this.initPosition();
                this.m_Initialize = true;
            }

            if(this.m_Random == null)
            {
                this.m_Random = this.Game.Services.GetService(typeof(Random)) as Random;
            }

            if (!this.m_OnMove)
            {
                if (this.m_Random.Next(0, k_MaxRandomNumber) <= k_MaxRandomNumToDrawMotherShip)
                {
                    this.Visible = true;
                    this.m_OnMove = true;
                }
            }
            else
            {
                base.Update(i_GameTime);
                if (this.m_Position.X >= this.GraphicsDevice.Viewport.Width)
                {
                    this.Visible = false;
                    this.m_OnMove = false;
                    this.m_Initialize = false;
                }
            }
        }

        private void initPosition()
        {
            this.Position = new Vector2(-Texture.Width, Texture.Height);
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (!this.Animations["DestroyMother"].Enabled)
            {
                if (this.m_GameEngine == null)
                {
                    this.m_GameEngine = this.Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
                }

                this.Animations["DestroyMother"].Restart();
                this.m_GameEngine.HandleHit(this, i_Collidable as Bullet);
            }
        }

        protected override void InitOrigins()
        {
            this.m_PositionOrigin = new Vector2(this.Texture.Width / 2, 0);
            base.InitOrigins();
        }

        private void initAnimations()
        {
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(2.2));
            FadeAnimator fadeAnimator = new FadeAnimator(TimeSpan.FromSeconds(2.2));
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(2.2));

            CompositeAnimator DestroyAnimator2 = new CompositeAnimator("DestroyMother", TimeSpan.FromSeconds(2.2), this, fadeAnimator, blinkAnimator, shrinkAnimator);
            this.Animations.Add(DestroyAnimator2);
            Animations["DestroyMother"].Finished += new EventHandler(this.destroyed_Finished);   
        }

        private void destroyed_Finished(object sender, EventArgs e)
        {
            this.initPosition();
            this.m_OnMove = false;
        }
        
        public override void Draw(GameTime gameTime)
        {
            this.m_SpriteBatch.End();
            this.m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            this.DrawWithAllParameters();
            this.m_SpriteBatch.End();
            this.m_SpriteBatch.Begin();
        }
    }
}
