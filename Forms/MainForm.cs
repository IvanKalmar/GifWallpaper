using System;
using System.Threading;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin;
using Microsoft.Win32;
using System.IO;

namespace wallpaperVideo
{
    public partial class MainForm : MaterialForm
    {
        string cache;

        DesktopDrawer drawer;
        Player player;

        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public MainForm()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            drawer = new DesktopDrawer();

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            this.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                notifyIcon1.Visible = false;
                this.player.Close();
            };

            this.Shown += (object sender, EventArgs e) =>
            {
                Hide();
                notifyIcon1.Visible = true;
            };

            this.Resize += (object sender, EventArgs e) =>
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    Hide();
                    notifyIcon1.Visible = true;
                }
            };

            notifyIcon1.Click += (object sender, EventArgs e) =>
            {
                Show();
                this.WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = false;
            };

            materialCheckBox1.Checked = rkApp.GetValue("GifWallpaper") != null;

            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GifWallpaper"
            );
            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            cache = Path.Combine(folder, "cache");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string file = File.ReadAllText(cache);

            this.player = new Player(
                SystemInformation.VirtualScreen.Width,
                SystemInformation.VirtualScreen.Height
            );

            if (File.Exists(file))
            {
                this.player.SetImage(file);
            }

            this.drawer.SetParent(player);
            player.Show();
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = ".gif";
            dlg.Filter = "GIF Files (*.gif)|*.gif";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(cache, dlg.FileName);
                this.player.SetImage(dlg.FileName);
            }
        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox1.Checked)
            {
                rkApp.SetValue("GifWallpaper", Application.ExecutablePath);
            }
            else
            {
                rkApp.DeleteValue("GifWallpaper", false);
            }
        }
    }
}
