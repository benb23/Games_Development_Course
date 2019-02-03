﻿using System;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class MainMenuScreen : MenuScreen
    {
        private Background m_Background;
        private MenuHeader m_MenuHeader;
        private ISpaceInvadersEngine m_GameEngine;

        public MainMenuScreen(Game i_Game) : base(i_Game)
        {
            IsUsingKeyboard = true;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MenuHeader = new MenuHeader(this, @"Screens\MainMenu\MainMenuLogo");
            this.m_MenuHeader.OffsetX = 20;

            int index = 0;
            ToggleItem playersItem = new ToggleItem(@"Screens\MainMenu\PlayersWhite", @"Screens\MainMenu\PlayersOptions_70x50", this, index++);
            playersItem.ToggleValueChanched += new EventHandler<EventArgs>(numOfPlayersChanged);
            AddMenuItem(playersItem);

            ClickItem SettingsScreenItem = new ClickItem("ScreenSettingsScreen", @"Screens\MainMenu\ScreenSettings", this, index++);
            ClickItem SoundSettingsItem = new ClickItem("SoundSettingsScreen", @"Screens\MainMenu\SoundSettings", this, index++);
            ClickItem playItem = new ClickItem("PlayScreen", @"Screens\MainMenu\PlayGameWhite", this, index++);
            ClickItem QuitItem = new ClickItem("Quit", @"Screens\Wellcome\QuitGame", this, index++);

            SettingsScreenItem.ItemClicked += new EventHandler<ScreenEventArgs>(handleItemClicked);
            SoundSettingsItem.ItemClicked += new EventHandler<ScreenEventArgs>(handleItemClicked);
            playItem.ItemClicked += new EventHandler<ScreenEventArgs>(handleItemClicked);
            QuitItem.ItemClicked += new EventHandler<ScreenEventArgs>(quit);

            AddMenuItem(SettingsScreenItem);
            AddMenuItem(SoundSettingsItem);
            AddMenuItem(playItem);
            AddMenuItem(QuitItem);
        }

        private void quit(object sender, ScreenEventArgs args)
        {
            Game.Exit();
        }

        private void handleItemClicked(object sender, ScreenEventArgs args)
        {
            MenuUtils.GoToScreen(this, this.m_ScreensManager.GetScreen(args.ScreenName));
        }

        private void numOfPlayersChanged(object sender, EventArgs args)
        {
            if (m_GameEngine == null)
            {
                m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            }

            m_GameEngine.ChangeNumOfPlayers(this.m_ScreensManager.GetScreen("PlayScreen"));
        }

        public override string ToString()
        {
            return "MainMenuScreen";
        }
    }
}
