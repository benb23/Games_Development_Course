using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Bullet : Sprite, ICollidable2D
    {
        public enum eBulletType //change the direction
        {
            PlayerOneBullet,
            PlayerTwoBullet,
            EnemyBullet 
        }

        private const string k_AssteName = @"Sprites\Bullet";
        private const float k_BulletVelocity = 155;
        private eBulletType m_Type;

        public eBulletType Type
        {
            get { return this.m_Type; }
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.isBulletHitTheScreenBorder())
            {
                this.Visible = false;
            }

            this.Position = new Vector2(this.Position.X, this.Position.Y + ((float)this.m_Type * k_BulletVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds));
        }

        private bool isBulletHitTheScreenBorder()
        {
            bool isBulletHit = m_Position.Y <= 0 || m_Position.Y >= Game.GraphicsDevice.Viewport.Height;

            return isBulletHit;
        }

        public Bullet(Game i_Game, eBulletType i_BulletType) : base(k_AssteName ,i_Game)
        {
            this.m_Type = i_BulletType;

            if (this.m_Type == eBulletType.EnemyBullet)
            {
                this.m_TintColor = Color.Blue;
            }
            else
            {
                this.m_TintColor = Color.Red;
            }

            m_Velocity =new Vector2 (0,k_BulletVelocity); // TODO: in initialize???
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            Visible = false;
        }
    }
}
