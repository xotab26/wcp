using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace control
{

    class sys
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime([In] ref SYSTEMTIME st);

        public void time(short year, short month, short day, short hour, short min, short sec)
        {
            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = year;
            st.wMonth = month;
            st.wDay = day;
            st.wHour = hour;
            st.wMinute = min;
            st.wSecond = sec;

            SetSystemTime(ref st);
            //set.time(2014, 8, 28, 22, 23, 23);    
        }
    }
}
