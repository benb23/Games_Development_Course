using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IScreenSettingsManager
    {
        void ToggleMouseVisabilityConfig();

        void AllowWindowResizingConfigToggle_Click(object sender, EventArgs args);

        void FullScreenModeConfigToggle_Click(object sender, EventArgs args);
    }
}