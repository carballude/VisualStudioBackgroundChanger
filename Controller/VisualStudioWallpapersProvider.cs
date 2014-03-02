using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VSWallpaperChanger.Controller
{
    class VisualStudioWallpapersProvider : WallpaperProvider
    {

        private List<string> _wallpapers;
        private static int _PAGES = 8;

        public List<string> Wallpapers
        {
            get { if (_wallpapers.Count == 0) PopulateWallpapers(); return _wallpapers; }
        }

        public VisualStudioWallpapersProvider()
        {
            _wallpapers = new List<string>();
        }

        private void PopulateWallpapers()
        {
            var wc = new WebClient();
                ParseWallpapersPage(wc.DownloadString("http://visualstudiowallpapers.com/"));
                Enumerable.Range(2, _PAGES).ToList().ForEach(x => ParseWallpapersPage(wc.DownloadString("http://visualstudiowallpapers.com/page/" + x)));
        }

        private void ParseWallpapersPage(string htmlCode) 
        {
            var lines = htmlCode.Split(new char[] { '\n' });
            var url = from x in lines where x.Contains("_1280.png?.jpg") select x.Split(new char[] { '"' })[1];
            url.ToList().ForEach(x => _wallpapers.Add(x));
        }
    }
}
