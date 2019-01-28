using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IScreenSettingsManager
    {

        void ToggleMouseVisabilityConfig(object sender, EventArgs args);

        void ToggleAllowWindowResizingConfig(object sender, EventArgs args);

        void ToggleFullScreenModeConfig(object sender, EventArgs args);

    }
}
