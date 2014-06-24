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

namespace SRV
{
    public partial class Server_fm : Form
    {
        #region cons
        private static Socket sServer;
        private static readonly List<Socket> sClient = new List<Socket>();
        private const int buffer = 2048;
        private const int port = 100;
        private static readonly byte[] _buffer = new byte[buffer];
		public const string ManagerKey = "a5a050400a31ee2c1b3b61b4a437cd881cb06497";
		public const string ControlKey = "4add06161bbcf668b576f581b6bb621aba5a812f";
        ArrayList clients;
        struct ClientInfo
        {
            public Socket socket;
            public string ip;
			public string type;
           // public string subnet;
        }
        ClientInfo clientInfo = new ClientInfo();
		//database db = new database();   
        #endregion
        
        public Server_fm()
        {
            InitializeComponent();
            clients = new ArrayList();
            SetupServer();
			//db.StartServer(true);
            //http.Start.Run();
        }
        public void SetupServer()
        {
             textBox1.Text+="Start server...\r\n";
            sServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//_serverSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));
			sServer.Bind(new IPEndPoint(IPAddress.Loopback, port));
            sServer.Listen(5000);
            sServer.BeginAccept(AcceptCallback, null);            
        }
        public void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;
            try { socket = sServer.EndAccept(AR); }
            catch (ObjectDisposedException) { return; }
            sClient.Add(socket);           
            socket.BeginReceive(_buffer, 0, buffer, SocketFlags.None, ReceiveCallback, socket);
           sServer.BeginAccept(AcceptCallback, null);
                       
        }
        public void ReceiveCallback(IAsyncResult AR)
        {
            #region sock
            Socket current = (Socket)AR.AsyncState;            
            int received;

            try
            {                
                received = current.EndReceive(AR);                
            }
            catch (SocketException)
            {                
                sClient.Remove(current);
                clientInfo.socket = current;
                clientInfo.ip = current.RemoteEndPoint.ToString();
                clients.Remove(clientInfo);
                current.Close();
                return;
            }
            #endregion
            byte[] recBuf = new byte[received];
            Array.Copy(_buffer, recBuf, received);
            string[] masd = Encoding.ASCII.GetString(recBuf).Split(':');
            switch (masd[0].ToLower())
            {
				#region Auth
                case "auth":
                    {
						if (masd[1].ToLower() == "control") { byte[] data = Encoding.ASCII.GetBytes("auth:"+ControlKey); current.Send(data); }
						else if (masd[1].ToLower() == "manager") { byte[] data = Encoding.ASCII.GetBytes("auth:"+ManagerKey); current.Send(data); }

						clientInfo.socket = current;
						clientInfo.ip = current.RemoteEndPoint.ToString();
						clientInfo.type = (masd[1].ToLower()=="control") ? "control" : "manager";
						clients.Add(clientInfo);
                    } break;
               #endregion
				#region update
                case "update":{ byte[] data = Encoding.ASCII.GetBytes("update:ok"); foreach (ClientInfo clientInfo in clients) clientInfo.socket.Send(data); } break;
                #endregion
				#region update params
                case "param":{
                    byte[] data = Encoding.ASCII.GetBytes("param:" + masd[1].ToLower());
                    foreach (ClientInfo clientInfo in clients)
                    if (current.RemoteEndPoint.ToString() != clientInfo.ip)
                        clientInfo.socket.Send(data);
                } break;
                    #endregion
				#region params
				case "setting":
					{
						byte[] data = Encoding.ASCII.GetBytes("param:setting:"+masd[1] );
						foreach (ClientInfo clientInfo in clients)
							if (current.RemoteEndPoint.ToString() != clientInfo.ip)
								clientInfo.socket.Send(data);
					} break;
				#endregion
                #region process
                case "getprocess":
					{
						byte[] data = Encoding.ASCII.GetBytes("getprocess");
							foreach (ClientInfo clientInfo in clients)
								if (current.RemoteEndPoint.ToString() != clientInfo.ip && clientInfo.type != "manager")
									clientInfo.socket.Send(data);								
					} break;
				case "loadprocess":
					{
						byte[] data = Encoding.ASCII.GetBytes("process:"+masd[1]);
                        foreach (ClientInfo clientInfo in clients)
                        {
                            clientInfo.socket.Send(data);                            
                        }

					} break;
                #endregion                
                #region Pc status
                case "status":
                    {                        
                            byte[] data = Encoding.ASCII.GetBytes("status:"+clients.Count+"|"+clients.Count);
                            foreach (ClientInfo clientInfo in clients)
                            {                                
                                if (clientInfo.type == "manager")
                                    clientInfo.socket.Send(data);
                            }  
                    } break;                
                #endregion         
            }
            #region text box srv
            try
            {
            MethodInvoker inv = new MethodInvoker(delegate {
                if (masd[0] == "param" && masd.Length>2) textBox1.Text += current.RemoteEndPoint + " " + masd[0] + " " + masd[1] + "\r\n";
                else textBox1.Text += current.RemoteEndPoint + " " + masd[0] + "\r\n";
            }); this.Invoke(inv);
            
                current.BeginReceive(_buffer, 0, buffer, SocketFlags.None, ReceiveCallback, current);
            }
            catch (Exception) { current.Disconnect(true); }
            #endregion
        }
        
    }
}
