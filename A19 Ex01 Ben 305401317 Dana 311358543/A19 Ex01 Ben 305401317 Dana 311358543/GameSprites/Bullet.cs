using Microsoft.Xna.Framework;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
    public class Bullet : Sprite
    {
        public enum eBulletType
        {
            SpaceShipBullet = -1,
            EnemyBullet = 1
        }

        private const float k_BulletVelocity = 155;
        private eBulletType m_Type;

        public eBulletType Type
        {
            get { return this.m_Type; }
        }

        public override void Update(GameTime i_GameTime)
        {
            ShootingManager shootingManager = SpaceInvaders.s_GameUtils.ShootingManager;

            if (this.isBulletHitTheScreenBorder())
            {
                this.RemoveComponent();
            }

            Sprite hittenSprite = shootingManager.IsGameObjectWasHitten(this);

            if (hittenSprite != null)
            {
                shootingManager.OnHit(this, hittenSprite);
            }
            else
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y + ((float)this.m_Type * k_BulletVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds));
            }
        }

        private bool isBulletHitTheScreenBorder()
        {
            bool isBulletHit = m_Position.Y <= 0 || m_Position.Y >= Game.GraphicsDevice.Viewport.Height;

            return isBulletHit;
        }

        public Bullet(Game i_Game, eBulletType i_BulletType, Sprite i_Shooter) : base(i_Game)
        {
            this.m_AssetName = @"Sprites\Bullet";
            this.m_Type = i_BulletType;

            if (this.m_Type == eBulletType.EnemyBullet)
            {
                this.m_Tint = Color.Blue;
            }
            else
            {
                this.m_Tint = Color.Red;
            }

            this.InitBulletPosition(i_Shooter);
        }

        public void InitBulletPosition(Sprite i_Shooter) 
        {
            this.Position = new Vector2(i_Shooter.Position.X + (i_Shooter.Texture.Width / 2), i_Shooter.Position.Y + ((float)this.m_Type * (1 + i_Shooter.Texture.Height))); 
        }

        public override void InitPosition() 
        {  
        }
    }
}
