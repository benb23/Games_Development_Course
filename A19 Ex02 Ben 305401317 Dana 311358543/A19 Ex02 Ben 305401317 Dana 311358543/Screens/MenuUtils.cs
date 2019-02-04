using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public static class MenuUtils 
    {
        public static void GoToScreen(GameScreen i_SourceScreen, GameScreen i_TargetScreen)
        {
            i_SourceScreen.ScreensManager.SetCurrentScreen(i_TargetScreen);
            if (i_TargetScreen is PlayScreen)
            {
                i_SourceScreen.ScreensManager.SetCurrentScreen(new LevelTransitionScreen(i_SourceScreen.Game));
            }

            i_SourceScreen.State = eScreenState.Inactive;
        }
    }
}
