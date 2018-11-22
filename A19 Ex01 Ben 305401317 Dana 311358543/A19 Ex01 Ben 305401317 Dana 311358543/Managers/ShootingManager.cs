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
        private Game m_Game;

        public ShootingManager(Game i_Game)
        {
            this.m_Game = i_Game;
        }  

        public void OnHit(Sprite i_Bullet, Sprite i_HittenSprite)
        {
                i_Bullet.RemoveComponent();

                if (i_HittenSprite is SpaceShip)
                {
                    this.handleSpaceShipHit((SpaceShip)i_HittenSprite);
                }
                else
                {
                    this.handleNonSpaceShipHit(i_HittenSprite);
                }
          }

        public Sprite IsGameObjectWasHitten(Bullet i_Bullet)
        {
            Sprite hittenSprite = null;
            Rectangle bulletRectangle = new Rectangle((int)i_Bullet.Position.X, (int)i_Bullet.Position.Y, i_Bullet.Texture.Width, i_Bullet.Texture.Height);

            foreach (DrawableGameComponent gameComponent in this.m_Game.Components)
            {
                if (this.isShootableComponent(gameComponent) && this.isOpponents((Sprite)gameComponent, i_Bullet))
                {
                    Rectangle elementRectangle = new Rectangle((int)((Sprite)gameComponent).Position.X, (int)((Sprite)gameComponent).Position.Y, (int)((Sprite)gameComponent).Texture.Width, (int)((Sprite)gameComponent).Texture.Height);

                    if (bulletRectangle.Intersects(elementRectangle))
                    {
                        hittenSprite = (Sprite)gameComponent;
                    }
                }
            }

            return hittenSprite;
        }

        private void handleSpaceShipHit(SpaceShip i_SpaceShip)
        {
            ScoreManager scoreManager = SpaceInvaders.s_GameUtils.ScoreManager;

            if (scoreManager.Souls.Count - 1 == 0)
            {
                i_SpaceShip.RemoveComponent();
                SpaceInvaders.s_GameUtils.InputManager.ShowGameOverMessage();
                this.m_Game.Exit();
            }
            else
            {
                scoreManager.UpdateScoreAfterHit(i_SpaceShip);
            }
        }

        private void handleNonSpaceShipHit(Sprite i_Sprite)
        {
            i_Sprite.RemoveComponent();

            if (i_Sprite is Enemy || i_Sprite is MotherSpaceShip)
            {
                ScoreManager scoreManager = SpaceInvaders.s_GameUtils.ScoreManager;
                scoreManager.UpdateScoreAfterHit(i_Sprite);
            }
        }

        private bool isShootableComponent(DrawableGameComponent i_GameComponent)
        {
            bool isShootableComponent = i_GameComponent is Enemy || i_GameComponent is Bullet || i_GameComponent is SpaceShip || i_GameComponent is MotherSpaceShip;

            return isShootableComponent;
        }

        private bool isOpponents(Sprite i_Sprite, Bullet i_Bullet)
        {
            bool isOpponent;

            isOpponent = (i_Bullet.Type == Bullet.eBulletType.EnemyBullet && (i_Sprite is SpaceShip || (i_Sprite is Bullet && ((Bullet)i_Sprite).Type == Bullet.eBulletType.SpaceShipBullet)))
            || (i_Bullet.Type == Bullet.eBulletType.SpaceShipBullet && (i_Sprite is Enemy || i_Sprite is MotherSpaceShip));

            return isOpponent;
        }
    }
}
