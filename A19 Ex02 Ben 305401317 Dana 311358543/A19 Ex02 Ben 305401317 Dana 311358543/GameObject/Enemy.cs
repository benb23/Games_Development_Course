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
    public class Enemy : RectangleCollidableSprite, IRectangleCollidable
    {
        private List<Bullet> m_Bullets = new List<Bullet>(5);
        private const string k_AssteName = @"Sprites\EnemiesSheet_192x32";
        private Gun m_Gun;
        private const int k_MaxRandomToShoot = 10; //TODO: LOCATION?
        public const int k_MaxRandomNumber = 50000; //TODO: LOCATION?
        private IGameEngine m_GameEngine;
        private int k_NumOfTOtalFrames = 6;
        public int k_NumOfFrames = 2;
        public int m_StartSqureIndex;
        private int m_Row;
        private int m_Colum;



        public int Row
        {
            get { return m_Row; }
        }

        public int Colum
        {
            get { return m_Colum; }
        }

        public Enemy(Game i_Game, Color i_Tint, int i_StartSqureIndex, int i_Row, int i_Colum) : base(k_AssteName, i_Game)
        {
            m_Row = i_Row;
            m_Colum = i_Colum;
            m_StartSqureIndex = i_StartSqureIndex;
            m_TintColor = i_Tint;
        }

        public void LoadAsset()
        {
            this.Texture = this.Game.Content.Load<Texture2D>(this.m_AssetName);
        }

        public override void Update(GameTime i_GameTime)
        {
            int rnd = SpaceInvaders.s_RandomNum.Next(0, k_MaxRandomNumber);

            if (rnd <= k_MaxRandomToShoot && m_Gun.PermitionToShoot())
            {
                shoot();
            }

            base.Update(i_GameTime);
        }

        private void shoot()
        {
            m_Gun.Shoot(new Vector2(Position.X,Position.Y+Texture.Height/2));
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet && (i_Collidable as Bullet).Type == Bullet.eBulletType.EnemyBullet || i_Collidable is Enemy)
            {
                return;
            }
            else if(!this.m_Animations["dyingEnemy"].Enabled)
            {
                if (m_GameEngine == null)
                {
                    m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
                }

                //if ((i_Collidable as Bullet).Type != Bullet.eBulletType.EnemyBullet)
                //{
                //    Visible = false;
                //}

                m_GameEngine.HandleHit(this, i_Collidable);
                this.m_Animations["dyingEnemy"].Restart();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Gun = new Gun(Game, 1, Bullet.eBulletType.EnemyBullet, 1);
            initAnimations(); 
        }

        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Height / 2, Texture.Height/2);
            m_RotationOrigin = new Vector2(Texture.Height / 2, Texture.Height/2);
            base.InitOrigins();
        }


        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();
            m_WidthBeforeScale = m_WidthBeforeScale / k_NumOfTOtalFrames;

            this.SourceRectangle = new Rectangle(
                (int)m_WidthBeforeScale * m_StartSqureIndex,
                0,
                 (int)m_WidthBeforeScale,
                (int)Height);
        }

        private void dyingEnemy_Finished(object sender, EventArgs e)
        {
            this.Animations.Enabled = false;
            this.Visible = false;
            this.Enabled = false;
        }

        private void initAnimations()
        {
            ShrinkAnimator shrinker = new ShrinkAnimator(TimeSpan.FromSeconds(1.2));
            RoataterAnimator rotate = new RoataterAnimator(6, TimeSpan.FromSeconds(1.2));

            CompositeAnimator dyingEnemy = new CompositeAnimator("dyingEnemy", TimeSpan.FromSeconds(1.2), this, shrinker, rotate);
            this.Animations.Add(dyingEnemy);

            dyingEnemy.Finished += new EventHandler(dyingEnemy_Finished);
            this.Animations.Enabled = true;

            // this.Animations.Add(new ShrinkerAnimator("shrinkEnemy", TimeSpan.FromSeconds(1.2)));
            //   this.Animations.Add();
        }
    }
}
