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
           if(isBulletHitTheScreenBorder())
           {
                //destroy element
                RemoveComponent();
                Visible = false;
            }
           else if(isBulletHitElement())
           {
                //destroy element
                RemoveComponent();
                Visible = false;
            }
           else
           {
                Position = new Vector2(Position.X,Position.Y + (float)m_Type*r_BulletVelocity* (float)gameTime.ElapsedGameTime.TotalSeconds);
           }
        }

        private bool isBulletHitTheScreenBorder()
        {
            if(m_Position.Y <= 0 || m_Position.Y >= Game.GraphicsDevice.Viewport.Height)
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
            m_Type = bulletType;

            if(m_Type == BulletType.EnemyBullet)
            {
                m_Tint = Color.Blue;
            }
            else
            {
                m_Tint = Color.Red;
            }

            initBulletPosition(shooterPosition);

            Visible = true;
            m_Type = bulletType;
        }

        private bool isBulletHitElement()//TODO: change name!
        {
            foreach (DrawableGameComponent element in Game.Components)
            {
                if (element is Sprite)
                {
                    if (Position == ((Sprite)element).Position && !(element is Bullet))
                    {
                        element.Dispose();
                    }
                }
            }

            return false;
        }

        //(Position == ((Sprite)element).Position && !(element is Bullet)

        public void initBulletPosition(Vector2 i_ShooterPosition)
        {
            Position=new Vector2(i_ShooterPosition.X + 32/2, i_ShooterPosition.Y +(float)m_Type*10);//TODO: CONST 32 SHOOTER WIDTH
        }

        public override void initPosition()
        {
           
        }
    }
}
