using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class SpaceInvadersEngine : GameService, ISpaceInvadersEngine
    {
        private bool m_IsGameOver = false;

        public bool IsGameOver
        {
            get { return m_IsGameOver; }
            set { m_IsGameOver = value; }
        }

        //private PlayerIndex? m_Winner;

        //public PlayerIndex? Winner
        //{
        //    get { return m_Winner; }
        //}

        private ISoundMananger m_SoundManager;
        private IInputManager m_InputManager;
        private List<Player> m_Players;
        private Random m_Random;
        private Game m_Game;

        public SpaceInvadersEngine(Game i_Game) : base(i_Game)
        {
            this.m_Game = i_Game;
            this.m_SoundManager = i_Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;
            //m_ScoreBoard = new ScoreBoardHeader(i_GameScreen);
        }

        public void InitGameEngineForNewGame()
        {
            SpaceInvadersConfig.m_LogicLevel = SpaceInvadersConfig.eLevel.One;
            InitNewPlayers();
            this.IsGameOver = false;
        }

        private void InitNewPlayers()
        {
            for(int i=0 ; i<(int)SpaceInvadersConfig.m_NumOfPlayers; i++)
            {
                this.m_Players[i].InitSouls();
                this.m_Players[i].Score = 0;
                this.m_Players[i].Enabled = true;
                this.m_Players[i].SpaceShip.Enabled = true;
                this.m_Players[i].SpaceShip.Visible = true;
            }
            initPlayersForNextLevel();
        }

        public void CreatePlayers(GameScreen i_GameScreen)
        {
            if (m_Players == null)
            {
                m_Players = new List<Player>((int)SpaceInvadersConfig.m_NumOfPlayers);
                m_Players.Add(new Player(i_GameScreen, PlayerIndex.One, Keys.H, Keys.K, Keys.U, true, new Vector2(0, 0)));
                if (SpaceInvadersConfig.m_NumOfPlayers == SpaceInvadersConfig.eNumOfPlayers.TwoPlayers)
                {
                    m_Players.Add(new Player(i_GameScreen, PlayerIndex.Two, Keys.A, Keys.D, Keys.W, false, new Vector2(1, 0)));
                }
            }
            else
            {
                InitNewPlayers();
            }
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
            this.m_IsGameOver = true;
            foreach(Player player in m_Players)
            {
                if(player.CurrentSoulsNum != 0)
                {
                    m_IsGameOver = false;
                    break;
                }
            }

            //if (this.m_IsGameOver && SpaceInvadersConfig.m_NumOfPlayers == SpaceInvadersConfig.eNumOfPlayers.TwoPlayers)
            //{
            //    m_Winner = getWinner();
            //}
        }
        
        public PlayerIndex? getWinner()
        {
            PlayerIndex? winner;

            if(Players[(int)PlayerIndex.One].Score > Players[(int)PlayerIndex.Two].Score)
            {
                winner = PlayerIndex.One;
            }
            else if(Players[(int)PlayerIndex.One].Score < Players[(int)PlayerIndex.Two].Score)
            {
                winner = PlayerIndex.Two;
            }
            else
            {
                winner = null;
            }

            return winner;
        }

        private void updatePlayerScoreAndSouls(PlayerIndex i_PlayerIndex)
        {
            Player player = m_Players[(int)i_PlayerIndex];

            player.Score = (int)MathHelper.Clamp(player.Score + player.Souls[0].ScoreValue, 0, float.PositiveInfinity);
            player.KillSoul(player);

            //player.Souls.Remove(player.Souls.First());
        }

        public void InitGameEngineForNextLevel()
        {
            initPlayersForNextLevel();
            SpaceInvadersConfig.m_Level++;
            SpaceInvadersConfig.m_LogicLevel = (SpaceInvadersConfig.eLevel)MathHelper.Clamp((int)SpaceInvadersConfig.m_LogicLevel +1, 0, (int)SpaceInvadersConfig.eLevel.Six);
        }

        private void initPlayersForNextLevel()
        {
            foreach (Player player in m_Players)
            {
                player.initPlayerForForNextLevel();
            }
        }

        private void updatePlayerScoreAfterHitEnemy(IScoreable i_Player, Enemy i_Enemy)
        {
            i_Player.Score += i_Enemy.ScoreValue;
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(ISpaceInvadersEngine), this);
        }

        public void HandleHit(Bullet bullet, ICollidable i_Collidable)
        {
            if (!(bullet.Type == Bullet.eBulletType.EnemyBullet && i_Collidable is Enemy))
            {
                if (bullet.Type == Bullet.eBulletType.EnemyBullet && i_Collidable is Bullet)
                {
                    if (this.m_Random == null)
                    {
                        this.m_Random = Game.Services.GetService(typeof(Random)) as Random;
                    }

                    int rndDisposeOfEnemyBullet = this.m_Random.Next(0, 45);
                    if (rndDisposeOfEnemyBullet < 10)
                    {
                        bullet.Enabled = false;
                        bullet.Visible = false;
                    }
                }
                else
                {
                    bullet.Enabled = false;
                    bullet.Visible = false;
                }
            }
        }

        public void HandleHit(SpaceShip i_Target, ICollidable i_Collidable)
        {
            Player player = this.m_Players[(int)i_Target.Owner];
            

            if (i_Collidable is Bullet)
            {
                this.updatePlayerScoreAndSouls(i_Target.Owner);
                if (player.CurrentSoulsNum == 0)
                {
                    
                    player.SpaceShip.Animations["Destroy"].Finished += new EventHandler(this.m_Players[(int)i_Target.Owner].destroyed_Finished);
                    player.SpaceShip.Animations["Destroy"].Finished += new EventHandler(this.player_Died);
                    player.SpaceShip.Animations["Destroy"].Restart();
                    
                }
                else
                {
                    player.SpaceShip.Animations["LoosingSoul"].Restart();
                    
                }
                
                m_SoundManager.GetSoundEffect("LifeDie").Play();
            }
            else if (i_Collidable is Enemy)
            {
                this.m_IsGameOver = true;
            }
        }

        public void HandleHit(Enemy i_Enemy, ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet && (i_Collidable as Bullet).Type != Bullet.eBulletType.EnemyBullet)
            {
                i_Enemy.Animations["dyingEnemy"].Restart();
                m_SoundManager.GetSoundEffect("EnemyKill").Play();

                if ((i_Collidable as Bullet).Type == Bullet.eBulletType.PlayerOneBullet)
                {
                    this.updatePlayerScoreAfterHitEnemy(this.m_Players[(int)PlayerIndex.One], i_Enemy);
                }
                else if ((i_Collidable as Bullet).Type == Bullet.eBulletType.PlayerTwoBullet)
                {
                    this.updatePlayerScoreAfterHitEnemy(this.m_Players[(int)PlayerIndex.Two], i_Enemy);
                }
            }
        }

        public void HandleHit(MotherSpaceShip i_MotherSpaceShip, Bullet i_Bullet)
        {
            if (i_Bullet.Type == Bullet.eBulletType.PlayerOneBullet)
            {
                this.m_Players[(int)PlayerIndex.One].Score += i_MotherSpaceShip.ScoreValue;
            }
            else if (i_Bullet.Type == Bullet.eBulletType.PlayerTwoBullet)
            {
                this.m_Players[(int)PlayerIndex.Two].Score += i_MotherSpaceShip.ScoreValue;
            }

            m_SoundManager.GetSoundEffect("MotherShipKill").Play();

        }

        public void HandleHit(Wall i_Wall, ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet && i_Wall.LastCollisionPixelsIndex.Count > 0)
            {
                this.deletePixelsInVerticalDirection(i_Wall as CollidableSprite, i_Collidable as CollidableSprite);
                m_SoundManager.GetSoundEffect("BarrierHit").Play();

            }
            else if(i_Collidable is Enemy)
            {
                this.HandleWallAndEnemyHit(i_Wall, i_Collidable as Enemy);
            }
        }

        private void HandleWallAndEnemyHit(Wall i_wall, Enemy i_Enemy)
        {
            foreach(Vector2 positionInPixels in i_wall.LastCollisionPixelsIndex)
            {
                i_wall.Pixels[(int)(positionInPixels.X + (positionInPixels.Y * i_wall.Texture.Width))] = new Color(0, 0, 0, 0);
            }

            i_wall.CurrTexture.SetData(i_wall.Pixels);
        }
        
        private void deletePixelsInVerticalDirection(CollidableSprite i_Target, CollidableSprite i_Sender)
        {
            int targetStartColomn = this.getHittenSpritesColomnInPixelsArray(i_Target, i_Sender);
            int targetRow = this.getHittenSpritesRowInPixelsArray(i_Target, i_Sender);
            int senderMinY, senderMaxY;

            if (i_Sender.Velocity.Y < 0) 
            {
                senderMinY = 0;
                senderMaxY = (int)(SpaceInvadersConfig.m_sizeOfBulletHitEffect * i_Sender.Texture.Height) + 1;
            }
            else
            {
                senderMinY = (int)((1 - SpaceInvadersConfig.m_sizeOfBulletHitEffect) * i_Sender.Texture.Height);
                senderMaxY = i_Sender.Texture.Height;
            }

            int targetColomn = targetStartColomn;

            /// delete pixels
            for (int senderRow = senderMinY; senderRow < senderMaxY; senderRow++)
            {
                targetColomn = targetStartColomn;
                for (int senderColomn = 0; senderColomn < i_Sender.Texture.Width; senderColomn++)
                {
                    if (i_Sender.Pixels[senderColomn + (senderRow * i_Sender.Texture.Width)].A != 0 &&
                       (targetColomn + (targetRow * i_Target.Texture.Width)) < i_Target.Pixels.Length)
                    {
                        i_Target.Pixels[targetColomn + (targetRow * i_Target.Texture.Width)] = new Color(0, 0, 0, 0);
                    }

                    targetColomn++;
                    if(targetColomn > i_Target.Texture.Width)
                    {
                        break;
                    }
                }

                targetRow++;
                if(targetRow > i_Target.Height)
                {
                    break;
                }
            }

            i_Target.CurrTexture.SetData(i_Target.Pixels);
            this.clearCollisionData(i_Target, i_Sender);
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
            return MathHelper.Clamp((int)i_HittenSprite.LastCollisionPixelsIndex[0].X + (int)((i_Sender.Texture.Width / 2) - i_Sender.LastCollisionPixelsIndex[0].X) - (i_Sender.Texture.Width / 2), 0, i_HittenSprite.Texture.Width);
        }

        private int getHittenSpritesRowInPixelsArray(CollidableSprite i_HittenSprite, CollidableSprite i_Sender)
        {
            int wallColomn = MathHelper.Clamp((int)(i_HittenSprite as CollidableSprite).LastCollisionPixelsIndex[0].Y, 0, i_HittenSprite.Texture.Height);

            if (i_Sender.Velocity.Y < 0)
            {
                wallColomn -= MathHelper.Clamp((int)(SpaceInvadersConfig.m_sizeOfBulletHitEffect * i_Sender.Texture.Height), 0, wallColomn);
            }

            return wallColomn;
        }
    }
}
