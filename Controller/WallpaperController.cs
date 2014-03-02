using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VSWallpaperChanger.Controller
{
    class WallpaperController
    {

        private const uint SPI_SETDESKWALLPAPER = 20;
        private const uint SPIF_UPDATEINIFILE = 0x01;
        private const uint SPIF_SENDWININICHANGE = 0x02;
        private const string WALLPAPER_NAME = "VSBackground.jpg";

        private static WallpaperController _INSTANCE = null;

        public event EventHandler DownloadingWallpapers;
        public event EventHandler WallpaperChanged;

        public static WallpaperController GetInstance()
        {
            if (_INSTANCE == null)
                _INSTANCE = new WallpaperController();
            return _INSTANCE;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        private List<WallpaperProvider> _providers;
        private WallpaperController()
        {
            _providers = new List<WallpaperProvider>() { new VisualStudioWallpapersProvider() };
        }

        private string SelectWallpaper()
        {
            DownloadingWallpapers(this, null);
            var urls = new List<string>();
            _providers.ForEach(x => urls.AddRange(x.Wallpapers));
            var index = new Random(DateTime.Now.Millisecond).Next(0, urls.Count - 1);
            return urls.ElementAt(index);
        }

        private void ChangeWallpaper(){
            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue(@"WallpaperStyle", "2");
            key.SetValue(@"TileWallpaper", "0");
            key.Close();
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Path.GetFullPath(WALLPAPER_NAME), SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        private void DownloadWallpaper(string url)
        {
            new WebClient().DownloadFile(url, WALLPAPER_NAME);
        }

        public void NextWallpaper()
        {
            DownloadWallpaper(SelectWallpaper());
            ChangeWallpaper();
            WallpaperChanged(this, null);
        }
    }
}
