using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Enemy : CollidableSprite, IRectangleCollidable, IPixelsCollidable
    {
        private List<Bullet> m_Bullets = new List<Bullet>(5);
        private const string k_AssteName = @"Sprites\EnemiesSheet_192x32";
        private Gun m_Gun;
        public int m_OriginalMaxRandomToShoot = 10;
        public int m_MaxRandomToShoot = 10;
        public const int k_MaxRandomNumber = 50000;
        private ISpaceInvadersEngine m_GameEngine;
        private int k_NumOfTOtalFrames = 6;
        public int k_NumOfFrames = 2;
        private Random m_Random;
        public int m_StartSqureIndex;
        private int m_Row;
        private int m_Colum;
        private float m_Gap;
        private TimeSpan m_TimeUntilNextStepInSec;
        public int m_Toggeler;
        GameScreen m_GameSreen;


        public int Row
        {
            get { return m_Row; }
        }

        public int Colum
        {
            get { return m_Colum; }
        }

        public Enemy(GameScreen i_GameSreen, Color i_Tint, int i_ScoreValue, int i_StartSqureIndex, int i_Row, int i_Colum, float i_Gap, float i_TimeUntilNextStepInSec)
            : base(k_AssteName, i_GameSreen)
        {
            //m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            m_OriginalScoreValue = m_ScoreValue = i_ScoreValue; //+ (int)m_GameEngine.Level*120; // todo : dana  const?
            m_Random = Game.Services.GetService(typeof(Random)) as Random;
            m_TimeUntilNextStepInSec = TimeSpan.FromSeconds(i_TimeUntilNextStepInSec);
            m_Gap = i_Gap;
            m_Row = i_Row;
            m_Colum = i_Colum;
            m_StartSqureIndex = i_StartSqureIndex;
            m_TintColor = i_Tint;
            m_GameSreen = i_GameSreen;
        }

        public TimeSpan TimeUntilNextStepInSec
        {
            set { m_TimeUntilNextStepInSec = value; }
        }

        public void LoadAsset()
        {
            this.Texture = this.Game.Content.Load<Texture2D>(this.m_AssetName);
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Gun = new Gun(m_GameSreen, 1, Bullet.eBulletType.EnemyBullet, 1, "EnemyGunShot");
            initPosition();

            initAnimations();
        }

        public void initPosition()
        {
            float halfEnemySize = this.Texture.Height / 2;
            m_Position = new Vector2(halfEnemySize + m_Colum * (Texture.Height * 1.5f ), this.Texture.Height * 3f + m_Row * (Texture.Height * 1.5f ));
        }

        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Height / 2, Texture.Height / 2);
            m_RotationOrigin = new Vector2(Texture.Height / 2, Texture.Height / 2);
        }

        protected override void InitSourceRectangle()
        {
            m_WidthBeforeScale = m_WidthBeforeScale / k_NumOfTOtalFrames;

            this.SourceRectangle = new Rectangle(
                (int)m_WidthBeforeScale * m_StartSqureIndex,
                0,
                 (int)m_WidthBeforeScale,
                (int)Height);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!this.Animations["CellAnimation"].Enabled)
            {
                m_Initialize = true;
                this.Animations["CellAnimation"].Restart();
            }

            ////test for level
            //m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            //if (SpaceInvadersConfig.m_Level == (int)SpaceInvadersConfig.eLevel.One || 
            //    SpaceInvadersConfig.m_Level == (int)SpaceInvadersConfig.eLevel.Two ||
            //    SpaceInvadersConfig.m_Level == (int)SpaceInvadersConfig.eLevel.tree)
            //{
            //    Visible = false;
            //    Enabled = false;
            //}
            ///////////

            int rnd = m_Random.Next(0, k_MaxRandomNumber);    

            //if (rnd <= m_MaxRandomToShoot && m_Gun.PermitionToShoot())
            //{
            //    shoot();
            //}

            base.Update(i_GameTime);
        }

        private void shoot()
        {
            m_Gun.Shoot(new Vector2(Position.X, Position.Y + (Texture.Height / 2)));
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (m_GameEngine == null)
            {
                m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            }

            if ((!this.m_Animations["dyingEnemy"].Enabled))
            {
                m_GameEngine.HandleHit(this, i_Collidable);
            }
        }

        private void dyingEnemy_Finished(object sender, EventArgs e)
        {
            //this.Animations.Enabled = false;
            this.Visible = false;
            this.Enabled = false;
        }

        private void cellAnimation_PositionChanged(object sender, EventArgs e)
        {
            (m_Animations["CellAnimation"] as CellAnimator).CellTime = m_TimeUntilNextStepInSec;
        }

        private void initAnimations()
        {
            ShrinkAnimator shrinker = new ShrinkAnimator(TimeSpan.FromSeconds(1.2));
            RoataterAnimator rotate = new RoataterAnimator(6, TimeSpan.FromSeconds(1.2));
            CompositeAnimator dyingEnemy = new CompositeAnimator("dyingEnemy", TimeSpan.FromSeconds(1.2), this, shrinker, rotate);
            CellAnimator enemyCellAnimation = new CellAnimator(m_TimeUntilNextStepInSec, 2, TimeSpan.Zero,  m_StartSqureIndex, true, m_Toggeler);

            this.Animations.Add(enemyCellAnimation);
            this.Animations.Add(dyingEnemy);

            PositionChanged += new EventHandler<EventArgs>(cellAnimation_PositionChanged);
            dyingEnemy.Finished += new EventHandler(dyingEnemy_Finished);
            this.Animations.Enabled = true; 
        }
    }
}
