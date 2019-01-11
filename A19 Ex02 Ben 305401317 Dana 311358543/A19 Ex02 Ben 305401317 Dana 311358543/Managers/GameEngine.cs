﻿using System;
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
    public class GameEngine : GameService, IGameEngine
    {
        private enum eScoreValue
        {
            MotherShip = 850,
            Soul = -1100,
            PinkEnemy = 260,
            BlueEnemy = 140,
            YellowEnemy = 110
        }
        public enum eCollisionDirection
        {
            horizontal,
            Vertical,
            Null // TODO : ??
        }

        private Game m_Game;
        private IInputManager m_InputManager;
        private List<Player> m_Players;
        private ScoreBoardHeader m_ScoreBoard;

        private double m_sizeOfBulletHitEffect = 0.7; //todo: name???
        public GameEngine(Game i_Game) : base(i_Game)
        {
            this.m_Game = i_Game;
            m_ScoreBoard = new ScoreBoardHeader(i_Game);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsPlayerAskToExit())
            {
                Game.Exit();
            }
        }

        public bool IsPlayerAskToExit()
        {
            bool IsPlayerAskToExit;

            if (m_InputManager == null)
            {
                m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            }

            IsPlayerAskToExit = m_InputManager.KeyboardState.IsKeyDown(Keys.Escape);
          
            return IsPlayerAskToExit;
        }



        public List<Player> Players
        {
            get { return m_Players; }
            set { m_Players = value; }
        }

        private void player_Died(object sender, EventArgs e)
        {
            bool gameIsOver = true;
            foreach(Player player in m_Players)
            {
                if(player.Souls.Count != 0)
                {
                    gameIsOver = false;
                    break;
                }
            }

            if (gameIsOver)
            {
                ShowGameOverMessage();
                this.m_Game.Exit();
            }
        }

        private void updatePlayerScoreAndSouls(PlayerIndex i_PlayerIndex)
        {
            Player player = m_Players[(int)i_PlayerIndex];

            player.Score = (int)MathHelper.Clamp(player.Score + (int)eScoreValue.Soul, 0, float.PositiveInfinity);
            player.Souls.First().Visible = false;
            player.Souls.Remove(player.Souls.First());
        }

        private void updatePlayerScoreAfterHitEnemy(IScoreable i_Player, Enemy i_Enemy)
        {
            if(i_Enemy.TintColor == Color.Pink)
            {
                i_Player.Score += (int)eScoreValue.PinkEnemy;
            }
            else if(i_Enemy.TintColor == Color.LightBlue)
            {
                i_Player.Score += (int)eScoreValue.BlueEnemy;
            }
            else if(i_Enemy.TintColor == Color.LightYellow)
            {
                i_Player.Score += (int)eScoreValue.YellowEnemy;
            }
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(IGameEngine), this);
        }

        public void ShowGameOverMessage()
        {
            /*
            System.Windows.Forms.MessageBox.Show(string.Format(
@"Game Over"); //TODO: PRINT WINNER */
        }

        public void HandleHit(Bullet bullet, ICollidable i_Collidable)
        {
            if (!(bullet.Type == Bullet.eBulletType.EnemyBullet && i_Collidable is Enemy))
            {
                bullet.Visible = false;
                bullet.Enabled = false;
            }
        }

        public void HandleHit(SpaceShip i_Target, ICollidable i_Collidable)
        {
            Player player = m_Players[(int)i_Target.Owner];

            if (i_Collidable is Bullet)
            {
                updatePlayerScoreAndSouls(i_Target.Owner);
                if (player.Souls.Count == 0)
                {
                    player.SpaceShip.Animations["Destroy"].Finished += new EventHandler(m_Players[(int)i_Target.Owner].destroyed_Finished);
                    player.SpaceShip.Animations["Destroy"].Finished += new EventHandler(player_Died);
                    player.SpaceShip.Animations["Destroy"].Restart();

                }
                else
                {
                    player.SpaceShip.Animations["LoosingSoul"].Restart();
                }
            }
            else if (i_Collidable is Enemy)
            {
                ShowGameOverMessage();
                this.m_Game.Exit();
            }
        }

        public void HandleHit(Enemy i_Enemy, ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet && (i_Collidable as Bullet).Type != Bullet.eBulletType.EnemyBullet)
            {
                i_Enemy.Animations["dyingEnemy"].Restart();

                if ((i_Collidable as Bullet).Type == Bullet.eBulletType.PlayerOneBullet)
                {
                    updatePlayerScoreAfterHitEnemy(m_Players[(int)PlayerIndex.One], i_Enemy);
                }
                else if ((i_Collidable as Bullet).Type == Bullet.eBulletType.PlayerTwoBullet)
                {
                    updatePlayerScoreAfterHitEnemy(m_Players[(int)PlayerIndex.Two], i_Enemy);
                }
            }
        }

        public void HandleHit(MotherSpaceShip i_MotherSpaceShip, Bullet i_Bullet)
        {
            if (i_Bullet.Type == Bullet.eBulletType.PlayerOneBullet)
            {
                m_Players[(int)PlayerIndex.One].Score += (int)eScoreValue.MotherShip;
            }
            else if (i_Bullet.Type == Bullet.eBulletType.PlayerTwoBullet)
            {
                m_Players[(int)PlayerIndex.Two].Score += (int)eScoreValue.MotherShip;
            }
        }

        public void HandleHit(Wall i_Wall, ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                deletePixelsInVerticalDirection(i_Wall as CollidableSprite, i_Collidable as CollidableSprite);
            }
            else if(i_Collidable is Enemy)
            {
                HandleWallAndEnemyHit(i_Wall, i_Collidable as Enemy);
            }
        }

        private void HandleWallAndEnemyHit(Wall i_wall, Enemy i_Enemy)
        {
            foreach(Vector2 positionInPixels in i_wall.LastCollisionPixelsIndex)
            {
                i_wall.Pixels[(int)(positionInPixels.X + positionInPixels.Y * i_wall.Texture.Width)] = new Color(0, 0, 0, 0);
            }

            i_wall.CurrTexture.SetData(i_wall.Pixels);
        }
        
        private void deletePixelsInVerticalDirection(CollidableSprite i_Target, CollidableSprite i_Sender)
        {
            int targetStartColomn = getHittenSpritesColomnInPixelsArray(i_Target, i_Sender);
            int targetRow = getHittenSpritesRowInPixelsArray(i_Target, i_Sender);
            int senderMinY;
            int senderMaxY;

            if (i_Sender.Velocity.Y < 0) 
            {
                senderMinY = 0;
                senderMaxY = (int)(m_sizeOfBulletHitEffect * i_Sender.Texture.Height) + 1;
            }
            else
            {
                senderMinY = (int)((1 - m_sizeOfBulletHitEffect) * i_Sender.Texture.Height);
                senderMaxY = i_Sender.Texture.Height;
            }

            int targetColomn = targetStartColomn;

            //delete pixels
            for (int senderRow = senderMinY; senderRow < senderMaxY; senderRow++)
            {
                targetColomn = targetStartColomn;
                for (int senderColomn = 0; senderColomn < i_Sender.Texture.Width; senderColomn++)
                {
                    if (i_Sender.Pixels[senderColomn + senderRow * i_Sender.Texture.Width].A != 0 &&
                       (targetColomn + targetRow * i_Target.Texture.Width) < i_Target.Pixels.Length)
                    {
                        i_Target.Pixels[targetColomn + targetRow * i_Target.Texture.Width] = new Color(0, 0, 0, 0);
                    }
                    targetColomn++;
                }
                targetRow++;
            }

            i_Target.CurrTexture.SetData(i_Target.Pixels);
            clearCollisionData(i_Target, i_Sender);
        }

        private void clearCollisionData(CollidableSprite i_Target, CollidableSprite i_Sender)
        {
            i_Target.LastCollisionPixelsIndex.Clear();
            i_Target.LastCollisionPixelsPositions.Clear();
            i_Sender.LastCollisionPixelsIndex.Clear();
            i_Sender.LastCollisionPixelsPositions.Clear();
        }
        private int getHittenSpritesColomnInPixelsArray(CollidableSprite i_HittenSprite, CollidableSprite i_Sender)
        {
            return MathHelper.Clamp((int)(i_HittenSprite as CollidableSprite).LastCollisionPixelsIndex[0].X + (int)(i_Sender.Texture.Width / 2 - i_Sender.LastCollisionPixelsIndex[0].X) - i_Sender.Texture.Width / 2, 0, i_HittenSprite.Texture.Width);
        }

        private int getHittenSpritesRowInPixelsArray(CollidableSprite i_HittenSprite, CollidableSprite i_Sender)
        {
            int wallColomn = MathHelper.Clamp((int)(i_HittenSprite as CollidableSprite).LastCollisionPixelsIndex[0].Y, 0, i_HittenSprite.Texture.Height);

            if (i_Sender.Velocity.Y < 0)
            {
                wallColomn -= MathHelper.Clamp((int)(m_sizeOfBulletHitEffect * i_Sender.Texture.Height), 0, wallColomn);
            }

            return wallColomn;
        }
    }
}
