using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace wallpaperVideo
{
    class Wallpaper
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        private static extern int SystemParametersInfo(UAction uAction, int uParam, StringBuilder lpvParam, int fuWinIni);
        private enum UAction
        {
            SPI_SETDESKWALLPAPER = 0x0014,
            SPI_GETDESKWALLPAPER = 0x0073,
        }


        public static string GetCurrent()
        {
            byte[] path = (byte[])Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop").GetValue("TranscodedImageCache");
            String wallpaper_file = Encoding.Unicode.GetString(SliceMe(path, 24)).TrimEnd("\0".ToCharArray());
            String reject_dir = System.IO.Path.GetTempPath();
            String new_path = reject_dir + "\\" + Path.GetFileName(wallpaper_file);

            if (!System.IO.Directory.Exists(reject_dir))
            {
                System.IO.Directory.CreateDirectory(reject_dir);
            }

            File.Copy(wallpaper_file, new_path);
            return new_path;
        }

        public static void SetCurrent(string fileName)
        {
            if (File.Exists(fileName))
            {
                StringBuilder s = new StringBuilder(fileName);
                SystemParametersInfo(UAction.SPI_SETDESKWALLPAPER, 0, s, 0x2);

            }
        }

        private static byte[] SliceMe(byte[] source, int pos)
        {
            byte[] destfoo = new byte[source.Length - pos];
            Array.Copy(source, pos, destfoo, 0, destfoo.Length);
            return destfoo;
        }
    }
}
