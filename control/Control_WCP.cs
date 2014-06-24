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
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Net;


namespace control
{
    public partial class Control_WCP : Form
    {
        private static readonly Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static bool authkey = false;
        private const int _PORT = 100;
		public const string ControlKey = "4add06161bbcf668b576f581b6bb621aba5a812f";

        reboot os = new reboot();        
        blocks block = new blocks();
        usb usb = new usb();
        sys set = new sys();

		public static bool pTaskMgrOff = false;
		public static bool pLockOS = false;
		public static bool pKillProc = false;
		public static bool pBlockKeyBoard = false;
		public static bool pBlockUsb = false;
		public static bool pSyncTime = false;


		public bool pReboot = false;
		public bool pHReboot = false;
		public bool pHalt = false;
		public bool pHHalt = false;

        public Control_WCP()
        {            
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
			while (!_clientSocket.Connected)
			{ _clientSocket.Connect(IPAddress.Loopback, _PORT);
				Send("auth:control");	 }

            try
            { while (true) { Thread.Sleep(100); Receive(); } }
            catch (Exception) { }
        } 
        public void Send(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            _clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
        public void Receive()
        {
            var buffer = new byte[2048];
            int received = _clientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string[] masd = Encoding.ASCII.GetString(data).Split(':');
            if (data.Length < 0) return;
			switch (masd[0].ToLower())
			{
                case "auth": { Thread.Sleep(500); authkey = (masd[1].ToLower() == ControlKey) ? true : false; } break;
				case "update": { MessageBox.Show("Update"); } break;
				case "param": {
						        #region params
						switch (masd[1])
						{
                            case "reboot": { Command("pReboot"); } break;
                            case "hreboot": { Command("pHReboot"); } break;
                            case "halt": { Command("pHalt"); } break;
                            case "hhalt": { Command("pHHalt"); } break;
                            case "lockos": { Command("pLockOS"); } break;
							case "setting": {
							#region Settings
							string[] param = masd[2].ToLower().Split('/');
                            
							//pSyncTime = Convert.ToBoolean(param[0]);
							pTaskMgrOff = Convert.ToBoolean(param[1]);
							//pBlockUsb = Convert.ToBoolean(param[2]);
							pBlockKeyBoard = Convert.ToBoolean(param[3]);
							pKillProc = Convert.ToBoolean(param[4]);
                            Command("pTaskMgrOff");
                            Command("pBlockKeyBoard");
                            Command("pKillProc");
                            //MessageBox.Show(
                            //    "Синхронизация времени " + Convert.ToBoolean(param[0]) + "\r\n" +
                            //    "Отключение диспетчера задач " + Convert.ToBoolean(param[1]) + "\r\n" +
                            //    "Блокировка USB " + Convert.ToBoolean(param[2]) + "\r\n" +
                            //    "Блокировка клавиатуры " + Convert.ToBoolean(param[3]) + "\r\n" +
                            //    "Убийство процессов " + Convert.ToBoolean(param[4])
                            //    );

							#endregion
							} break;
						}
					} break;
						#endregion
				case "getprocess": { 
                    #region process
					string process=null;
					for (int i = 0; i < blocks.prockill.Length; i++)
					{
                        process += blocks.prockill[i] + "/";
						if (i == blocks.prockill.Length - 1) { Send("loadprocess: "+process); }
					}	
                    #endregion			
				} break;
				default:{} break;
			}
        }

        public void Command(String com)
        {
            switch (com)
            {
                case "pTaskMgrOff": { blocks.tblock(pTaskMgrOff); } break;
                case "pLockOS": { os.Lock(); } break;
                case "pKillProc": { blocks.killproc(pKillProc); } break;
                case "pBlockKeyBoard": { blocks.BlockInput(pBlockKeyBoard); } break;
                case "pBlockUsb": { } break;
                case "pSyncTime": { } break;
                case "pReboot": { os.halt(true,false); } break;
                case "pHReboot": { os.halt(true, true); } break;
                case "pHalt": { os.halt(false, false); } break;
                case "pHHalt": { os.halt(false, true); } break;
            }
        }
    }    
}