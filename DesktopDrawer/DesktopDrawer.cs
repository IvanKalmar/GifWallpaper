using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wallpaperVideo
{
    public class DesktopDrawer
    {
        IntPtr workerw;

        public DesktopDrawer()
        {
            IntPtr progman = W32.FindWindow("progman", null);

            IntPtr result = IntPtr.Zero;

            W32.SendMessageTimeout(progman,
                                   0x052C,
                                   new IntPtr(0),
                                   IntPtr.Zero,
                                   W32.SendMessageTimeoutFlags.SMTO_NORMAL,
                                   1000,
                                   out result);

            workerw = IntPtr.Zero;
            W32.EnumWindows(new W32.EnumWindowsProc((tophandle, topparamhandle) => {
                IntPtr p = W32.FindWindowEx(tophandle,
                                            IntPtr.Zero,
                                            "SHELLDLL_DefView",
                                            IntPtr.Zero);

                if (p != IntPtr.Zero)
                {
                    workerw = W32.FindWindowEx(IntPtr.Zero,
                                               tophandle,
                                               "WorkerW",
                                               IntPtr.Zero);
                }
                return true;
            }), IntPtr.Zero);
        }

        public void SetParent(Form form)
        {
            W32.SetParent(form.Handle, workerw);
        }
    }
}
