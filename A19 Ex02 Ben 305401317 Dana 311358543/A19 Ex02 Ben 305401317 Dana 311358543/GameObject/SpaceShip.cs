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
    public class SpaceShip : CollidableSprite, IRectangleCollidable
    {
        private float k_Speed = 145; //TODO : VELOCITY
        private ISpaceInvadersEngine m_GameEngine;
        private Gun m_Gun;
        private PlayerIndex m_Owner;
        public float Speed
        {
            get { return k_Speed; }
        }

        public PlayerIndex Owner
        {
            get { return m_Owner; }
        }
        public SpaceShip(Game i_Game, string i_AssetName, Bullet.eBulletType i_GunBulletsType, PlayerIndex i_Owner)
            : base(i_AssetName, i_Game)
        {
            m_Gun = new Gun(i_Game, 3, i_GunBulletsType, -1);
            m_Owner = i_Owner;
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (!(i_Collidable is SpaceShip) && (!m_Animations["Destroy"].Enabled))
            {
                if (m_GameEngine == null)
                {
                    m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
                }

                m_GameEngine.HandleHit(this, i_Collidable);
            }
        }

        public void Shoot()
        {
             m_Gun.Shoot(new Vector2(Position.X, Position.Y - Texture.Height));
        }

        public bool PermitionToShoot()
        {
            return m_Animations["Destroy"].Enabled == false && m_Gun.PermitionToShoot();
        }

        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Width / 2, Texture.Height);
            m_RotationOrigin = new Vector2(Texture.Width / 2, Texture.Height/2);
            base.InitOrigins();
        }

        private void initAnimations()
        {
            BlinkAnimator blinkAnimator = new BlinkAnimator("LoosingSoul",TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(2.5));
            this.Animations.Add(blinkAnimator);

            RoataterAnimator roataterAnimator = new RoataterAnimator(4, TimeSpan.FromSeconds(2.5));
            FadeAnimator fadeAnimator = new FadeAnimator(TimeSpan.FromSeconds(2.5));

            CompositeAnimator DestroyAnimator = new CompositeAnimator("Destroy" ,TimeSpan.FromSeconds(2.5),this, fadeAnimator, roataterAnimator);
            DestroyAnimator.ResetAfterFinish = false;
            
            this.Animations.Add(DestroyAnimator);
        }

        public override void Initialize()
        {
            base.Initialize();
            initAnimations();
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.End();
            m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            this.DrawWithAllParameters();
            m_SpriteBatch.End();
            m_SpriteBatch.Begin();
        }
    }
}
