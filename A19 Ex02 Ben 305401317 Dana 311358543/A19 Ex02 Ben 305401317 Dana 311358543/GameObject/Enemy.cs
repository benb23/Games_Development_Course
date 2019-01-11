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
    public class Enemy : CollidableSprite, IRectangleCollidable, IPixelsCollidable
    {
        private const int k_MaxRandomToShoot = 10;
        public const int k_MaxRandomNumber = 50000;
        private const string k_AssteName = @"Sprites\EnemiesSheet_192x32";
        private Gun m_Gun;
        private ISpaceInvadersEngine m_GameEngine;
        private int k_NumOfTOtalFrames = 6;
        private Random m_Random;
        public int m_StartSqureIndex;
        private int m_Row;
        private int m_Colum;
        private float m_Gap;
        private TimeSpan m_TimeUntilNextStepInSec;
        public int m_Toggeler;

        public int Row
        {
            get { return this.m_Row; }
        }

        public int Colum
        {
            get { return this.m_Colum; }
        }

        public Enemy(Game i_Game, Color i_Tint, int i_StartSqureIndex, int i_Row, int i_Colum, float i_Gap, float i_TimeUntilNextStepInSec) 
            : base(k_AssteName, i_Game)
        {
            this.m_Random = this.Game.Services.GetService(typeof(Random)) as Random;
            this.m_TimeUntilNextStepInSec = TimeSpan.FromSeconds(i_TimeUntilNextStepInSec);
            this.m_Gap = i_Gap;
            this.m_Row = i_Row;
            this.m_Colum = i_Colum;
            this.m_StartSqureIndex = i_StartSqureIndex;
            this.m_TintColor = i_Tint;
        }

        public void LoadAsset()
        {
            this.Texture = this.Game.Content.Load<Texture2D>(this.m_AssetName);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Gun = new Gun(this.Game, 1, Bullet.eBulletType.EnemyBullet, 1);
            this.initPosition();

            this.initAnimations();
        }

        private void initPosition()
        {
            float halfEnemySize = this.Texture.Height / 2;
            this.m_Position = new Vector2(halfEnemySize + (this.m_Colum * ((this.Texture.Height * 1.5f) + this.m_Gap)), (this.Texture.Height * 3f) + (this.m_Row * ((this.Texture.Height * 1.5f) + this.m_Gap)));
        }

        protected override void InitOrigins()
        {
            this.m_PositionOrigin = new Vector2(this.Texture.Height / 2, this.Texture.Height / 2);
            this.m_RotationOrigin = new Vector2(this.Texture.Height / 2, this.Texture.Height / 2);
            base.InitOrigins();
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();
            this.m_WidthBeforeScale = this.m_WidthBeforeScale / this.k_NumOfTOtalFrames;

            this.SourceRectangle = new Rectangle(
                (int)this.m_WidthBeforeScale * this.m_StartSqureIndex,
                0,
                 (int)this.m_WidthBeforeScale,
                (int)this.Height);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!this.m_Initialize)
            {
                this.m_Initialize = true;
                this.Animations["CellAnimation"].Restart();
            }

            int rnd = this.m_Random.Next(0, k_MaxRandomNumber);    

            if (rnd <= k_MaxRandomToShoot && this.m_Gun.PermitionToShoot())
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
            if (!this.m_Animations["dyingEnemy"].Enabled)
            {
                if (this.m_GameEngine == null)
                {
                    this.m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
                }

                this.m_GameEngine.HandleHit(this, i_Collidable);
            }
        }

        private void dyingEnemy_Finished(object sender, EventArgs e)
        {
            this.Animations.Enabled = false;
            this.Visible = false;
            this.Enabled = false;
        }

        private void cellAnimation_PositionChanged(object sender, EventArgs e)
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

            this.PositionChanged += new EventHandler<EventArgs>(this.cellAnimation_PositionChanged);
            dyingEnemy.Finished += new EventHandler(this.dyingEnemy_Finished);
            this.Animations.Enabled = true; 
        }
    }
}
