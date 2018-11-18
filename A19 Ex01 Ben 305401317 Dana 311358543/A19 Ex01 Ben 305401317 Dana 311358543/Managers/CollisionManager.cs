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
    public class CollisionManager  
    {
        private Game m_game;

        public CollisionManager(Game i_game)
        {
            m_game = i_game;
        }  

        public void OnCollision(Sprite i_Bullet, Sprite i_HittenSprite)
        {
                i_Bullet.RemoveComponent();
                

                if (i_HittenSprite is SpaceShip)
                {
                    HandleSpaceShipCollision((SpaceShip)i_HittenSprite);
                }
                else
                {
                    HandleEnemiesOrMotherShipCollision(i_HittenSprite);
                }
          }

        private void HandleSpaceShipCollision(SpaceShip i_SpaceShip)
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

        private void HandleEnemiesOrMotherShipCollision(Sprite i_Sprite)
        {
            ScoreManager scoreManager = m_game.Services.GetService(typeof(ScoreManager)) as ScoreManager;
            i_Sprite.RemoveComponent();

            if (i_Sprite is Enemy || i_Sprite is MotherSpaceShip)
            {
                scoreManager.UpdateScoreAfterCollision(i_Sprite);
            }
        }
      }
}
