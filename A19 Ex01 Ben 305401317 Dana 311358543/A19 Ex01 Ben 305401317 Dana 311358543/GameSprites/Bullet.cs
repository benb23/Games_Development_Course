using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
    class Bullet : Sprite
    {
        public enum BulletType { SpaceShipBullet = -1, EnemyBullet = 1 };
        private BulletType m_Type;
        private readonly float r_BulletVelocity = 155;

        public override void Update(GameTime gameTime)
        {
           if(isBulletHitElement()|| isBulletHitTheScreenBorder())//collision 
           {
                //destroy element
                RemoveComponent();
                m_visible = false;
            }
           else
           {
                Position = new Vector2(Position.X,Position.Y + (float)m_Type*r_BulletVelocity* (float)gameTime.ElapsedGameTime.TotalSeconds);
           }
        }

        private bool isBulletHitTheScreenBorder()
        {
            if(m_Position.Y==0 || m_Position.Y==Game.GraphicsDevice.Viewport.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Bullet(Game game, BulletType bulletType, Vector2 shooterPosition ) :base(game)
        {
            m_AssetName = @"Sprites\Bullet";
            
            if(m_Type == BulletType.EnemyBullet)
            {
                m_Tint = Color.Green;
            }
            else
            {
                m_Tint = Color.Red;
            }

            m_Position = shooterPosition;

            m_visible = true;
            m_Type = bulletType;
        }

        private bool isBulletHitElement()//TODO: change name!
        {

            return false;
        }

        public override void initPosition()
        {
            Position=new Vector2(Position.X + 32/2, Position.Y +(float)m_Type*10);//TODO: CONST 32 SHOOTER WIDTH
        }
    }
}
