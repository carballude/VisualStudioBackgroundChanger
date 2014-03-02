using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSWallpaperChanger.Controller;

namespace VSWallpaperChanger
{
    public partial class MainWindow : Form
    {

        private WallpaperController _controller;
        private Thread _thread;

        public MainWindow()
        {
            _controller = WallpaperController.GetInstance();
            InitializeComponent();
            _controller.DownloadingWallpapers += _controller_DownloadingWallpapers;
            _controller.WallpaperChanged += _controller_WallpaperChanged;
        }

        void _controller_WallpaperChanged(object sender, EventArgs e)
        {
            statusBarLabel.Text = "Wallpaper has been changed!";
        }

        void _controller_DownloadingWallpapers(object sender, EventArgs e)
        {
            statusBarLabel.Text = "Changing wallpaper...";
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = !(notifyIcon1.Visible = true);
                notifyIcon1.ShowBalloonTip(500);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = !(ShowInTaskbar = true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_thread != null && _thread.IsAlive)
                _thread.Abort();
            _thread = new Thread(ChangeWallpaper);
            _thread.Start();
            
        }

        private void ChangeWallpaper()
        {
            _controller.NextWallpaper();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            changeWallpaperTimer.Stop();
            changeWallpaperTimer.Interval = (int)numericUpDown1.Value * 60 * 1000;
            changeWallpaperTimer.Start();
        }

        private void changeWallpaperTimer_Tick(object sender, EventArgs e)
        {
            if (_thread != null && _thread.IsAlive)
                _thread.Abort();
            _thread = new Thread(ChangeWallpaper);
            _thread.Start();
        }

        private void aboutVisualStudioWallpaperChangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

    }
}
