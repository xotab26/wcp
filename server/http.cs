using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace http
{
    public class HttpProcessor
    {
        #region cons
        public TcpClient socket;
        public HttpServer srv;

        private Stream inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();

        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        #endregion
        #region Start
        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            this.socket = s;
            this.srv = srv;
        }
        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        public void process()
        {
            inputStream = new BufferedStream(socket.GetStream());

            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try
            {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET"))
                {
                    handleGETRequest();
                }
                else if (http_method.Equals("POST"))
                {
                    handlePOSTRequest();
                }
            }
            catch (Exception)
            {
                writeFailure();
            }
            outputStream.Flush();
            inputStream = null; outputStream = null; // bs = null;            
            socket.Close();
        }
        public void parseRequest()
        {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];
        }
        public void readHeaders()
        {
            String line;
            while ((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                   return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                httpHeaders[name] = value;
            }
        }
        public void handleGETRequest()
        {
            srv.handleGETRequest(this);
        }
        private const int BUF_SIZE = 4096;
        public void handlePOSTRequest()
        {
           int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                          content_len));
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("client disconnected during post");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            srv.handlePOSTRequest(this, new StreamReader(ms));

        }

        public void writeSuccess()
        {
            outputStream.Write("HTTP/1.0 200 OK\n");
            outputStream.Write("Content-Type: text/html\n");
            outputStream.Write("Connection: close\n");
            outputStream.Write("\n");
        }

        public void writeFailure()
        {
            outputStream.Write("HTTP/1.0 404 File not found\n");
            outputStream.Write("Connection: close\n");
            outputStream.Write("\n");
        }
    }

    public abstract class HttpServer
    {

        protected int port;
        TcpListener listener;
        bool is_active = true;

        public HttpServer(int port)
        {
            this.port = port;
        }

        public void listen()
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }
        #endregion
    public class HTTPD : HttpServer
    {
        public HTTPD(int port) : base(port) { }
        public override void handleGETRequest(HttpProcessor p)
        {
            p.writeSuccess();

            //p.outputStream.WriteLine("<html><head><meta charset='UTF-8'></head><body><h1>Тестовая страница VS 2013=)</h1>");
            //p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
            //p.outputStream.WriteLine("url : {0}", p.http_url);
            /*
            p.outputStream.WriteLine("<form method=post action=/form>");
            p.outputStream.WriteLine("<input type=text name=foo value=foovalue>");
            p.outputStream.WriteLine("<input type=submit name=bar value=barvalue>");
            p.outputStream.WriteLine("</form>");*/
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            /*string data = inputData.ReadToEnd();
            p.outputStream.WriteLine("<html><head><meta charset='UTF-8'></head>");
            p.outputStream.WriteLine("<body><h1>Администратирование системы</h1>");
            p.outputStream.WriteLine("<a href=/>return</a><p>");
            p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);
            */

        }
    }

    public class Start
    {
        public static int Run()
        {
            HttpServer httpServer;            
            httpServer = new HTTPD(8080);
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
            return 0;
        }

    }
}
