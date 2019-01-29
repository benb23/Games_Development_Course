using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class SpaceShip : CollidableSprite, IRectangleCollidable
    {
        private float k_Speed = 145; 
        private ISpaceInvadersEngine m_GameEngine;
        private Gun m_Gun;
        private PlayerIndex m_Owner;

        public float Speed
        {
            get { return this.k_Speed; }
        }

        public PlayerIndex Owner
        {
            get { return this.m_Owner; }
        }

        public SpaceShip(GameScreen i_GameScreen, string i_AssetName, Bullet.eBulletType i_GunBulletsType, PlayerIndex i_Owner)
            : base(i_AssetName, i_GameScreen)
        {
            this.m_Gun = new Gun(i_GameScreen, 3, i_GunBulletsType, -1);
            this.m_Owner = i_Owner;
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (!(i_Collidable is SpaceShip) && (!this.m_Animations["Destroy"].Enabled))
            {
                if (this.m_GameEngine == null)
                {
                    this.m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
                }

                this.m_GameEngine.HandleHit(this, i_Collidable);
            }
        }

        public void Shoot()
        {
            this.m_Gun.Shoot(new Vector2(this.Position.X, this.Position.Y - this.Texture.Height));
        }

        public bool PermitionToShoot()
        {
            return this.m_Animations["Destroy"].Enabled == false && this.m_Gun.PermitionToShoot();
        }

        protected override void InitOrigins()
        {
            this.m_PositionOrigin = new Vector2(this.Texture.Width / 2, this.Texture.Height);
            this.m_RotationOrigin = new Vector2(this.Texture.Width / 2, this.Texture.Height / 2);
            base.InitOrigins();
        }

        private void initAnimations()
        {
            BlinkAnimator blinkAnimator = new BlinkAnimator("LoosingSoul", TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(2.5));
            this.Animations.Add(blinkAnimator);
            RoataterAnimator roataterAnimator = new RoataterAnimator(4, TimeSpan.FromSeconds(2.5));
            FadeAnimator fadeAnimator = new FadeAnimator(TimeSpan.FromSeconds(2.5));
            CompositeAnimator DestroyAnimator = new CompositeAnimator("Destroy", TimeSpan.FromSeconds(2.5), this, fadeAnimator, roataterAnimator);
            //DestroyAnimator.ResetAfterFinish = false;
            this.Animations.Add(DestroyAnimator);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.initAnimations();
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
