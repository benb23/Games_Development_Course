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
    class GameOverScreen : MenuScreen
    {
        private Background m_Background;
        private MenuHeader m_GameOverHeader;
        private MenuHeader m_Winner;
        private MenuHeader m_ResultAnnouncement;


        public GameOverScreen(Game i_Game) : base(i_Game, 0f,100f, 15f)
        {
            this.IsUsingKeyboard = false;
            this.m_ResultAnnouncement = new MenuHeader(this, @"Screens\GameOver\TheWinnerIs", -150f,250f);
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_Background.TintColor = Color.Red;
            this.m_GameOverHeader = new MenuHeader(this, @"Screens\GameOver\GameOverLogo");

            int index = 0;

            ClickItem QuitItem = new ClickItem("Quit", @"Screens\Wellcome\QuitGame", this, index++);
            ClickItem playItem = new ClickItem("PlayScreen", @"Screens\GameOver\Restart", this, index++);
            ClickItem mainMenuItem = new ClickItem("MainMenuScreen", @"Screens\Wellcome\MainMenu", this, index++);

            QuitItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnQuitItemClicked);
            playItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnItemClicked);
            mainMenuItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnItemClicked);

            AddMenuItem(QuitItem);
            AddMenuItem(playItem);
            AddMenuItem(mainMenuItem);
        }

        public override void Initialize()
        {
            ISpaceInvadersEngine engine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            if(engine.Winner == null)
            {
                m_Winner = new MenuHeader(this, @"Screens\GameOver\Tie", -10f, 250f);
            }
            else
            {
                m_Winner = new MenuHeader(this, @"Screens\GameOver\Player12_114x66", -10f, 250f);
            }
            base.Initialize();
        }
        private void OnQuitItemClicked(object sender, ScreenEventArgs args)
        {
            Game.Exit();
        }

        private void OnItemClicked(object sender, ScreenEventArgs args)
        {
            MenuUtils.GoToScreenAndExitCurrent(this, this.m_ScreensManager.GetScreen(args.ScreenName));
        }

        public override void Update(GameTime gameTime)
        {

            if (InputManager.KeyPressed(Keys.Escape))
            {
                OnQuitItemClicked(this, null);
            }
            else if(InputManager.KeyPressed(Keys.Home))
            {
                OnItemClicked(this, new ScreenEventArgs("PlayScreen"));
            }
            else if (InputManager.KeyPressed(Keys.T))
            {
                OnItemClicked(this, new ScreenEventArgs("MainMenuScreen"));
            }
            
            base.Update(gameTime);
        }

        public override string ToString()
        {
            return "GameOverScreen";
        }


    }
}
