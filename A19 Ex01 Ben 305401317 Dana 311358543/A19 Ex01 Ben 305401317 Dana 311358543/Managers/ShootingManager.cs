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
    public class ShootingManager  
    {
        private Game m_game;

        public ShootingManager(Game i_game)
        {
            m_game = i_game;
        }  

        public void OnHit(Sprite i_Bullet, Sprite i_HittenSprite)
        {
                i_Bullet.RemoveComponent();

                if (i_HittenSprite is SpaceShip)
                {
                    HandleSpaceShipHit((SpaceShip)i_HittenSprite);
                }
                else
                {
                    HandleNonSpaceShipHit(i_HittenSprite);
                }
          }

        private void HandleSpaceShipHit(SpaceShip i_SpaceShip)
        {
            ScoreManager scoreManager = m_game.Services.GetService(typeof(ScoreManager)) as ScoreManager;

            if (scoreManager.Souls.Count - 1 == 0)
            {
                i_SpaceShip.RemoveComponent();
                // TODO : MSGGGGGGG
                m_game.Exit();
            }
            else
            {
                scoreManager.UpdateScoreAfterCollision(i_SpaceShip);
            }
        }

        private void HandleNonSpaceShipHit(Sprite i_Sprite)
        {
            i_Sprite.RemoveComponent();

            if (i_Sprite is Enemy || i_Sprite is MotherSpaceShip)
            {
                ScoreManager scoreManager = m_game.Services.GetService(typeof(ScoreManager)) as ScoreManager;
                scoreManager.UpdateScoreAfterCollision(i_Sprite);
            }
        }

        public Sprite IsGameObjectWasHitten(Bullet i_Bullet)//TODO: change name!
        {
            Sprite hittenSprite = null;
            Rectangle BulletRectangle = new Rectangle((int)i_Bullet.Position.X, (int)i_Bullet.Position.Y, i_Bullet.Texture.Width, i_Bullet.Texture.Height);

            foreach (DrawableGameComponent gameComponent in m_game.Components)
            {
                if (isShootableComponent(gameComponent) && isOpponents((Sprite)gameComponent, i_Bullet))
                {
                    Rectangle elementRectangle = new Rectangle((int)((Sprite)gameComponent).Position.X, (int)((Sprite)gameComponent).Position.Y, (int)((Sprite)gameComponent).Texture.Width, (int)((Sprite)gameComponent).Texture.Height);

                    if (BulletRectangle.Intersects(elementRectangle))
                    {
                        hittenSprite = (Sprite)gameComponent;
                    }
                }
            }
            return hittenSprite;
        }

        private bool isShootableComponent(DrawableGameComponent gameComponent)
        {
            bool isShootableComponent = gameComponent is Enemy || gameComponent is Bullet || gameComponent is SpaceShip || gameComponent is MotherSpaceShip;

            return isShootableComponent;
        }

        private bool isOpponents(Sprite i_Sprite ,Bullet i_Bullet)
        {
            bool isOpponent;

            if ((i_Bullet.Type == Bullet.eBulletType.EnemyBullet && (i_Sprite is SpaceShip || (i_Sprite is Bullet && ((Bullet)i_Sprite).Type == Bullet.eBulletType.SpaceShipBullet)))
            || (i_Bullet.Type == Bullet.eBulletType.SpaceShipBullet && (i_Sprite is Enemy || i_Sprite is MotherSpaceShip)))
            {
                isOpponent = true;
            }
            else
            {
                isOpponent = false;
            }

            return isOpponent;
        }


    }
}
