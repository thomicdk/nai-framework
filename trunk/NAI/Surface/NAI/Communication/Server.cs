using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using NAI.UI.Client;
using NAI.Client;
using NAI.Communication.SocketLayer;
using NAI.Communication.MessageLayer;

namespace NAI.Communication
{

    /// <summary>
    /// Server class starts itself, and broadcast its exsitence via UDP, for the client to pick up, and connect on the 
    /// same IP address using the correct port number.
    /// </summary>
    internal sealed class Server 
    {
        #region Static fields, methods and properties

        public readonly static String Identifier = Properties.Settings.Default.UdpCallSign;
        public readonly static byte[] msg = System.Text.Encoding.UTF8.GetBytes(Identifier);
        public readonly static String UDPIPgroup = "224.0.2.0";
        public readonly static int UDPPortGroup = 10035;
        //public readonly static int UDPBroadcastDelay = 3000;

        public readonly static int TcpServerPort = 8888;
        public readonly static int TcpMaxClientQueue = 10;

        private static Server _server;
        

        public static Server Instance
        {
            get { 
                if ( _server == null)
                    _server = new Server();
                return _server;
                }
        }
        #endregion

        #region instance fields
         
            private bool _running = true;
            private Thread _TcpServerThread;
            private Thread _UdpServerThread;

        #endregion

        #region Constructor(s)

        private Server() 
        {
            // TCP...
            _TcpServerThread = new Thread(new ThreadStart(RunTcpServer));
            _TcpServerThread.Start();
            // Allow the Tcp Server to be fully functioning!
            Thread.Sleep(1000);
            
            // Setup UDP message broadcaster
            _UdpServerThread = new Thread(new ThreadStart(RunUdpServer));
            _UdpServerThread.Start();

        }

        #endregion
        
        #region methods

        private void RunTcpServer()
        {
            Socket _serverSocket =  new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, TcpServerPort));
            _serverSocket.Listen(TcpMaxClientQueue);
            Socket _clientSocket = null;
            while(_running)
            {

                try
                {
                    Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Listening for incomming requests....");
                    _clientSocket = _serverSocket.Accept();
                    Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Got client: " + _clientSocket.ToString());

                    SocketCommunication clientComm = new SocketCommunication(_clientSocket);
                    MessageCommunication messageComm = new MessageCommunication(clientComm);
                    ClientSessionsController.Instance.AddClient(new ClientSession(messageComm));
                    
                    _clientSocket = null;
                }
                catch (SocketException se)
                {
                    Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Closing server because TcpServer failed: " + se.Message.ToString());
                    _running = false;
                }

            }
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "TcpServer stopped");
            if (_serverSocket != null && _serverSocket.Connected)            
                _serverSocket.Close();
            
            _serverSocket = null;            
        }



        private void RunUdpServer()
        {
            // Setup
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, UDPPortGroup);
            s.Bind(ipep);
            
            IPAddress ip = IPAddress.Parse(UDPIPgroup);
            s.SetSocketOption(SocketOptionLevel.IP,
                SocketOptionName.AddMembership,
                    new MulticastOption(ip, IPAddress.Any));

            byte[] incomming = new byte[512];
            int numberOfBytesRecieved = 0;

            while (_running)
            {
                try
                {
                    Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Waiting for incomming UDP message.");
                    EndPoint epSendTo = (EndPoint) new IPEndPoint(IPAddress.Any, UDPPortGroup);
                    numberOfBytesRecieved = s.ReceiveFrom(incomming, 0, incomming.Length, SocketFlags.None, ref epSendTo);
                    Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Got UDP package!");
                    if (numberOfBytesRecieved == msg.Length)
                    {
                        String token = Encoding.UTF8.GetString(incomming, 0, numberOfBytesRecieved).Trim();
                        Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Token: " + token);
                        if (token.Equals(Identifier))
                        {
                            s.SendTo(msg, epSendTo);
                            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "UDP response sent!");
                        }

                    }
                    
                    
                    
                }
                catch (SocketException se)
                {
                    Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Closing server because UdpServer failed: " + se.Message.ToString());
                    _running = false;
                }
            }
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "UdpServer stopped");
            if (s != null && s.Connected)            
                s.Close();

            
            s = null;
            ipep = null;
        }

        private void stopRunning()
        {
            _running = false;
        }
         #endregion 

        private static X509Certificate2 _serverCertificate = null;
        public static X509Certificate2 ServerCertificate
        {
            get
            {
                if (_serverCertificate == null)
                {
                    X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2Collection storeCollection = (X509Certificate2Collection)store.Certificates;

                    string certificateSubject = Properties.Settings.Default.ServerCertificateSubject;
                    X509Certificate2Collection findResults = storeCollection.Find(X509FindType.FindBySubjectDistinguishedName, certificateSubject, true);
                    if (findResults.Count > 0)
                    {
                        _serverCertificate = findResults[0];
                    }
                }
                return _serverCertificate;
            }
        }
    }
}
