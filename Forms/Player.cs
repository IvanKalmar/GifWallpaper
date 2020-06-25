using System.Drawing;
using System.Windows.Forms;

namespace wallpaperVideo
{
    public partial class Player : Form
    {
        PictureBox pictureBox;

        public Player(int width, int height)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            InitializeComponent();
            this.Left = 0;
            this.Top = 0;
            this.Size = new Size(width, height);
            this.BackColor = Color.Black;

            pictureBox = new PictureBox();
            pictureBox.Width = width;
            pictureBox.Height = height;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            this.Controls.Add(pictureBox);
        }

        public void SetImage(string path)
        {
            pictureBox.Image = Image.FromFile(path);
        }
    }
}
