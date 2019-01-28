using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SettingsScreenManager : GameService, ISettingsScreenManager
    {
        public SettingsScreenManager(GameScreen i_GameScren) : base(i_GameScren.Game)
        {

        }
    }
}
