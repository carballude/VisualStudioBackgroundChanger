using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSWallpaperChanger.Controller
{
    interface WallpaperProvider
    {
        List<string> Wallpapers { get; }
    }
}
