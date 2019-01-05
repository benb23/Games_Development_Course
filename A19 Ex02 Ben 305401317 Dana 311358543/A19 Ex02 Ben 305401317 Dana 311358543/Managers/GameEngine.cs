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

        private Game m_Game;
        private List<Player> m_Players;

        public GameEngine(Game i_Game) : base(i_Game)
        {
            this.m_Game = i_Game;
        }

        public List<Player> Players
        {
            get { return m_Players; }
            set { m_Players = value; }
        }

        public void HandleHit(ICollidable i_Sender, ICollidable i_Target)
        {
            if(i_Sender is SpaceShip)
            {
                HandleSpaceShipHit(i_Sender, i_Target);
            }
            else
            {
                handleNonSpaceShipHit(i_Sender, i_Target);
            }
        }

        private void HandleSpaceShipHit(ICollidable i_Target, ICollidable i_Sender)
        {
            if(i_Sender is Bullet)
            {
                if(m_Players[(int)PlayerIndex.One].SpaceShip == i_Target)
                {
                    m_Players[(int)PlayerIndex.One].Score += (int)eScoreValue.Soul;
                    m_Players[(int)PlayerIndex.One].Souls.Remove(m_Players[(int)PlayerIndex.One].Souls[0]);
                }
                else if(m_Players[(int)PlayerIndex.Two].SpaceShip == i_Target) 
                {
                    m_Players[(int)PlayerIndex.Two].Score += (int)eScoreValue.Soul;
                    m_Players[(int)PlayerIndex.Two].Souls.Remove(m_Players[(int)PlayerIndex.Two].Souls[0]);

                }
            }
            else // i_Sender is Enemy
            {
                ShowGameOverMessage();
                this.m_Game.Exit();
            }
        }

        private void handleNonSpaceShipHit(ICollidable i_Target, ICollidable i_Sender)
        {
            if(i_Target is MotherSpaceShip)
            {
                if((i_Sender as Bullet).Type == Bullet.eBulletType.PlayerOneBullet)
                {
                    m_Players[0].Score += (int)eScoreValue.MotherShip;
                }
                else if((i_Sender as Bullet).Type == Bullet.eBulletType.PlayerOneBullet)
                {
                    m_Players[1].Score += (int)eScoreValue.MotherShip;
                }
            }
            else //Enemy
            {
                if ((i_Sender as Bullet).Type == Bullet.eBulletType.PlayerOneBullet)
                {
                    updatePlayerScoreAfterHitEnemy(m_Players[0], i_Target as Enemy);
                }
                else if ((i_Sender as Bullet).Type == Bullet.eBulletType.PlayerOneBullet)
                {
                    updatePlayerScoreAfterHitEnemy(m_Players[1], i_Target as Enemy);
                }
            }
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
            else if(i_Enemy.TintColor == Color.Yellow)
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
    }
}
