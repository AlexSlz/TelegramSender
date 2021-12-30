using System;
using System.Windows.Forms;

namespace tgSender
{
    internal class IconManager
    {
        static System.Drawing.Icon appIcon = new System.Drawing.Icon("e.ico");
        static bool consoleVisible = true;
        public static void InstallIcon()
        {
            SetWindowIcon();
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Click += (s, e) =>
            {
                consoleVisible = !consoleVisible;
                SetConsoleWindowVisibility(consoleVisible);
            };
            notifyIcon.Icon = appIcon;
            notifyIcon.Visible = true;
            notifyIcon.Text = Application.ProductName;

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, (s, e) => { Application.Exit(); });
            notifyIcon.ContextMenuStrip = contextMenu;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static void SetConsoleWindowVisibility(bool visible)
        {
            IntPtr hWnd = FindWindow(null, Console.Title);
            if (hWnd != IntPtr.Zero)
            {
                if (visible) ShowWindow(hWnd, 1); //1 = SW_SHOWNORMAL           
                else ShowWindow(hWnd, 0); //0 = SW_HIDE               
            }
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
        private static void SetWindowIcon()
        {
            IntPtr mwHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            IntPtr result01 = SendMessage(mwHandle, 0x0080, 0, appIcon.Handle);
            IntPtr result02 = SendMessage(mwHandle, 0x0080, 1, appIcon.Handle);
        }
    }
}
