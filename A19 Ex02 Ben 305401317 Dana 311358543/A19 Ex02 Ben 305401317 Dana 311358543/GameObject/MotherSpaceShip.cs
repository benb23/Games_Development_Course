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
    public class MotherSpaceShip : CollidableSprite, IRectangleCollidable
    {
        private const int k_MaxRandomNumToDrawMotherShip = 70;
        public const int k_MaxRandomNumber = 50000;
        private const string k_AssteName = @"Sprites\MotherShip_32x120";
        private Random m_Random;
        private ISpaceInvadersEngine m_GameEngine;
        private bool m_OnMove = false;

        public MotherSpaceShip(Game i_Game) : base(k_AssteName, i_Game)
		{
            this.m_TintColor = Color.Red;
            this.Velocity = new Vector2(40, 0);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.initAnimations();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!this.m_Initialize)
            {
                this.InitPosition();
                this.m_Initialize = true;
            }

            if(this.m_Random == null)
            {
                this.m_Random = Game.Services.GetService(typeof(Random)) as Random;
            }

            if (!this.m_OnMove)
            {
                if (this.m_Random.Next(0, k_MaxRandomNumber) <= k_MaxRandomNumToDrawMotherShip)
                {
                    this.m_OnMove = true;
                }
            }
            else
            {
                base.Update(i_GameTime);
                if (m_Position.X >= GraphicsDevice.Viewport.Width)
                {
                    this.m_OnMove = false;
                    this.m_Initialize = false;
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
                if (this.m_GameEngine == null)
                {
                    this.m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
                }

                Animations["DestroyMother"].Restart();
                this.m_GameEngine.HandleHit(this, i_Collidable as Bullet);
            }
        }

        protected override void InitOrigins()
        {
            this.m_PositionOrigin = new Vector2(Texture.Width / 2, 0);
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
            this.InitPosition();
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
