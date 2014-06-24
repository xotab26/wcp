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

namespace control
{
    public class reboot
    {
        // halt(true, false) //мягкая перезагрузка
        // halt(true, true) //жесткая перезагрузка
        // halt(false, false) //мягкое выключение
        // halt(false, true) //жесткое выключение
        // lock() //блокировка ос
        //импортируем API функцию InitiateSystemShutdown
        [DllImport("advapi32.dll", EntryPoint = "InitiateSystemShutdownEx")]
        static extern int InitiateSystemShutdown(string lpMachineName, string lpMessage, int dwTimeout, bool bForceAppsClosed, bool bRebootAfterShutdown);
        //импорт API функцию AdjustTokenPrivileges
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
        ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);
        //импорт API функцию GetCurrentProcess
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();
        //импорт API функцию OpenProcessToken
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);
        //импорт API функцию LookupPrivilegeValue
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);
        //импорт API функцию LockWorkStation
        [DllImport("user32.dll", EntryPoint = "LockWorkStation")]
        static extern bool LockWorkStation();
        //структурa TokPriv1Luid для работы с привилегиями
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }
        //объявляем необходимые, для API функций, константые значения, согласно MSDN
        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        //функция SetPriv для повышения привилегий процесса
        private void SetPriv()
        {
            TokPriv1Luid tkp; //экземпляр структуры TokPriv1Luid 
            IntPtr htok = IntPtr.Zero;
            //открываем "интерфейс" доступа для процесса
            if (OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok))
            {
                tkp.Count = 1;
                tkp.Attr = SE_PRIVILEGE_ENABLED;
                tkp.Luid = 0;
                //получаем системный идентификатор необходимой нам привилегии
                LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tkp.Luid);
                //повышем привилигеию процессу
                AdjustTokenPrivileges(htok, false, ref tkp, 0, IntPtr.Zero, IntPtr.Zero);
            }
        }
        //метод для перезагрузки/выключения машины
        public int halt(bool RSh, bool Force)
        {
            SetPriv(); 
            return InitiateSystemShutdown(null, null, 0, Force, RSh);
        }
        //метод для блокировки операционной системы
        public int Lock()
        {
            if (LockWorkStation())
                return 1;
            else
                return 0;
        }
    }
}
