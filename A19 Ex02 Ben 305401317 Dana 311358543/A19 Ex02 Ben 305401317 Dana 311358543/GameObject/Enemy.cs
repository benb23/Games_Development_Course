using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class Enemy : CollidableSprite, IRectangleCollidable, IPixelsCollidable
    {
        private const int r_MaxNumOfBullets = 5;
        private const int k_MaxRandomNumber = 50000;
        private const int k_NumOfTotalFrames = 6;
        private const string k_AssteName = @"Sprites\EnemiesSheet_192x32";
        private int m_StartSqureIndex;
        private int m_Row;
        private int m_Colum;
        private float m_Gap;
        private ISpaceInvadersEngine m_GameEngine;
        private Random m_Random;
        private Gun m_Gun;
        private TimeSpan m_TimeUntilNextStepInSec;
        private List<Bullet> m_Bullets = new List<Bullet>(r_MaxNumOfBullets);
        public int m_OriginalMaxRandomToShoot = 10;
        public int m_MaxRandomToShoot = 10;
        private int m_Toggeler;

        public int Toggeler
        {
            get { return this.m_Toggeler; }
            set { this.m_Toggeler = value; }
        }

        protected int m_OriginalScoreValue;

        public int OriginalScoreValue
        {
            get { return this.m_OriginalScoreValue; }
        }

        public int Row
        {
            get { return this.m_Row; }
        }

        public int Colum
        {
            get { return this.m_Colum; }
        }

        public Enemy(GameScreen i_GameScreen, Color i_Tint, int i_ScoreValue, int i_StartSqureIndex, int i_Row, int i_Colum, float i_Gap, float i_TimeUntilNextStepInSec)
            : base(k_AssteName, i_GameScreen)
        {
            this.m_OriginalScoreValue = this.m_ScoreValue = i_ScoreValue;
            this.m_Random = this.Game.Services.GetService(typeof(Random)) as Random;
            this.m_TimeUntilNextStepInSec = TimeSpan.FromSeconds(i_TimeUntilNextStepInSec);
            this.m_Gap = i_Gap;
            this.m_Row = i_Row;
            this.m_Colum = i_Colum;
            this.m_StartSqureIndex = i_StartSqureIndex;
            this.m_TintColor = i_Tint;
            this.m_GameScreen = i_GameScreen;
        }

        public TimeSpan TimeUntilNextStepInSec
        {
            set { this.m_TimeUntilNextStepInSec = value; }
        }

        public void LoadAsset()
        {
            this.Texture = this.Game.Content.Load<Texture2D>(this.m_AssetName);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Gun = new Gun(this.GameScreen, 1, Bullet.eBulletType.EnemyBullet, 1, "EnemyGunShot");
            this.initPosition();
            this.initAnimations();
        }

        public void initPosition()
        {
            float halfEnemySize = this.Texture.Height / 2;
            this.m_Position = new Vector2(halfEnemySize + (this.m_Colum * (this.Texture.Height * 1.5f)), (this.Texture.Height * 3f) + (this.m_Row * (this.Texture.Height * 1.5f )));
        }

        protected override void InitOrigins()
        {
            this.m_PositionOrigin = new Vector2(this.Texture.Height / 2, this.Texture.Height / 2);
            this.m_RotationOrigin = new Vector2(this.Texture.Height / 2, this.Texture.Height / 2);
        }

        protected override void InitSourceRectangle()
        {
            this.m_WidthBeforeScale = this.m_WidthBeforeScale / k_NumOfTotalFrames;

            this.SourceRectangle = new Rectangle(
                (int)this.m_WidthBeforeScale * this.m_StartSqureIndex,
                0,
                (int)this.m_WidthBeforeScale,
                (int)this.Height);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!this.Animations["CellAnimation"].Enabled)
            {
                this.m_Initialize = true;
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

            int rnd = this.m_Random.Next(0, k_MaxRandomNumber);

            if (rnd <= this.m_MaxRandomToShoot && this.m_Gun.PermitionToShoot())
            {
                this.shoot();
            }

            base.Update(i_GameTime);
        }

        private void shoot()
        {
            this.m_Gun.Shoot(new Vector2(this.Position.X, this.Position.Y + (this.Texture.Height / 2)));
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (this.m_GameEngine == null)
            {
                this.m_GameEngine = this.Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            }

            if (!this.m_Animations["dyingEnemy"].Enabled)
            {
                this.m_GameEngine.HandleHit(this, i_Collidable);
            }
        }

        private void dyingEnemyFinished(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Enabled = false;
        }

        private void cellAnimationPositionChanged(object sender, EventArgs e)
        {
            (this.m_Animations["CellAnimation"] as CellAnimator).CellTime = this.m_TimeUntilNextStepInSec;
        }

        private void initAnimations()
        {
            ShrinkAnimator shrinker = new ShrinkAnimator(TimeSpan.FromSeconds(1.2));
            RoataterAnimator rotate = new RoataterAnimator(6, TimeSpan.FromSeconds(1.2));
            CompositeAnimator dyingEnemy = new CompositeAnimator("dyingEnemy", TimeSpan.FromSeconds(1.2), this, shrinker, rotate);
            CellAnimator enemyCellAnimation = new CellAnimator(this.m_TimeUntilNextStepInSec, 2, TimeSpan.Zero, this.m_StartSqureIndex, true, this.m_Toggeler);

            this.Animations.Add(enemyCellAnimation);
            this.Animations.Add(dyingEnemy);

            this.PositionChanged += new EventHandler<EventArgs>(this.cellAnimationPositionChanged);
            dyingEnemy.Finished += new EventHandler(this.dyingEnemyFinished);
            this.Animations.Enabled = true; 
        }
    }
}
