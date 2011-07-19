using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Net;

namespace NAI.Communication.SocketLayer
{
    internal class SocketCommunication : ISocketLayerOutgoing
    {
        private  bool KeepRunning { get; set; }
        private bool _isRunning = false;

        public  ISocketLayerIncoming IncomingMessageHandler { get; set; }

        private Socket _clientSocket;
        private BinaryWriter _out;
        private BinaryReader _in;

        private List<byte[]> _messageQueue;

        static readonly object _queueLock = new object();

        public SocketCommunication(Socket socket)
        {           
            this._clientSocket = socket;
            SslStream sslStream = CreateSecureStream();
            this._out = new BinaryWriter(sslStream);
            this._in = new BinaryReader(sslStream);
            //this._out = new BinaryWriter(new NetworkStream(_clientSocket));
            //this._in = new BinaryReader(new NetworkStream(_clientSocket));
            _messageQueue = new List<byte[]>();
            Start();
        }

        private SslStream CreateSecureStream()
        {
            SslStream sslStream = new SslStream(new NetworkStream(_clientSocket), false);
            sslStream.AuthenticateAsServer(Server.ServerCertificate, false, SslProtocols.Tls, true);
            return sslStream;
        }

        public void Start()
        {
            if (!_isRunning)
            {
                Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Starting ClientCommunition. Client EndPoint: " + _clientSocket.RemoteEndPoint.ToString());
                _isRunning = true;
                KeepRunning = true;
                new Thread(new ThreadStart(RunOutgoing)).Start();
                new Thread(new ThreadStart(RunIncoming)).Start();
            }
        }

        private void RunIncoming()
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Running incoming-stream-thread");
            while (_clientSocket.Connected && KeepRunning)
            {
                try
                {
                    byte[] socketLayerMessage = ReadMessage();
                    if (IncomingMessageHandler != null)
                    {
                        IncomingMessageHandler.OnIncomingMessage(socketLayerMessage);
                    }
                }
                catch (EndOfStreamException ee)
                {
                    Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "ClientCommuniction: Got EndOfStreamException exception: " + ee.Message.ToString());
                    KeepRunning = false;                 
                }
                catch (IOException e)
                {
                    Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "ClientCommuniction: Got IO exception: " + e.Message.ToString());
                    KeepRunning = false;
                }
            }
            CloseAndCleanUp();
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Incoming-stream-thread ended");
        }

        private void RunOutgoing()
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Running outgoing-stream-thread");
            try
            {
                while (_clientSocket.Connected && KeepRunning)
                {
                    lock (_queueLock)
                    {
                        while (_messageQueue.Count > 0) // Continue until all messages in queue have been send
                        {
                            byte[] msg = _messageQueue[0];
                            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Sending message: " + msg.GetType().Name);
                            _messageQueue.RemoveAt(0);
                            _out.Write(MessageToPDU(msg));
                            _out.Flush();
                            Thread.Sleep(10); // Sleep two bits before sending next message
                        }
                    }                    
                    Thread.Sleep(10); // Sleep two bits before checking the queue again
                }
            }
            catch (Exception e)
            {
                Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "EXCEPTION: " + e.Message);
            }
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Outgoing-stream-thread ended");
        }


        private byte[] MessageToPDU(byte[] message)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            // Write the header of the PDU
            bw.Write(System.Net.IPAddress.HostToNetworkOrder(message.Length));
            // Write the payload of the PDU
            bw.Write(message);
            return ms.ToArray();
        }

        private byte[] ReadMessage()
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, "Reading message");

            // Read the header of the PDU
            int messageLength = IPAddress.NetworkToHostOrder(_in.ReadInt32());
            
            Debug.WriteLineIf(DebugSettings.DEBUG_COMMUNICATION, string.Format("Message length: {0}", messageLength));
            // Read the payload (the Message layer SDU)
            byte[] messageBytes = _in.ReadBytes(messageLength);
            return messageBytes;
        }

        private void CloseAndCleanUp()
        { 
            if (_clientSocket != null && _clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Close();
                }
                catch (Exception) { }
            }
            if (IncomingMessageHandler != null)
            {
                IncomingMessageHandler.OnConnectionLost();
                IncomingMessageHandler = null;
            }
            _isRunning = false;
        }

        #region ISocketLayerOutgoing Members

        void ISocketLayerOutgoing.SendMessage(byte[] message)
        {
            lock (_queueLock)
            {
                _messageQueue.Add(message);
            }
        }

        #endregion
    }
}
