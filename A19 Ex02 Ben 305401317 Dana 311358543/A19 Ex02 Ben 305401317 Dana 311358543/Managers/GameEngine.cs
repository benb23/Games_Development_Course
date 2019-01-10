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

        public void HandleHit(Wall i_wall, ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                if (checkBulletCollisionDirection(i_Collidable as Bullet) == eCollisionDirection.horizontal)
                {
                    handleWallAndBullethorizontalCollision(i_wall, i_Collidable as Bullet);
                }
                else
                {
                    handleWallAndBulletVerticalCollision(i_wall, i_Collidable as Bullet);
                }
            }
            else if(i_Collidable is Enemy)
            {
                HandleWallAndEnemyHit(i_wall, i_Collidable as Enemy);
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

        private void handleWallAndBulletVerticalCollision(Wall i_wall, Bullet i_bullet)
        {
            int wallRow =MathHelper.Clamp((int)(i_wall as CollidableSprite).LastCollisionPixelsIndex[0].X/* - (int)(i_bullet.LastCollisionPixelsPositions[0].X-i_bullet.Position.X)*/-i_bullet.Texture.Width/2,0,i_wall.Texture.Width);
            int wallColomn = MathHelper.Clamp((int)(i_wall as CollidableSprite).LastCollisionPixelsIndex[0].Y, 0, i_wall.Texture.Height);
            int wallX = wallRow;
            int wallY = wallColomn;
            int bulletMinY, bulletMaxY;

            if (i_bullet.Type != Bullet.eBulletType.EnemyBullet)
            {
                wallY -= MathHelper.Clamp((int)(m_sizeOfBulletHitEffect * i_bullet.Texture.Height), 0, wallColomn);
                bulletMinY = 0;
                bulletMaxY =(int)( m_sizeOfBulletHitEffect * i_bullet.Texture.Height) + 1 ;
            }
            else
            {
                bulletMinY = (int)((1 - m_sizeOfBulletHitEffect) * i_bullet.Texture.Height);
                bulletMaxY = i_bullet.Texture.Height;
            }


            for (int bulletRow = bulletMinY; bulletRow < bulletMaxY; bulletRow++)
                {
                    wallX = wallRow;
                    for (int bulletColomn = 0; bulletColomn < i_bullet.Texture.Width; bulletColomn++)
                    {
                        if (i_bullet.Pixels[bulletColomn + bulletRow * i_bullet.Texture.Width].A != 0 &&
                           (wallX + wallY * i_wall.Texture.Width) < i_wall.Pixels.Length)
                        {
                            i_wall.Pixels[wallX + wallY * i_wall.Texture.Width] = new Color(0, 0, 0, 0);
                        }
                        wallX++;
                    }
                    wallY++;
                }

            i_wall.CurrTexture.SetData(i_wall.Pixels);
        }

        private void handleWallAndBullethorizontalCollision(Wall i_wall, Bullet i_bullet)
        {
            float wallX = (float)i_bullet.Position.X - (float)(0.5 * i_bullet.Texture.Width) -7/10 * i_bullet.Texture.Width;
            float wallY = (float)i_bullet.Position.Y + (float)(0.5 * i_bullet.Texture.Height);

            for (int row = 0; row < i_bullet.Texture.Height; row++)//todo: <=?
            {
                for(int colomn = 0; colomn <= m_sizeOfBulletHitEffect * i_bullet.Texture.Width; colomn++)
                {
                    if(i_bullet.Pixels[row + colomn* i_bullet.Texture.Width].A != 0)
                    {
                        i_wall.Pixels[(int)wallX + (int)(wallY * m_sizeOfBulletHitEffect * i_bullet.Texture.Width)] = new Color(0, 0, 0, 0);
                    }
                    wallX++;
                }
                wallY++;
            }

        }

        private eCollisionDirection checkBulletCollisionDirection(Bullet i_bullet)
        {
            eCollisionDirection collisionDirection = eCollisionDirection.Null;

            foreach (Vector2 pixelPosition in i_bullet.LastCollisionPixelsPositions)
            {
                if(pixelPosition.X ==i_bullet.Position.X +0.5*i_bullet.Texture.Width || pixelPosition.X == i_bullet.Position.X + 0.5 * i_bullet.Texture.Width)
                {
                    collisionDirection = eCollisionDirection.horizontal;
                }
                else
                {
                    collisionDirection = eCollisionDirection.Vertical;
                }
            }

            return collisionDirection;
        }
    }
}
