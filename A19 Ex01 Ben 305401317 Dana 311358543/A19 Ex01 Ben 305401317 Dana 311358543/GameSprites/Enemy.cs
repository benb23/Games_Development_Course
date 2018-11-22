using Microsoft.Xna.Framework;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
    public class Enemy : Sprite
    {
        private const int k_MaxRandomToShoot = 10;
        private Gun m_Gun = new Gun(); 

        public Enemy(Game i_Game, Color i_Tint, string i_AssetName) : base(i_Game)
        {
            this.m_AssetName = i_AssetName;
            this.m_Tint = i_Tint;
        }

        public override void Update(GameTime i_GameTime)
        {
            int rnd = SpaceInvaders.s_RandomNum.Next(0,  SpaceInvaders.k_MaxRandomNumber);
            
            if (rnd <= k_MaxRandomToShoot)
            {
                this.m_Gun.Shoot(new Bullet(Game, Bullet.eBulletType.EnemyBullet, this));
            }
        }

        public override void InitPosition()
        {
        }
    }
}
