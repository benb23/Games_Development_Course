using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class SpaceInvadersEngine : GameService, ISpaceInvadersEngine
    {
        private bool m_IsGameOver = false;
        private ISoundMananger m_SoundManager;
        private IInputManager m_InputManager;
        private List<Player> m_Players;
        private Random m_Random;
        private Game m_Game;

        public bool IsGameOver
        {
            get { return this.m_IsGameOver; }
            set { this.m_IsGameOver = value; }
        }

        public SpaceInvadersEngine(Game i_Game) : base(i_Game)
        {
            this.m_Game = i_Game;
            this.m_SoundManager = i_Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;
        }

        public void InitGameEngineForNewGame()
        {
            SpaceInvadersConfig.s_LogicLevel = SpaceInvadersConfig.eLevel.One;
            this.initNewPlayers();
            this.IsGameOver = false;
        }

        private void initNewPlayers()
        {
            for (int i = 0; i < (int)SpaceInvadersConfig.s_NumOfPlayers; i++)
            {
                this.m_Players[i].InitSouls();
                this.m_Players[i].Score = 0;
                this.m_Players[i].Enabled = true;
                this.m_Players[i].SpaceShip.Enabled = true;
                this.m_Players[i].SpaceShip.Visible = true;
            }

            this.initPlayersForNextLevel();
        }

        public void CreatePlayers(GameScreen i_GameScreen)
        {
            if (this.m_Players == null)
            {
                this.m_Players = new List<Player>((int)SpaceInvadersConfig.s_NumOfPlayers);
                this.m_Players.Add(new Player(i_GameScreen, PlayerIndex.One, Keys.H, Keys.K, Keys.U, true, new Vector2(0, 0)));
                if (SpaceInvadersConfig.s_NumOfPlayers == SpaceInvadersConfig.eNumOfPlayers.TwoPlayers)
                {
                    this.m_Players.Add(new Player(i_GameScreen, PlayerIndex.Two, Keys.A, Keys.D, Keys.W, false, new Vector2(1, 0)));
                }
            }
            else
            {
                this.initNewPlayers();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.IsPlayerAskToExit())
            {
                this.Game.Exit();
            }
        }

        public bool IsPlayerAskToExit()
        {
            bool IsPlayerAskToExit;

            if (this.m_InputManager == null)
            {
                this.m_InputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            }

            IsPlayerAskToExit = this.m_InputManager.KeyboardState.IsKeyDown(Keys.Escape);
          
            return IsPlayerAskToExit;
        }

        public List<Player> Players
        {
            get { return this.m_Players; }
            set { this.m_Players = value; }
        }

        private void player_Died(object sender, EventArgs e)
        {
            this.m_IsGameOver = true;
            foreach(Player player in this.m_Players)
            {
                if(player.CurrentSoulsNum != 0)
                {
                    this.m_IsGameOver = false;
                    break;
                }
            }
        }
        
        public PlayerIndex? getWinner()
        {
            PlayerIndex? winner;

            if(this.Players[(int)PlayerIndex.One].Score > this.Players[(int)PlayerIndex.Two].Score)
            {
                winner = PlayerIndex.One;
            }
            else if(this.Players[(int)PlayerIndex.One].Score < this.Players[(int)PlayerIndex.Two].Score)
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
            Player player = this.m_Players[(int)i_PlayerIndex];

            player.Score = (int)MathHelper.Clamp(player.Score + player.SpaceShip.ScoreValue, 0, float.PositiveInfinity);
            player.KillSoul(player);
        }

        public void InitGameEngineForNextLevel()
        {
            this.initPlayersForNextLevel();
            SpaceInvadersConfig.m_Level++;
            SpaceInvadersConfig.s_LogicLevel =(SpaceInvadersConfig.eLevel)(SpaceInvadersConfig.m_Level % ((int)SpaceInvadersConfig.eLevel.Six + 1));
        }

        private void initPlayersForNextLevel()
        {
            foreach (Player player in this.m_Players)
            {
                player.initPlayerForForNextLevel();
            }
        }

        private void updatePlayerScoreAfterHitEnemy(Player i_Player, Enemy i_Enemy)
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

                this.m_SoundManager.PlaySoundEffect("LifeDie");
            }
            else if (i_Collidable is Enemy)
            {
                this.m_IsGameOver = true;
            }
        }

        public void ChangeNumOfPlayers(GameScreen i_GameScreen)
        {
            if(SpaceInvadersConfig.s_NumOfPlayers == SpaceInvadersConfig.eNumOfPlayers.OnePlayer)
            {
                SpaceInvadersConfig.s_NumOfPlayers = SpaceInvadersConfig.eNumOfPlayers.TwoPlayers;

                if (this.m_Players != null && SpaceInvadersConfig.s_NumOfPlayers == SpaceInvadersConfig.eNumOfPlayers.TwoPlayers &&
                    this.m_Players.Count < (int)SpaceInvadersConfig.eNumOfPlayers.TwoPlayers)
                {
                    this.m_Players.Add(new Player(i_GameScreen, PlayerIndex.Two, Keys.A, Keys.D, Keys.W, false, new Vector2(1, 0)));
                }
            }
            else
            {
                SpaceInvadersConfig.s_NumOfPlayers = SpaceInvadersConfig.eNumOfPlayers.OnePlayer;
            }
        }

        public void HandleHit(Enemy i_Enemy, ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet && (i_Collidable as Bullet).Type != Bullet.eBulletType.EnemyBullet)
            {
                i_Enemy.Animations["dyingEnemy"].Restart();
                this.m_SoundManager.PlaySoundEffect("EnemyKill");

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

            this.m_SoundManager.PlaySoundEffect("MotherShipKill");
        }

        public void HandleHit(Wall i_Wall, ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet && i_Wall.LastCollisionPixelsIndex.Count > 0)
            {
                this.deletePixelsInVerticalDirection(i_Wall as CollidableSprite, i_Collidable as CollidableSprite);
                this.m_SoundManager.PlaySoundEffect("BarrierHit");
            }
            else if(i_Collidable is Enemy)
            {
                this.HandleWallAndEnemyHit(i_Wall, i_Collidable as Enemy);
            }
        }

        private void HandleWallAndEnemyHit(Wall i_wall, Enemy i_Enemy)
        {

            for (int y = i_wall.m_LastCollisionRectangle.Top ; y < i_wall.m_LastCollisionRectangle.Bottom ; y++)
            {
                for (int x = i_wall.m_LastCollisionRectangle.Left; x < i_wall.m_LastCollisionRectangle.Right; x++)
                {
                    i_wall.Pixels[(x - i_wall.Bounds.Left) + ((y - i_wall.Bounds.Top) * i_wall.Bounds.Width)] = new Color(0, 0, 0, 0);
                }
            }

            i_wall.CurrTexture.SetData(i_wall.Pixels);
            this.clearCollisionData(i_wall, i_Enemy);

        }

        private void deletePixelsInVerticalDirection(CollidableSprite i_Target, CollidableSprite i_Sender)
        {
            int targetStartColomn = this.getHittenSpritesColomnInPixelsArray(i_Target, i_Sender);
            int targetRow = this.getHittenSpritesRowInPixelsArray(i_Target, i_Sender);
            int senderMinY, senderMaxY;

            if (i_Sender.Velocity.Y < 0) 
            {
                senderMinY = 0;
                senderMaxY = (int)(SpaceInvadersConfig.k_sizeOfBulletHitEffect * i_Sender.Texture.Height) + 1;
            }
            else
            {
                senderMinY = (int)((1 - SpaceInvadersConfig.k_sizeOfBulletHitEffect) * i_Sender.Texture.Height);
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
                       (targetColomn + (targetRow * i_Target.Texture.Width)) < i_Target.Pixels.Length &&
                        targetColomn + (targetRow * i_Target.Texture.Width ) > 0)
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
            int wallColomn = (int)(i_HittenSprite as CollidableSprite).LastCollisionPixelsIndex[0].Y;

            if (i_Sender.Velocity.Y < 0)
            {
                wallColomn -= (int)(SpaceInvadersConfig.k_sizeOfBulletHitEffect * i_Sender.Texture.Height);
            }

            return wallColomn;
        }
    }
}
