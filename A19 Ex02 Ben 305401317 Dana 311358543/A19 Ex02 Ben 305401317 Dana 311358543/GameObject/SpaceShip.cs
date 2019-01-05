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
    public class SpaceShip : Sprite, ICollidable2D
    {
        private float k_Speed = 145; //TODO : VELOCITY
        private const string k_AssteName = @"Sprites\Ship01_32x32";
        private IGameEngine m_GameEngine;
        private Gun m_Gun;
        private PlayerIndex m_Owner;
        public float Speed
        {
            get { return k_Speed; }
        }

        public CompositeAnimator Animations
        {
            get { return m_Animations; }
        }
        public PlayerIndex Owner
        {
            get { return m_Owner; }
        }
        public SpaceShip(Game i_Game, Bullet.eBulletType i_GunBulletsType, PlayerIndex i_Owner)
            : base(k_AssteName, i_Game)
        {
            m_Gun = new Gun(i_Game, 3, i_GunBulletsType,-1);
            m_Owner = i_Owner;
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (!(i_Collidable is SpaceShip))
            {
                if (m_GameEngine == null)
                {
                    m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
                }

                m_GameEngine.HandleHit(this, i_Collidable);
                
                /*
                this.Animations.Enabled = true;
                m_Animations["LoosingSoul"].Restart();*/
                
            }
        }

        public void Shoot()
        {
             m_Gun.Shoot(new Vector2(Position.X, Position.Y - Texture.Height));
        }
        public bool PermitionToShoot()
        {
            return m_Gun.PermitionToShoot();
        }
        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Width / 2, Texture.Height);
            m_RotationOrigin = new Vector2(Texture.Width / 2, Texture.Height/2);
            base.InitOrigins();
        }

        private void initAnimations()
        {
            BlinkAnimator blinkAnimator = new BlinkAnimator("LoosingSoul",TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(2.5));//TODO: numbers?
            this.Animations.Add(blinkAnimator);
            
            FadeAnimator fadeAnimator = new FadeAnimator(TimeSpan.FromSeconds(4.5));
            SpriteAnimator[] destriyAnimations =  { fadeAnimator };
            CompositeAnimator spaceShipDestroyAnimator = new CompositeAnimator("Destroy" ,TimeSpan.FromSeconds(2.5),this, destriyAnimations) ;
            spaceShipDestroyAnimator.ResetAfterFinish = false;
            m_Animations.Add(spaceShipDestroyAnimator);
            
            this.Animations.Enabled = true;
        }



        public override void Initialize()
        {
            base.Initialize();
            initAnimations();
        }
    }
}
