using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Manager_WCP
{

    public partial class Manager_WCP : Form
    {
        private static readonly Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        #region Constants
        private const int port = 100;
        public static bool active = false;
        public bool authkey = false;
        public static bool connected;
		public const string ManagerKey = "a5a050400a31ee2c1b3b61b4a437cd881cb06497";
        public int n = 0;
        public int n2 = 0;
		public static bool pTaskMgrOff; //setting
		public static bool pLockOS;
		public static bool pKillProc; //setting
		public static bool pBlockKeyBoard; //setting
		public static bool pBlockUsb; //setting
		public static bool pSyncTime; //setting
        #endregion
        #region Func
        public static ArrayList aProces;
        public struct sProces
        {
            public string pName;
        }
        sProces process = new sProces();        
        #endregion

        public Manager_WCP()
        {
            InitializeComponent();            
            aProces = new ArrayList();
            //bgw.RunWorkerAsync();
            if (!_clientSocket.Connected)
            {

                _clientSocket.Connect(IPAddress.Loopback, port);
                if (_clientSocket.Connected) connected = true;
                Send("auth:manager");
                Send("status:");
               /* MethodInvoker inv = new MethodInvoker(delegate
                {
                    Send("auth:manager");
                    Send("status:");
                }); this.Invoke(inv);*/
            }
        }
           
        private void bUpdateall_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(_ => Send("update"));
        }
        
        private void bSetting_Click(object sender, EventArgs e)
        {			
            settings setting = new settings();            
            if (!active)
            {
                setting.Show();
                active = true;
                setting.Owner = this;
            }            
        }

        private void bRun_Click(object sender, EventArgs e)
        {
            run runs = new run();
            if (!active)
            {
                runs.Show();
                active = true;
                runs.Owner = this;
            } 
        }

		private void bProc_Click(object sender, EventArgs e)
		{
			if (!active)
			{
                Process proc = new Process();
                ThreadPool.QueueUserWorkItem(_ => Send("getprocess"));
                foreach (sProces sProc in aProces)
                {
                    proc.lProc.Items.Add(sProc.pName);
                }
				proc.Show();
				active = true;
				proc.Owner = this;                
			} 
		}
        public void Send(string text)
        {
            try {
                byte[] buffer = Encoding.ASCII.GetBytes(text);
                _clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                Receive();
            }
            catch (SocketException) { }			
        }       

        public void Receive()
        {            
            try
            {				
                var buffer = new byte[2048];
                int received = _clientSocket.Receive(buffer, SocketFlags.None);
                if (received < 0 || buffer == null) return;
                var data = new byte[received];
                Array.Copy(buffer, data, received);
                string[] masd = Encoding.ASCII.GetString(data).Split(':');
				switch (masd[0].ToLower())
                {
                    #region auth
                    case "auth": { authkey = (masd[1] == ManagerKey) ? true : false; } break;
                    #endregion
                    #region process
                    case "process":
						{
                            string[] getproc = masd[1].Split('/');                            
                            foreach (String setproc in getproc)
                            {
                                if (setproc.Length > 3)
                                {
                                    if (setproc.ToString() != process.pName)
                                    { process.pName = setproc.Replace(" ", string.Empty);
                                        aProces.Add(process); } else {
                                        aProces.Remove(process); }
                                }                          
                            }                            
							
					} break;
                    #endregion
                    #region pc info
                    case "status":
                        {
                            string[] type = masd[1].Split('|');
                            aPc.Text = "Всего ПК в сети = " + type[0];
                            sPc.Text = "Всего ПК в вашей подсети = " + type[1];
                            Subnet.Text = "Подсеть = ";
                        } break;
                    #endregion
                }
            }
            catch (SocketException) { _clientSocket.Disconnect(true); }
        }

		private void bgw_DoWork(object sender, DoWorkEventArgs e)
		{
            try
            {
                /*if (!_clientSocket.Connected)
                {
                    
                    _clientSocket.Connect(IPAddress.Loopback, port);
                    if(_clientSocket.Connected) connected = true;
                    
                    MethodInvoker inv = new MethodInvoker(delegate
                    {
                        Send("auth:manager");
                        Send("status:");
                    }); this.Invoke(inv);
                }
                else
                {
                    Thread.Sleep(60000);
                    MethodInvoker inv = new MethodInvoker(delegate
                    {
                        Send("pc:");
                    }); this.Invoke(inv);
                }*/
                
            }
            catch (Exception) { }              
                        
		}

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            try
            {
                #region auntification
                if (authkey)
                    {
                        bsettings.Enabled = true;
                        brun.Enabled = true;
                        bupdateall.Enabled = true;
                        bproc.Enabled = true;                        
                    }
                    #endregion                 
                
            }
            catch (Exception) { }
        }

        private void bUpdatePc_Click(object sender, EventArgs e)
        {
                        
        }

        
    }

}
