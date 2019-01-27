using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ScreenSettingsManager : GameService, IScreenSettingsManager
    {
        public ScreenSettingsManager(GameScreen i_GameScren) : base(i_GameScren.Game)
        {

        }
    }
}
